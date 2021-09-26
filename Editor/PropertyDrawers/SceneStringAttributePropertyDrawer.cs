// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe.Editor -> GibEditor : SceneStringAttributePropertyDrawer.cs
//
// All Rights Reserved

using GibFrame;
using UnityEditor;
using UnityEngine;

namespace GibEditor
{
    [CustomPropertyDrawer(typeof(SceneStringAttribute))]
    public class SceneStringAttributePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                SceneAsset sceneObject = GetSceneAsset(property.stringValue);
                Object scene = EditorGUI.ObjectField(position, label, sceneObject, typeof(SceneAsset), true);
                if (scene == null)
                {
                    property.stringValue = "";
                }
                else if (scene.name != property.stringValue)
                {
                    var sceneObj = GetSceneAsset(scene.name);
                    if (sceneObj != null)
                    {
                        property.stringValue = scene.name;
                    }
                }
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "The SceneString attribute works with strings");
            }
        }

        protected SceneAsset GetSceneAsset(string sceneAssetName)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                return null;
            }

            foreach (var editorScene in EditorBuildSettings.scenes)
            {
                if (editorScene.path.IndexOf(sceneAssetName) != -1)
                {
                    return AssetDatabase.LoadAssetAtPath(editorScene.path, typeof(SceneAsset)) as SceneAsset;
                }
            }
            Debug.LogWarning("Unable to find scene [" + sceneAssetName + "] in build settings");
            return null;
        }
    }
}
