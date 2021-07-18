using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ludwintor.Tools
{
    internal abstract class Library<T> : ScriptableObject, ILibrary where T : Preset
    {
        public abstract List<T> Presets { get; }

        public T this[string key] => FindPreset(key) ?? throw new KeyNotFoundException();

        public bool ContainsKey(string key) => FindPreset(key) != null;

        private T FindPreset(string key)
        {
            foreach (T preset in Presets)
            {
                if (preset.name.ToLower() == key.ToLower())
                    return preset;
            }

            return null;
        }
    }

    internal abstract class Preset
    {
        public string name;
    }
}