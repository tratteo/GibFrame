//Copyright (c) matteo
//GButton_CE.cs - com.tratteo.gibframe.Editor

using GibFrame.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(GButton))]
public class GButton_CE : Editor
{
    private GButton script;
    private SerializedProperty pointerDownProp, pointerUpProp, longPressedProp;
    private GUIStyle labelStyle;

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(pointerDownProp);
        EditorGUILayout.PropertyField(pointerUpProp);
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Behaviour", labelStyle);
        EditorGUILayout.Space(4);
        script.callbackOnlyOnPointerInside = EditorGUILayout.Toggle("Events only on pointer inside", script.callbackOnlyOnPointerInside);
        script.inheritCallbackEvents = EditorGUILayout.Toggle("Inherit callbacks activation", script.inheritCallbackEvents);
        script.enableLongPress = EditorGUILayout.Toggle("Long press", script.enableLongPress);
        if (script.enableLongPress)
        {
            script.longPressDelay = EditorGUILayout.IntField("Long press delay", script.longPressDelay);
            EditorGUILayout.PropertyField(longPressedProp);
        }
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Graphics", labelStyle);
        EditorGUILayout.Space(4);
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
            if (!Application.isPlaying)
            {
                EditorSceneManager.MarkSceneDirty(script.gameObject.scene);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }

    protected virtual void OnEnable()
    {
        script = (GButton)target;

        pointerDownProp = serializedObject.FindProperty("onPressed");
        pointerUpProp = serializedObject.FindProperty("onReleased");
        longPressedProp = serializedObject.FindProperty("onLongPressed");
        labelStyle = new GUIStyle();
        labelStyle.fontSize = 16;
        labelStyle.normal.textColor = Color.white;
    }
}
