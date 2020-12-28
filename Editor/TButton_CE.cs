// Copyright (c) 2020 Matteo Beltrame

using GibFrame.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(TButton))]
public class TButton_CE : Editor
{
    private TButton script;
    private SerializedProperty pointerDownProp, pointerUpProp;

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(pointerDownProp);
        EditorGUILayout.PropertyField(pointerUpProp);
        script.colorPressEffect = EditorGUILayout.Toggle("Pressed color effect", script.colorPressEffect);
        if (script.colorPressEffect)
        {
            script.pressedColor = EditorGUILayout.ColorField("Pressed color", script.pressedColor);
        }
        script.resizeOnPress = EditorGUILayout.Toggle("Resize on press", script.resizeOnPress);
        if (script.resizeOnPress)
        {
            script.pressedScaleMultiplier = EditorGUILayout.Vector2Field("Resize multiplier", script.pressedScaleMultiplier);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(script);
            EditorSceneManager.MarkSceneDirty(script.gameObject.scene);
            serializedObject.ApplyModifiedProperties();
        }
    }

    protected virtual void OnEnable()
    {
        script = (TButton)target;

        pointerDownProp = serializedObject.FindProperty("onPressed");
        pointerUpProp = serializedObject.FindProperty("onReleased");
    }
}