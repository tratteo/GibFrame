﻿// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe.Editor -> GibEditor : RandomizedVariablePropertyDrawer.cs
//
// All Rights Reserved

using GibFrame.Data;
using UnityEditor;
using UnityEngine;

namespace GibFrame.Editor
{
    [CustomPropertyDrawer(typeof(RandomizedInt))]
    [CustomPropertyDrawer(typeof(RandomizedFloat))]
    internal class RandomizedVariablePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            var minLabelRect = new Rect(position.x, position.y, 32F, position.height);
            var minRect = new Rect(minLabelRect.x + minLabelRect.width, position.y, position.width * 0.25F, position.height);
            var maxLabelRect = new Rect(minRect.x + minRect.width + 6F, position.y, 32F, position.height);
            var maxRect = new Rect(maxLabelRect.x + maxLabelRect.width, position.y, position.width * 0.25F, position.height);
            EditorGUI.LabelField(minLabelRect, "Min");
            EditorGUI.LabelField(maxLabelRect, "Max");
            EditorGUI.PropertyField(minRect, property.FindPropertyRelative("min"), GUIContent.none);
            EditorGUI.PropertyField(maxRect, property.FindPropertyRelative("max"), GUIContent.none);
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}