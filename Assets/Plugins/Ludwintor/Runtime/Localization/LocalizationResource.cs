using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Ludwintor.Tools
{
    [Serializable]
    public class LocalizationResource
    {
        public string StringValue => stringValue;
        public TMP_FontAsset FontValue => fontValue;
        public Sprite SpriteValue => spriteValue;
        public Texture TextureValue => textureValue;
        public AudioClip AudioValue => audioValue;

        [SerializeField]
        private string stringValue;
        [SerializeField]
        private TMP_FontAsset fontValue;
        [SerializeField]
        private Sprite spriteValue;
        [SerializeField]
        private Texture textureValue;
        [SerializeField]
        private AudioClip audioValue;
    }
}