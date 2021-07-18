using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ludwintor.Tools
{
    public abstract class Localizer : MonoBehaviour
    {
        [SerializeField]
        protected string key;

        protected LocalizationController Controller => LocalizationController.Current;

        private void Start()
        {
            LanguageChanged();
        }

        private void OnEnable()
        {
            LocalizationController.OnLanguageChanged += LanguageChanged;
        }

        private void OnDisable()
        {
            LocalizationController.OnLanguageChanged -= LanguageChanged;
        }

        protected abstract void LanguageChanged();
    }
}