// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe.Editor -> GibEditor : ToggleButtonCustomEditor.cs
//
// All Rights Reserved

using GibFrame.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace GibEditor
{
    [CustomEditor(typeof(ToggleButton))]
    internal class ToggleButtonCustomEditor : GButtonCustomEditor
    {
        private ToggleButton script;
        private GUIStyle style;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(5);
            EditorGUILayout.LabelField("Toggle", style);
            GUILayout.Space(2);
            script.colorChange = EditorGUILayout.Toggle("Color toggle", script.colorChange);
            if (script.colorChange)
            {
                script.enabledColor = EditorGUILayout.ColorField("Enabled color", script.enabledColor);
                script.disabledColor = EditorGUILayout.ColorField("Disabled color", script.disabledColor);
            }
            script.spriteChange = EditorGUILayout.Toggle("Sprite toggle", script.spriteChange);
            if (script.spriteChange)
            {
                EditorGUILayout.LabelField("Enabled");
                script.enabledSprite = EditorGUILayout.ObjectField(script.enabledSprite, typeof(Sprite), true) as Sprite;
                EditorGUILayout.LabelField("Disabled");
                script.disabledSprite = EditorGUILayout.ObjectField(script.disabledSprite, typeof(Sprite), true) as Sprite;
            }
            if (GUI.changed)
            {
                EditorUtility.SetDirty(script);
                EditorSceneManager.MarkSceneDirty(script.gameObject.scene);
                serializedObject.ApplyModifiedProperties();
            }
        }

        protected override void OnEnable()
        {
            script = (ToggleButton)target;
            style = new GUIStyle();
            style.fontSize = 14;
            style.normal.textColor = Color.white;
            base.OnEnable();
        }
    }
}
