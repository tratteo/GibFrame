using GibFrame.DataStructures;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(RandomizedInt))]
[CustomPropertyDrawer(typeof(RandomizedFloat))]
public class RandomizedVariable_PD : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        Rect minLabelRect = new Rect(position.x, position.y, position.width * 0.075F, position.height);
        Rect minRect = new Rect(minLabelRect.x + minLabelRect.width, position.y, position.width * 0.2F, position.height);
        Rect maxLabelRect = new Rect(minRect.x + minRect.width + 0.075F * position.width, position.y, position.width * 0.075F, position.height);
        Rect maxRect = new Rect(maxLabelRect.x + maxLabelRect.width, position.y, position.width * 0.2F, position.height);
        EditorGUI.LabelField(minLabelRect, "Min");
        EditorGUI.LabelField(maxLabelRect, "Max");
        EditorGUI.PropertyField(minRect, property.FindPropertyRelative("min"), GUIContent.none);
        EditorGUI.PropertyField(maxRect, property.FindPropertyRelative("max"), GUIContent.none);
        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}
