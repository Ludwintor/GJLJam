using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ludwintor.Tools
{
    public class ImageLocalizer : Localizer
    {
        // I dont think we should cache components cuz user don't want to change language often
        protected override void LanguageChanged()
        {
            Image imageComponent = GetComponent<Image>();
            Sprite sprite = Controller.GetSprite(key);

            imageComponent.sprite = sprite;
        }
    }
}