using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Ludwintor.Tools
{
    public class LocalizationData : ScriptableObject
    {
        public List<string> Languages = new List<string>();
        public List<LocalizationTag> Tags = new List<LocalizationTag>();

        public void AddTag()
        {
            string uniqueKey = "newTag";
            IEnumerable<string> keys = Tags.Select(tag => tag.Key);
            int i = 0;
            while (keys.Contains(uniqueKey))
            {
                uniqueKey = $"newTag({i})";
                i++;
            }

            Tags.Add(new LocalizationTag(uniqueKey));
        }

        public void RemoveTag(int index)
        {
            Tags.RemoveAt(index);
        }
    }
}