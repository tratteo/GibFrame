// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe.Editor -> GibEditor : GButtonCustomEditor.cs
//
// All Rights Reserved

using GibFrame.UI;
using UnityEditor;
using UnityEngine;

namespace GibFrame.Editor
{
    [CustomEditor(typeof(GButton))]
    internal class GButtonCustomEditor : UnityEditor.Editor
    {
        private GUIStyle labelStyle;
        private bool events;
        private bool behaviour;
        private bool graphics;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            SerializedProperty serProp;

            #region Events

            events = EditorGUILayout.Foldout(events, "Events", true);
            if (events)
            {
                this.PropertyField("onPressed");
                this.PropertyField("onReleased");
                serProp = this.PropertyField("enableLongPress", "Long press");
                if (serProp.boolValue)
                {
                    this.PropertyField("longPressDelay", "Long press delay");
                    this.PropertyField("resetOnFire", "Reset on fire", "If disabled, keep firing the event when the button is pressed");
                    this.PropertyField("onLongPressed");
                }

                EditorGUILayout.Space(10);
            }

            #endregion Events

            #region Behaviour

            behaviour = EditorGUILayout.Foldout(behaviour, "Behaviour", true);
            if (behaviour)
            {
                this.PropertyField("callbackOnlyOnPointerInside", "Events only on pointer inside");
                this.PropertyField("inheritCallbackEvents", "Inherit callbacks activation");

                EditorGUILayout.Space(10);
            }

            #endregion Behaviour

            #region Graphics

            graphics = EditorGUILayout.Foldout(graphics, "Graphics", true);
            if (graphics)
            {
                this.PropertyField("pressedSprite", "Pressed sprite");
                serProp = this.PropertyField("colorPressEffect", "Color effect");
                if (serProp.boolValue)
                {
                    this.PropertyField("pressedColor", "Pressed color");
                }
                serProp = this.PropertyField("pressedSizeEffect", "Size effect");
                if (serProp.boolValue)
                {
                    this.PropertyField("pressedScaleMultiplier", "Pressed scale");
                }
            }

            #endregion Graphics

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void OnEnable()
        {
            labelStyle = new GUIStyle
            {
                fontSize = 16
            };
            labelStyle.normal.textColor = Color.white;
        }
    }
}