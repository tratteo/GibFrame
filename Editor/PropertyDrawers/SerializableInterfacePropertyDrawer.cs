using System;
using UnityEditor;
using UnityEngine;

namespace GibFrame.Editor
{
    [CustomPropertyDrawer(typeof(SerializableInterface<>))]
    internal class SerializableInterfacePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var behaviourProp = property.FindPropertyRelative("interfaceBehaviour");
            _ = property.GetFieldInfoAndStaticType(out var type);
            var interfaceType = Type.GetType(type.GetGenericArguments()[0].AssemblyQualifiedName);

            label = new GUIContent($"[{interfaceType.Name}] {label.text}")
            {
                tooltip = $"This reference is marked as interface"
            };
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            GUI.contentColor = new Color(185 / 255F, 255 / 255F, 185 / 255F);
            EditorGUI.PropertyField(position, behaviourProp, label, true);
            GUI.contentColor = Color.white;
            if (EditorGUI.EndChangeCheck())
            {
                if (!TryAssignInterface(behaviourProp.objectReferenceValue, interfaceType, out var assigned))
                {
                    property.serializedObject.Update();
                    UnityEngine.Debug.LogWarning($"Unable to find interface of type {interfaceType.Name} in behaviour {behaviourProp.objectReferenceValue}");
                    behaviourProp.objectReferenceValue = null;
                    property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
                }
                else
                {
                    property.serializedObject.Update();
                    behaviourProp.objectReferenceValue = assigned;
                    property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
                }
            }
            EditorGUI.EndProperty();
        }

        private bool TryAssignInterface(UnityEngine.Object propertyObject, Type interfaceType, out UnityEngine.Object assigned)
        {
            if (propertyObject is GameObject obj)
            {
                if (obj.TryGetComponent(interfaceType, out var component))
                {
                    assigned = component;
                    return true;
                }
            }
            else if (propertyObject is ScriptableObject so)
            {
                if (interfaceType.IsAssignableFrom(so.GetType()))
                {
                    assigned = so;
                    return true;
                }
            }
            assigned = null;
            return false;
        }
    }
}