using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ludwintor.Tools
{
    public class LanguageData : ScriptableObject
    {
        public string Name => language;

        [SerializeField]
        private string language;

        public List<LocalizationResource> Resources = new List<LocalizationResource>();

        public void AddResource()
        {
            Resources.Add(new LocalizationResource());
        }

        public void RemoveResource(int index)
        {
            Resources.RemoveAt(index);
        }
    }
}