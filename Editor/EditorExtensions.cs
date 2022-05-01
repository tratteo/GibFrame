using UnityEditor;
using UnityEngine;

namespace GibEditor
{
    public static class EditorExtensions
    {
        public static SerializedProperty PropertyField(this Editor editor, string propName, string label = null, string tooltip = null)
        {
            var prop = editor.serializedObject.FindProperty(propName);
            label ??= propName;
            tooltip ??= string.Empty;
            EditorGUILayout.PropertyField(prop, new GUIContent(label, tooltip));
            return prop;
        }
    }
}
