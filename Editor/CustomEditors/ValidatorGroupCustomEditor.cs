using GibFrame.Editor.Validators;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GibFrame.Editor
{
    [CustomEditor(typeof(ValidationGroup))]
    internal class ValidatorGroupCustomEditor : UnityEditor.Editor
    {
        private ValidationGroup validationGroup;
        private bool showResults = false;
        private List<ValidatorFailure> latestResults = new List<ValidatorFailure>();
        private Vector2 scrollPos;
        private bool resultsFoldout = false;
        private GUIStyle labelStyle;
        private Texture checkIcon;
        private Texture errorIcon;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            this.PropertyField("validables", "Validables");

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Validate"))
            {
                latestResults = validationGroup.ValidateAll();
                showResults = true;
            }
            EditorGUI.BeginDisabledGroup(!showResults);
            if (GUILayout.Button("Clear"))
            {
                showResults = false;
                latestResults.Clear();
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
            if (showResults)
            {
                var hasErrors = latestResults.Count > 0;
                var content = new GUIContent()
                {
                    text = $" Validation completed with {latestResults.Count} errors",
                    image = hasErrors ? errorIcon : checkIcon
                };

                EditorGUILayout.LabelField(content, labelStyle);
                resultsFoldout = EditorGUILayout.Foldout(resultsFoldout, "Results");
                if (resultsFoldout)
                {
                    scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    foreach (var f in latestResults)
                    {
                        EditorGUILayout.LabelField(new GUIContent(f.ToString()), labelStyle);
                    }
                    EditorGUILayout.EndScrollView();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void OnEnable()
        {
            errorIcon = Resources.Icons.Error().Resize(24, 24);
            checkIcon = Resources.Icons.Check().Resize(24, 24);
            validationGroup = target as ValidationGroup;
            labelStyle = new GUIStyle()
            {
                wordWrap = true,
                imagePosition = ImagePosition.ImageLeft,
                padding = new RectOffset(0, 0, 10, 10)
            };
            labelStyle.normal.textColor = Color.white;
        }
    }
}