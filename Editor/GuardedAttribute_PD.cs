using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(GuardedAttribute))]
public class GuardedAttribute_PD : PropertyDrawer
{
    private readonly StringBuilder logBuilder = new StringBuilder();
    private bool logged = false;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, label);
        if (!logged)
        {
            LogStatus(property);
            logged = true;
        }

        if (GUI.changed)
        {
            logged = false;
        }
    }

    private void LogStatus(SerializedProperty property)
    {
        var targetObject = property.serializedObject.targetObject;
        var targetObjectClassType = targetObject.GetType();
        var field = targetObjectClassType.GetField(property.propertyPath, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
        if (field.GetValue(targetObject) == default)
        {
            GuardedAttribute att = attribute as GuardedAttribute;
            switch (att.Gravity)
            {
                case GuardedAttribute.MissingValueGravity.INFO:
                    logBuilder.AppendFormat("Field <b>{0}</b> of class <b>{1}</b> has not been assigned.", field, targetObjectClassType).Append("\n").Append(att.Message);
                    Debug.Log(logBuilder.ToString());
                    break;

                case GuardedAttribute.MissingValueGravity.WARNING:
                    logBuilder.AppendFormat("Field <color=yellow><b>{0}</b></color> of class <color=yellow><b>{1}</b></color> has not been assigned.", field, targetObjectClassType).Append("\n").Append(att.Message);
                    Debug.LogWarning(logBuilder.ToString());
                    break;

                case GuardedAttribute.MissingValueGravity.ERROR:
                    logBuilder.AppendFormat("Field <color=red><b>{0}</b></color> of class <color=red><b>{1}</b></color> has not been assigned.", field, targetObjectClassType).Append("\n").Append(att.Message);
                    Debug.LogError(logBuilder.ToString());
                    break;
            }
        }
        logBuilder.Clear();
    }
}
