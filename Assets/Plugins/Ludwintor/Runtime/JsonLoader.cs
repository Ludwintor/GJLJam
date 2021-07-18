using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Ludwintor.Tools
{
    public static class JsonLoader
    {
        /// <summary>
        /// Load specific type object from json file
        /// </summary>
        /// <typeparam name="T">Type of deserialized object</typeparam>
        /// <param name="path">Full path of json file</param>
        /// <returns>Deserialized object with type</returns>
        public static T LoadFromJson<T>(string path)
        {
            string json = Resources.Load<TextAsset>(path).text;

            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Load specific dictionary from json file
        /// </summary>
        /// <typeparam name="TKey">Dictionary key type</typeparam>
        /// <typeparam name="TValue">Dictionary value type</typeparam>
        /// <param name="path">Full path of json file</param>
        /// <returns>Deserialized dictionary</returns>
        public static Dictionary<TKey, TValue> LoadFromJson<TKey, TValue>(string path)
        {
            return LoadFromJson<Dictionary<TKey, TValue>>(path);
        }

        public static void SaveToJson<T>(string path, T objectToSave)
        {
            path = Path.Combine(Application.dataPath, "Resources", path);
            path += ".json";

            if (!File.Exists(path))
                File.Create(path);

            string json = JsonConvert.SerializeObject(objectToSave, Formatting.Indented);

            File.WriteAllText(path, json);
        }
    }
}