using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Ludwintor.Tools
{
    internal sealed class TextTag
    {
        private const char openingDelimeter = '<';
        private const char closingDelimeter = '>';
        private const char endTagDelimeter = '/';
        private const char parameterDelimeter = '=';

        /// <summary>
        /// Full tag text. Example: &lt;/delay=0.5&gt;
        /// </summary>
        public string FullTag { get; private set; }

        public TextTag(string tag)
        {
            FullTag = tag;
        }

        public bool IsClosingTag => FullTag.Length > 2 && FullTag[1] == endTagDelimeter;

        public bool IsOpeningTag => !IsClosingTag;

        public int Length => FullTag.Length;

        public string ClosingTag => IsClosingTag ? FullTag : $"</{Type}>";

        /// <summary>
        /// Get tag type. Example: Tag &lt;/delay=0.5&gt; has type "delay"
        /// </summary>
        public string Type
        {
            get
            {
                // </delay=0.5> to delay=0.5
                string type = FullTag.Substring(1, FullTag.Length - 2);
                type = type.TrimStart(endTagDelimeter);

                // delay=0.5 to delay
                int delimeterIndex = type.IndexOf(parameterDelimeter);
                if (delimeterIndex > 0)
                    type = type.Substring(0, delimeterIndex);

                return type;
            }
        }

        public string Parameter
        {
            get
            {
                int parameterIndex = FullTag.IndexOf(parameterDelimeter);
                if (parameterIndex < 0)
                    return string.Empty;

                int parameterLength = FullTag.Length - parameterIndex - 2;
                string parameter = FullTag.Substring(parameterIndex + 1, parameterLength);

                // Erase optional quotes in parameter
                if (parameter.Length > 1 && parameter[0] == '\"' && parameter[parameter.Length - 1] == '\"')
                    parameter = parameter.Substring(1, parameter.Length - 2);

                return parameter;
            }
        }

        public static TextTag FindNextTag(string text)
        {
            int openingDelimeterIndex = text.IndexOf(openingDelimeter);
            if (openingDelimeterIndex < 0)
                throw new TagParseException(text);

            int closingDelimeterIndex = text.IndexOf(closingDelimeter);
            if (closingDelimeterIndex < 0 || closingDelimeterIndex < openingDelimeterIndex)
                throw new TagParseException(text);

            string tag = text.Substring(openingDelimeterIndex, closingDelimeterIndex - openingDelimeterIndex + 1);
            return new TextTag(tag);
        }

        public static string RemoveTagsFromString(string text, string tagType)
        {
            string textWithoutTags = text;
            int parsedCharacters = 0;
            while (parsedCharacters < text.Length)
            {
                string remainingText = text.Substring(parsedCharacters, text.Length - parsedCharacters);
                if (StringStartsWithTag(remainingText))
                {
                    TextTag tag = FindNextTag(remainingText);
                    if (tag.Type == tagType)
                        textWithoutTags = textWithoutTags.Replace(tag.FullTag, string.Empty);

                    parsedCharacters += tag.Length - 1;
                }
                parsedCharacters++;
            }

            return textWithoutTags;
        }

        public static bool StringStartsWithTag(string text) => text.StartsWith(openingDelimeter.ToString());
    }
}