using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ludwintor.Tools
{
    internal sealed class ShakeAnimation : TextAnimation
    {
        [SerializeField]
        [Tooltip("Unnecessary to assign if using tags")]
        private ShakeLibrary shakeLibrary;

        [SerializeField]
        [Tooltip("Unnecessary to assign if using tags")]
        private string shakePresetKey;

        private ShakePreset shakePreset;

        public override void LoadPreset(ILibrary library, string presetKey)
        {
            shakeLibrary = (ShakeLibrary)library;
            shakePresetKey = presetKey;
            shakePreset = shakeLibrary[presetKey];
        }

        protected override void OnEnable()
        {
            if (shakeLibrary != null && !string.IsNullOrEmpty(shakePresetKey))
                LoadPreset(shakeLibrary, shakePresetKey);

            base.OnEnable();
        }

        protected override void Animate(int characterIndex, out Vector2 translation, out float rotation, out float scale)
        {
            translation = Vector2.zero;
            rotation = 0f;
            scale = 1f;

            if (shakePreset == null)
                return;

            float randomX = Random.Range(-shakePreset.xPosShake, shakePreset.xPosShake);
            float randomY = Random.Range(-shakePreset.yPosShake, shakePreset.yPosShake);
            translation = new Vector2(randomX, randomY);

            rotation = Random.Range(-shakePreset.rotationShake, shakePreset.rotationShake);

            scale = 1f + Random.Range(-shakePreset.scaleShake, shakePreset.scaleShake);
        }
    }
}