using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ludwintor.Tools
{
    internal sealed class TextParser
    {
        private static readonly string[] unityTags = new string[]
        {
            "align",
            "alpha",
            "color",
            "b",
            "i",
            "cspace",
            "font",
            "indent",
            "line-height",
            "link",
            "lowercase",
            "uppercase",
            "smallcaps",
            "margin",
            "mark",
            "mspace",
            "noparse",
            "nobr",
            "page",
            "pos",
            "size",
            "space",
            "sprite",
            "s",
            "u",
            "style",
            "sub",
            "sup",
            "voffset",
            "width"
        };

        private static readonly string[] customTags = new string[]
        {
            CustomTags.delay,
            CustomTags.anim
        };

        public static List<TextSymbol> CreateSymbolList(string text)
        {
            List<TextSymbol> symbolList = new List<TextSymbol>();
            int parsedCharacters = 0;
            while (parsedCharacters < text.Length)
            {
                string remainingText = text.Substring(parsedCharacters, text.Length - parsedCharacters);
                TextSymbol symbol;
                if (TextTag.StringStartsWithTag(remainingText))
                {
                    TextTag tag = TextTag.FindNextTag(remainingText);
                    symbol = new TextSymbol(tag);
                }
                else
                    symbol = new TextSymbol(remainingText[0]);

                parsedCharacters += symbol.Length;
                symbolList.Add(symbol);
            }

            return symbolList;
        }

        public static string RemoveAllTags(string text)
        {
            text = RemoveUnityTags(text);
            text = RemoveCustomTags(text);

            return text;
        }

        public static string RemoveUnityTags(string text) => RemoveTags(text, unityTags);
        public static string RemoveCustomTags(string text) => RemoveTags(text, customTags);

        private static string RemoveTags(string text, params string[] tags)
        {
            foreach (string tag in tags)
                text = TextTag.RemoveTagsFromString(text, tag);

            return text;
        }

        public class TextSymbol
        {
            public char Character { get; private set; }
            public TextTag Tag { get; private set; }

            public TextSymbol(char character)
            {
                Character = character;
            }

            public TextSymbol(TextTag tag)
            {
                Tag = tag;
            }

            public string Text => IsTag ? Tag.FullTag : Character.ToString();

            public bool IsTag => Tag != null;

            public int Length => Text.Length;

            public bool IsReplacedWithSprite => IsTag && Tag.Type == "sprite";

            public float GetFloatParameter(float defaultValue = 0f)
            {
                if (!IsTag)
                {
                    Debug.LogWarning("Attempted to retrieve parameter from symbol that is not a tag");
                    return defaultValue;
                }

                if (float.TryParse(Tag.Parameter, out float paramValue))
                    return paramValue;

                string warning = string.Format("Found invalid parameter format in tag [{0}]. Parameter[{1}] does not parse to a float", Tag, Tag.Parameter);
                Debug.LogWarning(warning);
                return defaultValue;
            }
        }

        internal struct CustomTags
        {
            public const string delay = "delay";
            public const string anim = "anim";
        }
    }
}