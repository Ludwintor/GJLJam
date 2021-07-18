using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Ludwintor.Tools
{
    /// <summary>
    /// Use <see cref="TypeText(string, float)"/> to type text
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class TextTyper : MonoBehaviour
    {
        private const float printDelaySetting = 0.05f;
        private const float punctuationDelayMultiplier = 8f;

        private static readonly List<char> punctuationCharacters = new List<char>
        {
            '.',
            ',',
            '!',
            '?'
        };

        public event Action onPrintCompleted;
        /// <summary>
        /// Pass every character that printed. PASS "Sprite" STRING IN PLACE OF SPRITE CHARACTER!
        /// </summary>
        public event Action<string> onCharacterPrinted;

        [SerializeField]
        private ShakeLibrary shakeLibrary = null;

        [SerializeField]
        private WaveLibrary waveLibrary = null;

        [SerializeField]
        private bool useUnscaledTime = false;

        private TextMeshProUGUI textComponent;
        private float defaultPrintDelay;
        private List<TypableCharacter> charactersToType;
        private List<TextAnimation> animations;
        private Coroutine typeTextCoroutine;

        public bool IsTyping => typeTextCoroutine != null;

        private TextMeshProUGUI TextComponent
        {
            get
            {
                if (textComponent == null)
                    textComponent = GetComponent<TextMeshProUGUI>();

                return textComponent;
            }
        }

        private void Awake()
        {
            animations = new List<TextAnimation>();
            GetComponents(animations);
        }

        /// <summary>
        /// Types text with delay after each character. Try always specify printDelay parameter.
        /// </summary>
        public void TypeText(string text, float printDelay = -1)
        {
            CleanupCoroutine();

            foreach (TextAnimation animation in animations)
                animation.ClearSegments();

            defaultPrintDelay = printDelay > 0 ? printDelay : printDelaySetting;
            ProcessTags(text);

            TMP_TextInfo textInfo = TextComponent.textInfo;
            textInfo.ClearMeshInfo(false);

            typeTextCoroutine = StartCoroutine(TypeTextCharByChar(text));
        }

        /// <summary>
        /// Skips the typing to the end
        /// </summary>
        public void Skip()
        {
            CleanupCoroutine();

            TextComponent.maxVisibleCharacters = int.MaxValue;
            UpdateMeshAndAnims();

            OnTypewritingComplete();
        }

        // TODO: Make a way to configure this
        public bool IsSkippable() => IsTyping;

        private void CleanupCoroutine()
        {
            if (typeTextCoroutine != null)
            {
                StopCoroutine(typeTextCoroutine);
                typeTextCoroutine = null;
            }
        }

        private IEnumerator TypeTextCharByChar(string text)
        {
            TextComponent.text = TextParser.RemoveCustomTags(text);
            for (int i = 0; i < charactersToType.Count; i++)
            {
                TextComponent.maxVisibleCharacters = i + 1;
                UpdateMeshAndAnims();
                TypableCharacter printedChar = charactersToType[i];
                CharacterPrinted(printedChar.ToString());

                if (useUnscaledTime)
                    yield return new WaitForSecondsRealtime(printedChar.Delay);
                else
                    yield return new WaitForSeconds(printedChar.Delay);
            }

            typeTextCoroutine = null;
            OnTypewritingComplete();
        }

        private void UpdateMeshAndAnims()
        {
            TextComponent.ForceMeshUpdate();

            foreach (TextAnimation animation in animations)
                animation.AnimateAllChars();
        }

        private void ProcessTags(string text)
        {
            if (charactersToType == null)
                charactersToType = new List<TypableCharacter>();
            charactersToType.Clear();

            List<TextParser.TextSymbol> textAsSymbolList = TextParser.CreateSymbolList(text);

            int printedCharCount = 0;
            int customTagOpenIndex = 0;
            string customTagParam = "";
            float nextDelay = defaultPrintDelay;

            foreach (TextParser.TextSymbol symbol in textAsSymbolList)
            {
                if (symbol.IsTag && !symbol.IsReplacedWithSprite)
                {
                    switch (symbol.Tag.Type)
                    {
                        case TextParser.CustomTags.delay:
                            nextDelay = DelayTag(symbol);
                            break;
                        case TextParser.CustomTags.anim:
                            AnimTag(symbol, ref customTagParam, ref customTagOpenIndex, printedCharCount);
                            break;
                        default:
                            OtherTag();
                            break;
                    }
                }
                else
                {
                    printedCharCount++;
                    TypableCharacter characterToType = new TypableCharacter();
                    if (symbol.IsTag && symbol.IsReplacedWithSprite)
                        characterToType.IsSprite = true;
                    else
                        characterToType.Char = symbol.Character;

                    characterToType.Delay = nextDelay;
                    if (punctuationCharacters.Contains(symbol.Character))
                        characterToType.Delay *= punctuationDelayMultiplier;

                    charactersToType.Add(characterToType);
                }    
            }
        }

        private float DelayTag(TextParser.TextSymbol symbol)
        {
            float nextDelay = symbol.Tag.IsClosingTag ? defaultPrintDelay : symbol.GetFloatParameter(defaultPrintDelay);
            return nextDelay;
        }

        private void AnimTag(TextParser.TextSymbol symbol, ref string customTagParam, ref int customTagOpenIndex, int printedCharCount)
        {
            if (symbol.Tag.IsClosingTag)
            {
                TextAnimation anim = null;
                if (CheckAnimation(customTagParam, shakeLibrary))
                {
                    anim = animations.Find(animation => animation is ShakeAnimation);
                    if (anim == null)
                        anim = AddAnimation<ShakeAnimation>();

                    anim.LoadPreset(shakeLibrary, customTagParam);
                }
                else if (CheckAnimation(customTagParam, waveLibrary))
                {
                    anim = animations.Find(animation => animation is WaveAnimation);
                    if (anim == null)
                        anim = AddAnimation<WaveAnimation>();

                    anim.LoadPreset(waveLibrary, customTagParam);
                }
                else
                {
                    throw new Exception($"No animation preset found with key [{customTagParam}]");
                }

                anim.UseUnscaledTime = useUnscaledTime;
                anim.AddCharsToAnimate(customTagOpenIndex, printedCharCount - 1);
                anim.enabled = true;
            }
            else
            {
                customTagOpenIndex = printedCharCount;
                customTagParam = symbol.Tag.Parameter;
            }
        }

        private void OtherTag()
        {
            // This tag is unity tag, but it might be other tag.
            // TODO: Throw exception if it is not a unity tag
        }

        private TextAnimation AddAnimation<T>() where T : TextAnimation
        {
            TextAnimation anim = gameObject.AddComponent<T>();
            animations.Add(anim);
            return anim;
        }

        private bool CheckAnimation<T>(string animName, Library<T> library) where T : Preset => library.ContainsKey(animName);

        private void CharacterPrinted(string printedCharacter) => onCharacterPrinted?.Invoke(printedCharacter);
        private void OnTypewritingComplete() => onPrintCompleted?.Invoke();

        private class TypableCharacter
        {
            public char Char { get; set; }
            public float Delay { get; set; }
            public bool IsSprite { get; set; }
            public override string ToString() => IsSprite ? "Sprite" : Char.ToString();
        }
    }
}