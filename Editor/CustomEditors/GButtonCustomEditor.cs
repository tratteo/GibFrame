// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe.Editor -> GibEditor : GButtonCustomEditor.cs
//
// All Rights Reserved

using GibFrame.UI;
using UnityEditor;
using UnityEngine;

namespace GibEditor
{
    [CustomEditor(typeof(GButton))]
    internal class GButtonCustomEditor : Editor
    {
        private GUIStyle labelStyle;

        public override void OnInspectorGUI()
        {
            #region Events

            EditorGUILayout.LabelField("Events", labelStyle);
            EditorGUILayout.Space(5);
            PropertyField("onPressed");
            PropertyField("onReleased");
            var lp = PropertyField("enableLongPress", "Long press");
            if (lp.boolValue)
            {
                PropertyField("longPressDelay", "Long press delay");
                PropertyField("resetOnFire", "Reset on fire", "If disabled, keep firing the event when the button is pressed");
                PropertyField("onLongPressed");
            }

            EditorGUILayout.Space(10);

            #endregion Events

            #region Behaviour

            EditorGUILayout.LabelField("Behaviour", labelStyle);
            EditorGUILayout.Space(5);
            PropertyField("callbackOnlyOnPointerInside", "Events only on pointer inside");
            PropertyField("inheritCallbackEvents", "Inherit callbacks activation");

            EditorGUILayout.Space(10);

            #endregion Behaviour

            #region Graphics

            EditorGUILayout.LabelField("Graphics", labelStyle);
            EditorGUILayout.Space(5);
            PropertyField("pressedSprite", "Pressed sprite");
            lp = PropertyField("colorPressEffect", "Color effect");
            if (lp.boolValue)
            {
                PropertyField("pressedColor", "Pressed color");
            }
            lp = PropertyField("pressedSizeEffect", "Size effect");
            if (lp.boolValue)
            {
                PropertyField("pressedScaleMultiplier", "Pressed scale");
            }

            #endregion Graphics
        }

        protected virtual void OnEnable()
        {
            labelStyle = new GUIStyle
            {
                fontSize = 16
            };
            labelStyle.normal.textColor = Color.white;
        }

        private SerializedProperty PropertyField<T>(string propName, string label = null)
        {
            var prop = serializedObject.FindProperty(propName);
            label ??= propName;
            serializedObject.Update();
            if (typeof(T) == typeof(bool))
            {
                prop.boolValue = EditorGUILayout.Toggle(label, prop.boolValue);
            }
            else if (typeof(T) == typeof(int))
            {
                prop.intValue = EditorGUILayout.IntField(label, prop.intValue);
            }
            else
            {
                EditorGUILayout.PropertyField(prop, new GUIContent(label));
            }
            serializedObject.ApplyModifiedProperties();
            return prop;
        }

        private SerializedProperty PropertyField(string propName, string label = null, string tooltip = null)
        {
            var prop = serializedObject.FindProperty(propName);
            label ??= propName;
            tooltip ??= string.Empty;
            serializedObject.Update();
            EditorGUILayout.PropertyField(prop, new GUIContent(label, tooltip));
            serializedObject.ApplyModifiedProperties();
            return prop;
        }
    }
}
