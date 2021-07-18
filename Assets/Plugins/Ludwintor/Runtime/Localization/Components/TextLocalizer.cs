using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Ludwintor.Tools
{
    public class TextLocalizer : Localizer
    {
        [SerializeField, Tooltip("If true, then localizer will use font from localization resource")]
        private bool grabFont;

        // I dont think we should cache components cuz user don't want to change language often
        protected override void LanguageChanged()
        {
            TextMeshProUGUI textComponent = GetComponent<TextMeshProUGUI>();
            string text = Controller.GetString(key, out TMP_FontAsset font);
            textComponent.SetText(text);

            if (grabFont && font != null)
                textComponent.font = font;
        }
    }
}