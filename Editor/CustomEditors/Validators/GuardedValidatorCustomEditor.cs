using GibFrame.Editor.Validators;
using UnityEditor;
using UnityEngine;

namespace GibFrame.Editor
{
    [CustomEditor(typeof(GuardedValidator))]
    internal class GuardedValidatorCustomEditor : ValidatorCustomEditor
    {
        private GUIStyle textStyle;

        protected override void DrawProperties()
        {
            EditorGUILayout.LabelField("Assets", textStyle);
            this.PropertyField("validateAssets", "Validate assets");
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Scenes", textStyle);
            var prop = this.PropertyField("allScenes", "Validate all scenes");
            if (!prop.boolValue)
            {
                this.PropertyField("scenes", "Scenes", "Scenes to validate");
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            textStyle = new GUIStyle()
            {
                fontSize = 14
            };
            textStyle.normal.textColor = GUI.contentColor;
        }
    }
}