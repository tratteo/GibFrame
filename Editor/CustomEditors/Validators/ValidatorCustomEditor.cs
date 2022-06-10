using GibFrame.Editor.Validators;
using UnityEditor;
using UnityEngine;

namespace GibFrame.Editor
{
    [CustomEditor(typeof(Validator))]
    public abstract class ValidatorCustomEditor : UnityEditor.Editor
    {
        protected IValidable validable;
        private Vector2 scrollPos;
        private bool resultsFoldout;
        private GUIStyle labelStyle;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawProperties();

            var latestResultsProp = serializedObject.FindProperty("latestResultsStrings");
            var showResultsProp = serializedObject.FindProperty("showResults");
            EditorGUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Validate"))
            {
                validable.Validate();
                latestResultsProp = serializedObject.FindProperty("latestResultsStrings");
                showResultsProp = serializedObject.FindProperty("showResults");
            }
            EditorGUI.BeginDisabledGroup(!showResultsProp.boolValue);
            if (GUILayout.Button("Clear"))
            {
                showResultsProp.boolValue = false;
                latestResultsProp.ClearArray();
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
            if (showResultsProp.boolValue)
            {
                EditorGUILayout.Space(10);
                var rect = EditorGUILayout.GetControlRect(false, 2);
                EditorGUI.DrawRect(rect, new Color(0.5F, 0.5F, 0.5F, 1));
                EditorGUILayout.Space(10);
                var dateTimeProp = serializedObject.FindProperty("lastValidationTime");
                var count = latestResultsProp.arraySize;
                var hasErrors = count > 0;
                var content = hasErrors ?
                    EditorGUIUtility.TrTextContentWithIcon($" Validation completed with {count} errors", "winbtn_mac_close@2x") :
                    EditorGUIUtility.TrTextContentWithIcon($" Validation completed with {count} errors", "winbtn_mac_max@2x");

                EditorGUILayout.LabelField(content, labelStyle);
                EditorGUILayout.LabelField(EditorGUIUtility.TrTextContentWithIcon($" {dateTimeProp.stringValue}", "TestStopwatch"), labelStyle);

                EditorGUILayout.Space(10);
                resultsFoldout = EditorGUILayout.Foldout(resultsFoldout, "Results");
                if (resultsFoldout)
                {
                    scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    for (var i = 0; i < count; i++)
                    {
                        EditorGUILayout.LabelField(new GUIContent(latestResultsProp.GetArrayElementAtIndex(i).stringValue + "\n"), labelStyle);
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
                imagePosition = ImagePosition.ImageLeft
            };
            labelStyle.normal.textColor = Color.white;
        }
    }
}