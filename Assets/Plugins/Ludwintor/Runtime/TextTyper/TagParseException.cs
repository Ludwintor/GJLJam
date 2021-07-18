using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ludwintor.Tools
{
    public sealed class TagParseException : Exception
    {
        public string Text { get; private set; }

        public TagParseException(string text) 
            : base("Exception occured during parsing text for tags.")
        {
            Text = text;
        }
    }
}