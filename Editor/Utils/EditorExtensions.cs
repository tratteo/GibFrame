using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GibFrame.Editor
{
    public static class EditorExtensions
    {
        private static GetFieldInfoAndStaticTypeFromProperty getFieldInfoAndStaticTypeFromPropertyDelegate;

        private delegate FieldInfo GetFieldInfoAndStaticTypeFromProperty(SerializedProperty property, out Type type);

        public static FieldInfo GetFieldInfoAndStaticType(this SerializedProperty prop, out Type type)
        {
            if (getFieldInfoAndStaticTypeFromPropertyDelegate == null)
            {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (var t in assembly.GetTypes())
                    {
                        if (t.Name == "ScriptAttributeUtility")
                        {
                            var mi = t.GetMethod("GetFieldInfoAndStaticTypeFromProperty", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                            getFieldInfoAndStaticTypeFromPropertyDelegate = (GetFieldInfoAndStaticTypeFromProperty)Delegate.CreateDelegate(typeof(GetFieldInfoAndStaticTypeFromProperty), mi);
                            break;
                        }
                    }
                    if (getFieldInfoAndStaticTypeFromPropertyDelegate != null) break;
                }
                if (getFieldInfoAndStaticTypeFromPropertyDelegate == null)
                {
                    Debug.LogError("GetFieldInfoAndStaticType::Reflection failed!");
                    type = null;
                    return null;
                }
            }
            return getFieldInfoAndStaticTypeFromPropertyDelegate(prop, out type);
        }

        public static T GetCustomAttributeFromProperty<T>(this SerializedProperty prop) where T : System.Attribute
        {
            var info = prop.GetFieldInfoAndStaticType(out _);
            return info.GetCustomAttribute<T>();
        }

        public static SerializedProperty PropertyField(this UnityEditor.Editor editor, string propName, string label = null, string tooltip = null)
        {
            var prop = editor.serializedObject.FindProperty(propName);
            label ??= propName;
            tooltip ??= string.Empty;
            EditorGUILayout.PropertyField(prop, new GUIContent(label, tooltip));
            return prop;
        }
    }
}