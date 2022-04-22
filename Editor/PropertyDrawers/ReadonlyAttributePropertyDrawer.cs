﻿// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe.Editor -> GibEditor : ReadonlyAttributePropertyDrawer.cs
//
// All Rights Reserved

using GibFrame;
using GibFrame.Meta;
using UnityEditor;
using UnityEngine;

namespace GibEditor
{
    [CustomPropertyDrawer(typeof(ReadonlyAttribute))]
    public class ReadonlyAttributePropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}
