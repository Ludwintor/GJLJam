using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ludwintor.Tools
{
    internal class WaveAnimation : TextAnimation
    {
        [SerializeField]
        [Tooltip("Unnecessary to assign if using tags")]
        private WaveLibrary waveLibrary;

        [SerializeField]
        [Tooltip("Unnecessary to assign if using tags")]
        private string wavePresetKey;

        private WavePreset wavePreset;

        private float timeAnimationStarted;

        public override void LoadPreset(ILibrary library, string presetKey)
        {
            waveLibrary = (WaveLibrary)library;
            wavePresetKey = presetKey;
            wavePreset = waveLibrary[presetKey];
        }

        protected override void OnEnable()
        {
            if (waveLibrary != null && !string.IsNullOrEmpty(wavePresetKey))
                LoadPreset(waveLibrary, wavePresetKey);

            timeAnimationStarted = TimeForTimeScale;
            base.OnEnable();
        }

        protected override void Animate(int characterIndex, out Vector2 translation, out float rotation, out float scale)
        {
            translation = Vector2.zero;
            rotation = 0f;
            scale = 1f;

            if (wavePreset == null)
                return;

            // Calculate t as offset per character (for wave effects)
            float t = TimeForTimeScale - timeAnimationStarted + (characterIndex * wavePreset.timeOffsetPerChar);

            float xPos = wavePreset.xPosCurve.Evaluate(t) * wavePreset.xPosMultiplier;
            float yPos = wavePreset.yPosCurve.Evaluate(t) * wavePreset.yPosMultiplier;

            translation = new Vector2(xPos, yPos);

            rotation = wavePreset.rotationCurve.Evaluate(t) * wavePreset.rotationMultiplier;

            scale += wavePreset.scaleCurve.Evaluate(t) * wavePreset.scaleMultiplier;
        }
    }
}