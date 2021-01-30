// Copyright (c) 2020 Matteo Beltrame

using GibFrame.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(GButton))]
public class GButton_CE : Editor
{
    private GButton script;
    private SerializedProperty pointerDownProp, pointerUpProp;

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(pointerDownProp);
        EditorGUILayout.PropertyField(pointerUpProp);
        script.pressOnlyOnPointerInside = EditorGUILayout.Toggle("Pressed only on pointer inside", script.pressOnlyOnPointerInside);
        EditorGUILayout.LabelField("Pressed sprite");
        script.pressedSprite = EditorGUILayout.ObjectField(script.pressedSprite, typeof(Sprite), true) as Sprite;
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
        script = (GButton)target;

        pointerDownProp = serializedObject.FindProperty("onPressed");
        pointerUpProp = serializedObject.FindProperty("onReleased");
    }
}