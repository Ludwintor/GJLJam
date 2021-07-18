using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ludwintor.Tools
{
    [CreateAssetMenu(fileName = "WaveLibrary", menuName = "Text Typer/Wave Library")]
    internal class WaveLibrary : Library<WavePreset>
    {
        [SerializeField]
        private List<WavePreset> wavePresets;
        public override List<WavePreset> Presets => wavePresets;
    }

    [Serializable]
    internal class WavePreset : Preset
    {
        [Tooltip("Time offset between each character when calculation animation. 0 makes all characters move together.")]
        [Range(0f, 0.5f)]
        public float timeOffsetPerChar = 0f;
        [Space]

        [Tooltip("Curve showing x-position delta over time.")]
        public AnimationCurve xPosCurve;
        [Tooltip("x-position curve is multiplied by this value")]
        [Range(0, 20)]
        public float xPosMultiplier = 0f;
        [Space]

        [Tooltip("Curve showing y-position delta over time.")]
        public AnimationCurve yPosCurve;
        [Tooltip("y-position curve is multiplied by this value")]
        [Range(0, 20)]
        public float yPosMultiplier = 0f;
        [Space]

        [Tooltip("Curve showing 2D rotation delta over time.")]
        public AnimationCurve rotationCurve;
        [Tooltip("2D rotation curve is multiplied by this value")]
        [Range(0, 90)]
        public float rotationMultiplier = 0f;
        [Space]

        [Tooltip("Curve showing scale delta over time.")]
        public AnimationCurve scaleCurve;
        [Tooltip("Scale curve is multiplied by this value")]
        [Range(0, 10)]
        public float scaleMultiplier = 0f;
    }
}