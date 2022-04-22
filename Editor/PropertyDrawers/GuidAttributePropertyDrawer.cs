using System;
using GibFrame.Meta;
using UnityEditor;
using UnityEngine;

namespace GibFrame.Editor
{
    [CustomPropertyDrawer(typeof(GuidAttribute))]
    public class GuidAttributePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GuidAttribute guidAttribute = attribute as GuidAttribute;
            Rect valueRect = new Rect(position.x, position.y, position.width, position.height / 2F);
            Rect generateButtonRect = new Rect(position.x, position.y + valueRect.height, position.width / 2F, position.height / 2F);
            Rect resetButtonRect = new Rect(position.x + generateButtonRect.width, generateButtonRect.y, position.width / 2F, position.height / 2F);

            EditorGUI.BeginProperty(position, label, property);

            if (property.propertyType == SerializedPropertyType.String)
            {
                if (guidAttribute.ReadOnly)
                {
                    GUI.enabled = false;
                    EditorGUI.PropertyField(valueRect, property, label, true);
                    GUI.enabled = true;
                }
                else
                {
                    EditorGUI.PropertyField(valueRect, property, label, true);
                }
                bool hasValue = !string.IsNullOrWhiteSpace(property.stringValue);

                EditorGUI.BeginDisabledGroup(hasValue);
                if (GUI.Button(generateButtonRect, "Generate"))
                {
                    var newGuid = Guid.NewGuid();
                    property.serializedObject.Update();
                    property.stringValue = newGuid.ToString();
                    property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
                }
                EditorGUI.EndDisabledGroup();
                EditorGUI.BeginDisabledGroup(!guidAttribute.AllowReset || !hasValue);
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
                EditorGUI.LabelField(position, label.text, "Use [Guid] with strings.", errorStyle);
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => property.propertyType == SerializedPropertyType.String ? base.GetPropertyHeight(property, label) * 2F : base.GetPropertyHeight(property, label);
    }
}
