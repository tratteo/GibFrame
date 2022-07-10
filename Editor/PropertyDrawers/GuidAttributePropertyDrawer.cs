using GibFrame.Meta;
using System;
using UnityEditor;
using UnityEngine;

namespace GibFrame.Editor
{
    [CustomPropertyDrawer(typeof(GuidAttribute))]
    internal class GuidAttributePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var guidAttribute = attribute as GuidAttribute;
            var valueRect = new Rect(position.x, position.y, position.width, position.height / 2F);
            var generateButtonRect = new Rect(position.x, position.y + valueRect.height, position.width / 2F, position.height / 2F);
            var resetButtonRect = new Rect(position.x + generateButtonRect.width, generateButtonRect.y, position.width / 2F, position.height / 2F);

            EditorGUI.BeginProperty(position, label, property);

            if (property.propertyType == SerializedPropertyType.String)
            {
                if (guidAttribute.Readonly)
                {
                    var contRect = EditorGUI.PrefixLabel(valueRect, label);
                    EditorGUI.SelectableLabel(contRect, property.stringValue);
                }
                else
                {
                    EditorGUI.PropertyField(valueRect, property, label, true);
                }
                var hasValue = !string.IsNullOrWhiteSpace(property.stringValue);

                EditorGUI.BeginDisabledGroup(hasValue);
                if (GUI.Button(generateButtonRect, "Generate"))
                {
                    var newGuid = Guid.NewGuid();
                    property.serializedObject.Update();
                    property.stringValue = newGuid.ToString();
                    property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
                }
                EditorGUI.EndDisabledGroup();
                EditorGUI.BeginDisabledGroup(!guidAttribute.Resettable || !hasValue);
                if (GUI.Button(resetButtonRect, "Reset"))
                {
                    if (EditorUtility.DisplayDialog("Guid reset", "Are you sure you want to reset the Guid?\n" +
                        "This will clear the field. Every dependency that referenced this object by this Guid will lose the reference.", "Reset", "Cancel"))
                    {
                        property.serializedObject.Update();
                        property.stringValue = null;
                        property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
                    }
                    GUIUtility.ExitGUI();
                }
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                var errorStyle = new GUIStyle();
                errorStyle.normal.textColor = Color.red;
                EditorGUI.LabelField(position, label.text, $"Use [{nameof(GuidAttribute)}] with strings.", errorStyle);
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => property.propertyType == SerializedPropertyType.String ? base.GetPropertyHeight(property, label) * 2F : base.GetPropertyHeight(property, label);
    }
}