using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Ludwintor.Tools.Editor
{
    [CustomEditor(typeof(LocalizationData))]
    internal class LocalizationDataEditor : UnityEditor.Editor
    {
        private GUIStyle greenHeader;

        private void OnEnable()
        {
            greenHeader = new GUIStyle();
            greenHeader.fontSize = 20;
            greenHeader.normal.textColor = Color.green;
        }

        public override void OnInspectorGUI()
        {
            LocalizationData data = target as LocalizationData;

            if (data.Tags.Count > 0)
            {
                EditorGUILayout.BeginVertical();

                EditorGUILayout.LabelField("Tags:", greenHeader);
                EditorGUILayout.Space(5);

                int count = data.Tags.Count(tag => tag.ResourceType == LocalizationResourceType.Text);
                DrawLabelStat("Text", count);
                count = data.Tags.Count(tag => tag.ResourceType == LocalizationResourceType.Image);
                DrawLabelStat("Image", count);
                count = data.Tags.Count(tag => tag.ResourceType == LocalizationResourceType.Texture);
                DrawLabelStat("Texture", count);
                count = data.Tags.Count(tag => tag.ResourceType == LocalizationResourceType.Audio);
                DrawLabelStat("Audio", count);

                EditorGUILayout.EndVertical();
            }

            if (GUILayout.Button("Open Editor Window", GUILayout.ExpandWidth(true)))
            {
                LocalizationEditor.Start(data);
            }

        }

        private void DrawLabelStat(string stat, int count)
        {
            if (count > 0)
                EditorGUILayout.LabelField($"{stat}: {count}");
        }
    }
}