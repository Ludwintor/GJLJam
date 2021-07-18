using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ludwintor.Tools.Editor
{
    internal class LocalizationTextEditor : EditorWindow
    {
        public SerializedProperty CurrentProp;
        public Font font;

        private GenericMenu copyPasteMenu;
        private GUIStyle textStyle;
        private Font defaultFont;

        public static void Start(string tag, string language, SerializedProperty textProp, Font font)
        {
            LocalizationTextEditor window = GetWindow<LocalizationTextEditor>($"[{language}: {tag}]");

            window.CurrentProp = textProp;
            window.font = font;
        }

        private void OnEnable()
        {
            copyPasteMenu = new GenericMenu();

            copyPasteMenu.AddItem(new GUIContent("Copy"), false, () =>
            {
                EditorGUIUtility.systemCopyBuffer = CurrentProp.stringValue;
            });

            copyPasteMenu.AddItem(new GUIContent("Paste"), false, () =>
            {
                CurrentProp.stringValue = EditorGUIUtility.systemCopyBuffer;
                CurrentProp.serializedObject.ApplyModifiedProperties();
            });

            defaultFont = Font.CreateDynamicFontFromOSFont("LiberationSans", 12);
        }

        private void OnGUI()
        {
            if (CurrentProp == null)
                return;

            if (textStyle == null)
            {
                textStyle = new GUIStyle(EditorStyles.textArea);
                textStyle.font = font ?? defaultFont;
                textStyle.fontSize = 13;
            }

            if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
            {
                copyPasteMenu.ShowAsContext();
            }

            CurrentProp.stringValue = EditorGUILayout.TextArea(CurrentProp.stringValue, textStyle, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            CurrentProp.serializedObject.ApplyModifiedProperties();
        }
    }
}