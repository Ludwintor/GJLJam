using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace Ludwintor.Tools
{
    public class LocalizationController
    {
        public delegate void LanguageChanged();
        public static event LanguageChanged OnLanguageChanged;
        public static LocalizationController Current => instance;
        private static LocalizationController instance;

        public LanguageData CurrentLanguage => currentLanguage;

        private Dictionary<string, LocalizationResourceType> resourceTypeByTag = new Dictionary<string, LocalizationResourceType>();
        private Dictionary<string, LocalizationResource> currentResources = new Dictionary<string, LocalizationResource>();

        private LanguageData currentLanguage;
        private List<LocalizationTag> tags;


        public LocalizationController()
        {
            instance = this;
        }

        public string GetString(string key) => GetString(key, out _);

        public string GetString(string key, out TMP_FontAsset font)
        {
            font = null;
            if (CheckType(key, LocalizationResourceType.Text))
            {
                LocalizationResource resource = currentResources[key];
                font = resource.FontValue;
                return resource.StringValue;
            }

            return string.Empty;
        }

        public Sprite GetSprite(string key)
        {
            if (CheckType(key, LocalizationResourceType.Image))
            {
                LocalizationResource resource = currentResources[key];
                return resource.SpriteValue;
            }

            return null;
        }

        public Texture GetTexture(string key)
        {
            if (CheckType(key, LocalizationResourceType.Texture))
            {
                LocalizationResource resource = currentResources[key];
                return resource.TextureValue;
            }

            return null;
        }

        public AudioClip GetAudio(string key)
        {
            if (CheckType(key, LocalizationResourceType.Audio))
            {
                LocalizationResource resource = currentResources[key];
                return resource.AudioValue;
            }

            return null;
        }

        public void SetLanguage(LanguageData language)
        {
            currentLanguage = language;
            ClearResources();

            AddResources(language.Resources);

            OnLanguageChanged?.Invoke();
        }

        public void AddTags(List<LocalizationTag> tags)
        {
            this.tags = tags;
            foreach (LocalizationTag tag in tags)
                resourceTypeByTag.Add(tag.Key, tag.ResourceType);
        }

        public void AddResources(List<LocalizationResource> resources)
        {
            for (int i = 0; i < resources.Count; i++)
            {
                LocalizationResource resource = resources[i];
                currentResources.Add(tags[i].Key, resource);
            }
        }

        public void ClearResources() => currentResources.Clear();

        private bool CheckType(string key, LocalizationResourceType type)
        {
            if (resourceTypeByTag.TryGetValue(key, out LocalizationResourceType typeToCheck))
                return typeToCheck == type;
                    

            throw new KeyNotFoundException("There's no tag with this key");
        }
    }
}