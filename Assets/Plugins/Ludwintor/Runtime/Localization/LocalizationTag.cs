using System;
using UnityEngine;

namespace Ludwintor.Tools
{
    [Serializable]
    public class LocalizationTag
    {
        public string Key => key;
        public LocalizationResourceType ResourceType => resourceType;

        public LocalizationTag(string key)
        {
            this.key = key;
        }

        [SerializeField]
        private string key;

        [SerializeField]
        private LocalizationResourceType resourceType;
    }
}