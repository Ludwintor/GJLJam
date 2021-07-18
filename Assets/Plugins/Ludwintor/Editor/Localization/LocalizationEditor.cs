using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Ludwintor.Tools.Editor
{
    internal class LocalizationEditor : EditorWindow
    {
        private const string Path = "Localization";

        private Vector2 scrollPosition;

        private GUIStyle textFieldStyle;
        private GUIStyle buttonStyle;
        private GUIStyle textPreviewStyle;
        private bool isInited;

        private LocalizationData localization;
        private List<LanguageData> languages;

        private LanguageData redactingData;
        private string redactingName;

        [MenuItem("Game/Localization Editor")]
        public static void Start() => Start(null);

        public static void Start(LocalizationData localization)
        {
            LocalizationEditor window = GetWindow<LocalizationEditor>("Localization Editor");

            window.localization = localization;
            window.Show();
        }

        private void OnValidate()
        {
            isInited = false;
        }

        private void Init()
        {
            if (isInited)
                return;

            isInited = true;
            textFieldStyle = new GUIStyle("TextField");
            textFieldStyle.alignment = TextAnchor.MiddleLeft;
            textFieldStyle.fontSize = 15;

            buttonStyle = new GUIStyle("Button");
            buttonStyle.alignment = TextAnchor.MiddleLeft;
            buttonStyle.fontSize = 15;

            textPreviewStyle = new GUIStyle("Tooltip");
            textPreviewStyle.wordWrap = true;
            textPreviewStyle.clipping = TextClipping.Clip;
            textPreviewStyle.fontSize = 13;

            LoadData();
        }

        private void OnGUI()
        {
            Init();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            EditorGUILayout.BeginHorizontal("HelpBox");

            GUILayout.Label("Tags", "Tooltip", GUILayout.Width(150)); // 200 overall
            AddTagButton();

            EditorGUILayout.Space(5, false);
            ShowAllLanguages();
            EditorGUILayout.Space(5, false);
            AddLanguageButton();

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);

            if (languages.Any() && localization.Tags != null)
            {
                for (int i = 0; i < localization.Tags.Count; i++)
                {
                    LocalizationTag tag = localization.Tags[i];
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.BeginVertical("HelpBox", GUILayout.Width(182), GUILayout.Height(50));
                    EditorGUILayout.BeginHorizontal();

                    if (RemoveTagButton(i))
                        continue;

                    SerializedObject obj = new SerializedObject(localization);
                    SerializedProperty tagProp = obj.FindProperty($"Tags").GetArrayElementAtIndex(i);

                    SerializedProperty nameProp = tagProp.FindPropertyRelative("key");
                    EditorGUILayout.PropertyField(nameProp, GUIContent.none);

                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.Space(25, false);
                    SerializedProperty typeProp = tagProp.FindPropertyRelative("resourceType");

                    EditorGUILayout.BeginVertical(); 
                    EditorGUILayout.Space(2);
                    EditorGUILayout.PropertyField(typeProp, GUIContent.none);

                    EditorGUILayout.EndVertical(); 

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.Space(5, false);
                    DrawResourcesFields(tag, i);

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space(5);
                    obj.ApplyModifiedProperties();
                }
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawResourcesFields(LocalizationTag tag, int index)
        {
            for (int i = 0; i < languages.Count; i++)
            {
                LanguageData data = languages[i];
                LocalizationResource resource = data.Resources[index];

                EditorGUILayout.BeginVertical("HelpBox", GUILayout.Width(278), GUILayout.Height(51));

                SerializedObject obj = new SerializedObject(data);
                SerializedProperty resourceProp = obj.FindProperty("Resources").GetArrayElementAtIndex(index);

                switch (tag.ResourceType)
                {
                    case LocalizationResourceType.Text:
                        SerializedProperty textProp = resourceProp.FindPropertyRelative("stringValue");

                        EditorGUILayout.BeginHorizontal(); // Text preview
                        GUILayout.Label(textProp.stringValue, textPreviewStyle, GUILayout.Width(180), GUILayout.Height(45));

                        EditorGUILayout.BeginVertical(); // Font property and edit text button
                        EditorGUILayout.PropertyField(resourceProp.FindPropertyRelative("fontValue"), GUIContent.none);
                        EditorGUILayout.Space(1);
                        if (GUILayout.Button("Edit Text"))
                        {
                            LocalizationTextEditor.Start(tag.Key, data.Name, textProp, resource.FontValue?.sourceFontFile);
                        }

                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();

                        break;
                    case LocalizationResourceType.Image:
                        SerializedProperty spriteProp = resourceProp.FindPropertyRelative("spriteValue");
                        EditorGUILayout.PropertyField(spriteProp, GUIContent.none);
                        break;
                    case LocalizationResourceType.Texture:
                        SerializedProperty textureProp = resourceProp.FindPropertyRelative("textureValue");
                        EditorGUILayout.PropertyField(textureProp, GUIContent.none);
                        break;
                    case LocalizationResourceType.Audio:
                        SerializedProperty audioProp = resourceProp.FindPropertyRelative("audioValue");
                        EditorGUILayout.PropertyField(audioProp, GUIContent.none);
                        break;
                }

                EditorGUILayout.EndVertical();
            }
        }

        private void LoadData()
        {
            if (localization == null)
            {
                LocalizationData[] loadedDatas = Resources.LoadAll<LocalizationData>(Path);
                if (loadedDatas.Any())
                {
                    localization = loadedDatas[0];
                }
                else
                {
                    localization = CreateInstance<LocalizationData>();
                    AssetDatabase.CreateAsset(localization, "Assets/Resources/Localization/Localization.asset");
                    AssetDatabase.SaveAssets();
                }
            }

            languages = Resources.LoadAll<LanguageData>(Path).ToList();
        }

        private void AddTagButton()
        {
            if (GUILayout.Button("+", GUILayout.Width(25), GUILayout.Height(25)))
            {
                localization.AddTag();
                foreach (LanguageData language in languages)
                    language.AddResource();
            }
        }

        private bool RemoveTagButton(int index)
        {
            if (GUILayout.Button("-", "MiniButton", GUILayout.Width(25)))
            {
                localization.RemoveTag(index);
                foreach (LanguageData language in languages)
                    language.RemoveResource(index);

                return true;
            }

            return false;
        }

        private void AddLanguageButton()
        {
            if (GUILayout.Button("Add Language", GUILayout.Height(25), GUILayout.Width(100)))
            {
                LanguageData newLanguage = CreateInResources();
                if (localization.Tags.Any())
                {
                    for (int i = 0; i < localization.Tags.Count; i++)
                    {
                        newLanguage.AddResource();
                    }
                }

                localization.Languages.Add(newLanguage.Name);
                languages.Add(newLanguage);
            }
        }

        private void ShowAllLanguages()
        {
            for (int i = 0; i < languages.Count; i++)
            {
                LanguageData data = languages[i];

                if (data == redactingData)
                {
                    if (Event.current.isKey && Event.current.keyCode == KeyCode.Return)
                    {
                        SerializedObject obj = new SerializedObject(redactingData);
                        string path = AssetDatabase.GetAssetPath(redactingData);
                        AssetDatabase.RenameAsset(path, redactingName);


                        obj.FindProperty("language").stringValue = redactingName;
                        obj.ApplyModifiedProperties();
                        data.name = redactingName;
                        localization.Languages[i] = redactingName;

                        redactingData = null;
                    }
                    redactingName = GUILayout.TextField(redactingName, textFieldStyle, GUILayout.Width(250), GUILayout.Height(25));
                }
                else
                {
                    if (GUILayout.Button(data.name, buttonStyle, GUILayout.Width(250), GUILayout.Height(25)))
                    {
                        redactingData = data;
                        redactingName = data.name;
                    }
                }

                if (GUILayout.Button("-", GUILayout.Width(25), GUILayout.Height(25)))
                {
                    if (EditorUtility.DisplayDialog("Deleting language data", $"Are you sure you want to delete [{data.Name}] data and all it resources?", "Delete", "Cancel"))
                    {
                        //AssetDatabase.DeleteAsset(path + data.name + ".asset");
                        string path = AssetDatabase.GetAssetPath(data);
                        AssetDatabase.DeleteAsset(path);
                        AssetDatabase.SaveAssets();
                        languages.Remove(data);
                        localization.Languages.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        [MenuItem("Game/Localization/Create Data")]
        public static LanguageData CreateInResources()
        {
            LanguageData result = CreateInstance<LanguageData>();
            SerializedObject obj = new SerializedObject(result);
            obj.FindProperty("language").stringValue = "NewLanguage";
            obj.ApplyModifiedProperties();

            string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Localization/NewLanguage.asset");
            AssetDatabase.CreateAsset(result, path);
            AssetDatabase.SaveAssets();

            return result;
        }
    }
}