//Copyright (c) matteo
//GuardedExecutable.cs - com.tratteo.gibframe.Editor

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using GibFrame;
using UnityEditor;
using UnityEngine;

public class GuardedExecutable
{
    [MenuItem("GibFrame/Guarded/Verify")]
    public static void VerifyGuardedObjects()
    {
        VerifyGuardedObjects(true, true);
    }

    [MenuItem("GibFrame/Guarded/Verify Scene")]
    public static void VerifySceneGuardedObjects()
    {
        VerifyGuardedObjects(true, false);
    }

    [MenuItem("GibFrame/Guarded/Verify Prefabs")]
    public static void VerifyPrefabsGuardedObjects()
    {
        VerifyGuardedObjects(false, true);
    }

    public static void VerifyGuardedObjects(bool scene, bool prefabs)
    {
        List<UnityEngine.Object> objs = new List<UnityEngine.Object>();
        if (scene)
        {
            objs.AddRange(GetAllBehavioursInScene());
        }
        if (prefabs)
        {
            objs.AddRange(GetAllBehavioursInAssets());
        }
        StringBuilder logBuilder = new StringBuilder();
        objs.ForEach(obj =>
        {
            GuardRecursively(obj, obj, logBuilder);
        });
    }

    public static List<UnityEngine.Object> GetAllBehavioursInAssets(string path = "")
    {
        List<UnityEngine.Object> sel = new List<UnityEngine.Object>();
        string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + path);
        foreach (string fileName in fileEntries)
        {
            int index = fileName.LastIndexOf("/");
            string localPath = "Assets";

            if (index > 0)
            {
                localPath += fileName.Substring(index);
            }

            UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(localPath);
            if (asset)
            {
                if (asset is GameObject gameObject)
                {
                    MonoBehaviour[] monos = gameObject.GetComponentsInChildren<MonoBehaviour>();
                    if (monos != null && monos.Length > 0)
                    {
                        sel.AddRange(monos);
                    }
                }
                else if (asset is ScriptableObject so)
                {
                    if (so)
                    {
                        sel.Add(so);
                    }
                }
            }
        }
        string[] dirs = Directory.GetDirectories(Application.dataPath + "/" + path);
        foreach (string dir in dirs)
        {
            string relativePath = path;
            int index = dir.LastIndexOfAny(new char[] { '/', '\\' });
            if (index > 0)
            {
                string val = dir.Substring(index);
                relativePath += val;
                sel.AddRange(GetAllBehavioursInAssets(relativePath));
            }
        }
        return sel;
    }

    private static List<UnityEngine.Object> GetAllBehavioursInScene()
    {
        UnityEngine.Object[] objs = UnityEngine.Object.FindObjectsOfType<UnityEngine.Object>();
        List<UnityEngine.Object> sel = new List<UnityEngine.Object>();
        objs.ForEach(o =>
        {
            if (o)
            {
                if (o is GameObject obj)
                {
                    MonoBehaviour[] monos = obj.GetComponentsInChildren<MonoBehaviour>();
                    sel.AddRange(monos);
                }
                else if (o is ScriptableObject so)
                {
                    sel.Add(so);
                }
            }
        });
        return sel;
    }

    private static void Print(GuardedAttribute guarded, FieldInfo field, object fieldVal, Type parentClass, UnityEngine.Object obj, StringBuilder logBuilder)
    {
        logBuilder.Clear();
        if (EditorUtility.IsPersistent(obj))
        {
            logBuilder.Append("<color=cyan><b>Prefab</b></color> =>");
        }
        else
        {
            logBuilder.Append("<color=magenta><b>Scene Object</b></color> =>");
        }
        switch (guarded.Gravity)
        {
            case GuardedAttribute.MissingValueGravity.INFO:
                logBuilder.AppendFormat("Field <b>{0}</b> of class <b>{1}</b> sitting on GameObject <b>{2}</b> is set to default value <b>{3}</b>", field, parentClass, obj.name, fieldVal).Append("\n").Append(guarded.Message);
                Debug.Log(logBuilder.ToString());
                break;

            case GuardedAttribute.MissingValueGravity.WARNING:
                logBuilder.AppendFormat("Field <color=yellow><b>{0}</b></color> of class <color=yellow><b>{1}</b></color> sitting on GameObject <b>{2}</b> is set to default value <color=yellow><b>{3}</b></color>", field, parentClass, obj.name, fieldVal).Append("\n").Append(guarded.Message);
                Debug.LogWarning(logBuilder.ToString());
                break;

            case GuardedAttribute.MissingValueGravity.ERROR:
                logBuilder.AppendFormat("Field <color=red><b>{0}</b></color> of class <color=red><b>{1}</b></color> sitting on GameObject <b>{2}</b> is set to default value <color=red><b>{3}</b></color>", field, parentClass, obj.name, fieldVal).Append("\n").Append(guarded.Message);
                Debug.LogError(logBuilder.ToString());
                break;
        }
    }

    private static void GuardRecursively(object obj, UnityEngine.Object parentObj, StringBuilder logBuilder)
    {
        if (obj == null) return;
        FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        fields.ForEach(field =>
        {
            if (Attribute.GetCustomAttribute(field, typeof(GuardedAttribute)) is GuardedAttribute attribute)
            {
                var value = field.GetValue(obj);
                if (IsNullOrDefault(value))
                {
                    Print(attribute, field, value, obj.GetType(), parentObj, logBuilder);
                }
            }
            if (field.FieldType.IsClass)
            {
                object val = field.GetValue(obj);
                if (val != null)
                {
                    GuardRecursively(val, parentObj, logBuilder);
                }
            }
        });
    }

    private static bool IsNullOrDefault<T>(T arg)
    {
        if (arg is UnityEngine.Object && !(arg as UnityEngine.Object)) return true;
        if (arg == null) return true;
        if (Equals(arg, default(T))) return true;
        Type methodType = typeof(T);
        if (Nullable.GetUnderlyingType(methodType) != null) return false;
        Type argumentType = arg.GetType();
        if (argumentType.IsValueType && argumentType != methodType)
        {
            object obj = Activator.CreateInstance(arg.GetType());
            return obj.Equals(arg);
        }
        return false;
    }
}
