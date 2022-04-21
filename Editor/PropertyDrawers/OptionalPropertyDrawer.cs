using UnityEditor;
using UnityEngine;

namespace GibFrame.Editor
{
    [CustomPropertyDrawer(typeof(Optional<>))]
    public class OptionalPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = new GUIContent($"[Optional] {label.text}")
            {
                tooltip = "This reference is marked as optional, it can safely be null"
            };
            EditorGUI.BeginProperty(position, label, property);
            GUI.contentColor = new Color(255 / 255F, 233 / 255F, 179 / 255F);
            var prop = property.FindPropertyRelative("obj");

            EditorGUI.PropertyField(position, prop, label, true);
            EditorGUI.EndProperty();
        }
    }
}
