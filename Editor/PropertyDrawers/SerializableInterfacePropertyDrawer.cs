using System;
using UnityEditor;
using UnityEngine;

namespace GibFrame.Editor
{
    [CustomPropertyDrawer(typeof(SerializableInterface<>))]
    public class SerializableInterfacePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var behaviourProp = property.FindPropertyRelative("interfaceBehaviour");
            var interfaceProp = property.FindPropertyRelative("interfaceType");
            label = new GUIContent($"[{Type.GetType(interfaceProp.stringValue).Name}] {label.text}")
            {
                tooltip = $"This reference is marked as interface"
            };
            EditorGUI.BeginProperty(position, label, property);
            GUI.contentColor = new Color(185 / 255F, 255 / 255F, 185 / 255F);
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, behaviourProp, label, true);
            if (EditorGUI.EndChangeCheck())
            {
                if (!Type.GetType(interfaceProp.stringValue).IsAssignableFrom(behaviourProp.objectReferenceValue.GetType()))
                {
                    UnityEngine.Debug.LogWarning($"Unable to find interface of type {Type.GetType(interfaceProp.stringValue).Name} in behaviour {behaviourProp.objectReferenceValue}");
                    property.serializedObject.Update();
                    behaviourProp.objectReferenceValue = null;
                    property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
                }
            }
            GUI.contentColor = Color.white;
            EditorGUI.EndProperty();
        }
    }
}
