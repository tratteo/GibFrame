// Copyright (c) 2020 Matteo Beltrame

using GibFrame.UI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ToggleButton))]
public class ToggleButton_CE : GButton_CE
{
    private ToggleButton script;
    private GUIStyle style;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(5);
        EditorGUILayout.LabelField("Toggle", style);
        GUILayout.Space(2);
        script.colorChange = EditorGUILayout.Toggle("Color toggle", script.colorChange);
        if (script.colorChange)
        {
            script.enabledColor = EditorGUILayout.ColorField("Enabled color", script.enabledColor);
            script.disabledColor = EditorGUILayout.ColorField("Disabled color", script.disabledColor);
        }
        script.spriteChange = EditorGUILayout.Toggle("Sprite toggle", script.spriteChange);
        if (script.spriteChange)
        {
            script.enabledSprite = EditorGUILayout.ObjectField(script.enabledSprite, typeof(Sprite), true) as Sprite;
            script.disabledSprite = EditorGUILayout.ObjectField(script.disabledSprite, typeof(Sprite), true) as Sprite;
        }
    }

    protected override void OnEnable()
    {
        script = (ToggleButton)target;
        style = new GUIStyle();
        style.fontSize = 14;
        style.normal.textColor = Color.white;
        base.OnEnable();
    }
}