using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Ludwintor.Tools
{
    internal abstract class TextAnimation : MonoBehaviour
    {
        [Tooltip("0-based indexes of all characters that should be animated")]
        [SerializeField]
        private List<Segment> charactersToAnimate = new List<Segment>();

        [SerializeField]
        private bool playOnAwake = false;

        private const float frameRate = 20f;
        private const float timeBetweenAnimates = 1f / frameRate;

        private float lastAnimateTime;
        private TextMeshProUGUI textComponent;
        private TMP_TextInfo textInfo;
        private TMP_MeshInfo[] cachedMeshInfo;

        public bool UseUnscaledTime { get; set; }

        protected List<Segment> CharactersToAnimate => charactersToAnimate;

        protected float TimeForTimeScale => UseUnscaledTime ? Time.realtimeSinceStartup : Time.time;

        private TextMeshProUGUI TextComponent
        {
            get
            {
                if (textComponent == null)
                    textComponent = GetComponent<TextMeshProUGUI>();

                return textComponent;
            }
        }

        public void AddCharsToAnimate(int firstChar, int lastChar) => charactersToAnimate.Add(new Segment(firstChar, lastChar));

        public void ClearSegments() => charactersToAnimate.Clear();

        public void CacheTextMeshInfo()
        {
            textInfo = TextComponent.textInfo;
            cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
        }

        public abstract void LoadPreset(ILibrary library, string presetKey);

        public void AnimateAllChars()
        {
            lastAnimateTime = TimeForTimeScale;
            int characterCount = textInfo.characterCount;

            if (characterCount == 0)
                return;

            for (int i = 0; i < characterCount; i++)
            {
                // Skip characters that aren't supposed to animate
                if (!ShouldAnimate(i))
                    continue;

                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                // Skip invisible characters
                if (!charInfo.isVisible)
                    continue;

                int materialIndex = charInfo.materialReferenceIndex;
                int vertexIndex = charInfo.vertexIndex;

                Vector3[] sourceVertices = cachedMeshInfo[materialIndex].vertices;

                // Center point of mesh
                Vector2 charMidBaseline = (sourceVertices[vertexIndex + 0] + sourceVertices[vertexIndex + 2]) / 2;

                Vector3 offset = charMidBaseline;

                Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;

                // Apply offset from center
                destinationVertices[vertexIndex + 0] = sourceVertices[vertexIndex + 0] - offset;
                destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] - offset;
                destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] - offset;
                destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] - offset;

                // Set translation/rotation/scale
                Animate(i, out Vector2 translation, out float rotation, out float scale);
                Matrix4x4 matrix = Matrix4x4.TRS(translation, Quaternion.Euler(0f, 0f, rotation), scale * Vector3.one);

                // Apply transformation
                destinationVertices[vertexIndex + 0] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 0]);
                destinationVertices[vertexIndex + 1] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 1]);
                destinationVertices[vertexIndex + 2] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 2]);
                destinationVertices[vertexIndex + 3] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 3]);

                destinationVertices[vertexIndex + 0] += offset;
                destinationVertices[vertexIndex + 1] += offset;
                destinationVertices[vertexIndex + 2] += offset;
                destinationVertices[vertexIndex + 3] += offset;
            }

            ApplyChangesToMesh();
        }

        protected virtual void Awake()
        {
            enabled = playOnAwake;
        }

        protected virtual void Start()
        {
            TextComponent.ForceMeshUpdate();
            lastAnimateTime = float.MinValue;
        }

        protected virtual void OnEnable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
        }

        protected virtual void OnDisable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);

            // Reset standart text mesh
            TextComponent.ForceMeshUpdate();
        }

        protected virtual void Update()
        {
            if (TimeForTimeScale > lastAnimateTime + timeBetweenAnimates)
            {
                AnimateAllChars();
            }
        }

        /// <summary>
        /// Individual characters animation
        /// </summary>
        protected abstract void Animate(int characterIndex, out Vector2 translation, out float rotation, out float scale);

        private void ApplyChangesToMesh()
        {
            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                TextComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }
        }

        private bool ShouldAnimate(int index)
        {
            foreach (Segment segment in charactersToAnimate)
                if (segment.Contains(index))
                    return true;

            return false;
        }

        private void OnTextChanged(UnityEngine.Object obj)
        {
            if (obj == TextComponent)
                CacheTextMeshInfo();
        }
    }
}