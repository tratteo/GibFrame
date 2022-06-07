using UnityEditor;
using UnityEngine;

namespace GibFrame.Editor
{
    [CustomPropertyDrawer(typeof(Optional<>))]
    internal class OptionalPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = new GUIContent($"[Optional] {label.text}")
            {
                tooltip = "This reference is marked as optional, it can safely be null"
            };
            EditorGUI.BeginProperty(position, label, property);
            var prop = property.FindPropertyRelative("obj");
            GUI.contentColor = new Color(255 / 255F, 233 / 255F, 179 / 255F);
            EditorGUI.PropertyField(position, prop, label, true);
            GUI.contentColor = Color.white;
            EditorGUI.EndProperty();
        }
    }
}