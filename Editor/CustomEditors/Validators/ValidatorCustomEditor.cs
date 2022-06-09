using GibFrame.Editor.Validators;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GibFrame.Editor
{
    [CustomEditor(typeof(Validator))]
    public abstract class ValidatorCustomEditor : UnityEditor.Editor
    {
        protected IValidable validable;
        private bool showResults = false;
        private List<ValidatorFailure> latestResults = new List<ValidatorFailure>();
        private Vector2 scrollPos;
        private bool resultsFoldout = false;
        private GUIStyle labelStyle;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawProperties();

            EditorGUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Validate"))
            {
                latestResults = validable.Validate();
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
                var content = hasErrors ?
                    EditorGUIUtility.TrTextContentWithIcon($" Validation completed with {latestResults.Count} errors", "winbtn_mac_close@2x") :
                    EditorGUIUtility.TrTextContentWithIcon($" Validation completed with {latestResults.Count} errors", "winbtn_mac_max@2x");

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

        protected abstract void DrawProperties();

        protected virtual void OnEnable()
        {
            validable = target as IValidable;
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