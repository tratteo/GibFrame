using UnityEditor;
using UnityEngine;

namespace GibFrame.Editor
{
    public static class EditorExtensions
    {
        public static SerializedProperty PropertyField(this UnityEditor.Editor editor, string propName, string label = null, string tooltip = null)
        {
            var prop = editor.serializedObject.FindProperty(propName);
            label ??= propName;
            tooltip ??= string.Empty;
            EditorGUILayout.PropertyField(prop, new GUIContent(label, tooltip));
            return prop;
        }
    }
}