using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ludwintor.Tools
{
    [CreateAssetMenu(fileName = "ShakeLibrary", menuName = "Text Typer/Shake Library")]
    internal sealed class ShakeLibrary : Library<ShakePreset>
    {
        [SerializeField]
        private List<ShakePreset> shakePresets;
        public override List<ShakePreset> Presets => shakePresets;
    }

    [Serializable]
    internal class ShakePreset : Preset
    {
        [Range(0, 20)]
        [Tooltip("Amount of x-axis shake to apply")]
        public float xPosShake = 0f;

        [Range(0, 20)]
        [Tooltip("Amount of y-axis shake to apply")]
        public float yPosShake = 0f;

        [Range(0, 90)]
        [Tooltip("Amount of rotational shake to apply")]
        public float rotationShake = 0f;

        [Range(0, 10)]
        [Tooltip("Amount of scale shake to apply")]
        public float scaleShake = 0f;
    }
}