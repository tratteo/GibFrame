﻿// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe.Editor -> GibEditor : GuardedManager.cs
//
// All Rights Reserved

using GibFrame.Meta;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace GibEditor
{
    internal class GuardedManager
    {
        internal static void VerifyGuardedObjects(bool scene, bool prefabs)
        {
            var objs = new List<UnityEngine.Object>();
            if (scene)
            {
                objs.AddRange(GetAllBehavioursInScene());
            }
            if (prefabs)
            {
                objs.AddRange(GetAllBehavioursInAssets());
            }
            var logBuilder = new StringBuilder();
            for (var i = 0; i < objs.Count; i++)
            {
                var obj = objs[i];
                EditorUtility.DisplayProgressBar("Guarding members", $"Checking {obj.name}", (float)i / objs.Count);
                GuardRecursively(obj, obj, logBuilder);
            }
            EditorUtility.ClearProgressBar();
            Debug.Log($"Guarding process completed, analyzed {objs.Count} elements");
        }

        internal static List<UnityEngine.Object> GetAllBehavioursInAssets(string path = "")
        {
            var sel = new List<UnityEngine.Object>();
            var fileEntries = Directory.GetFiles(Application.dataPath + "/" + path);
            foreach (var fileName in fileEntries)
            {
                var index = fileName.LastIndexOf("/");
                var localPath = "Assets";

                if (index > 0) localPath += fileName[index..];

                var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(localPath);
                if (!asset) continue;
                if (asset is GameObject gameObject)
                {
                    var monos = gameObject.GetComponentsInChildren<MonoBehaviour>();
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
            var dirs = Directory.GetDirectories(Application.dataPath + "/" + path);
            foreach (var dir in dirs)
            {
                var relativePath = path;
                var index = dir.LastIndexOfAny(new char[] { '/', '\\' });
                if (index > 0)
                {
                    var val = dir[index..];
                    relativePath += val;
                    sel.AddRange(GetAllBehavioursInAssets(relativePath));
                }
            }
            return sel;
        }

        private static List<UnityEngine.Object> GetAllBehavioursInScene()
        {
            var objs = UnityEngine.Object.FindObjectsOfType<UnityEngine.Object>();
            var sel = new List<UnityEngine.Object>();
            for (var i = 0; i < objs.Length; i++)
            {
                var current = objs[i];

                if (!current) continue;

                if (current is GameObject obj)
                {
                    var monos = obj.GetComponents<MonoBehaviour>();
                    sel.AddRange(monos);
                }
                else if (current is ScriptableObject so)
                {
                    sel.Add(so);
                }
            }

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
            var path = AssetDatabase.GetAssetPath(obj);
            switch (guarded.Gravity)
            {
                case GuardedAttribute.MissingValueGravity.INFO:
                    logBuilder.AppendFormat("Field <b>{0}</b> of class <b>{1}</b> sitting on GameObject <b>{2}:{3}</b> is set to default value <b>{4}</b>", field, parentClass, path, obj.name, fieldVal).Append("\n").Append(guarded.Message);
                    Debug.Log(logBuilder.ToString());
                    break;

                case GuardedAttribute.MissingValueGravity.WARNING:

                    logBuilder.AppendFormat("Field <color=yellow><b>{0}</b></color> of class <color=yellow><b>{1}</b></color> sitting on GameObject <b>{2}:{3}</b> is set to default value <color=yellow><b>{4}</b></color>", field, parentClass, path, obj.name, fieldVal).Append("\n").Append(guarded.Message);
                    Debug.LogWarning(logBuilder.ToString());
                    break;

                case GuardedAttribute.MissingValueGravity.ERROR:
                    logBuilder.AppendFormat("Field <color=red><b>{0}</b></color> of class <color=red><b>{1}</b></color> sitting on GameObject <b>{2}:{3}</b> is set to default value <color=red><b>{4}</b></color>", field, parentClass, path, obj.name, fieldVal).Append("\n").Append(guarded.Message);
                    Debug.LogError(logBuilder.ToString());
                    break;
            }
        }

        private static void GuardRecursively(object obj, UnityEngine.Object parentObj, StringBuilder logBuilder)
        {
            if (obj is null) return;

            var fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (var i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                if (Attribute.GetCustomAttribute(field, typeof(GuardedAttribute)) is GuardedAttribute attribute)
                {
                    var value = field.GetValue(obj);
                    if (IsNullOrDefault(value))
                    {
                        Print(attribute, field, value, obj.GetType(), parentObj, logBuilder);
                    }
                }
            }
        }

        private static bool IsNullOrDefault<T>(T arg)
        {
            if (arg is UnityEngine.Object && !(arg as UnityEngine.Object)) return true;

            if (arg is null) return true;

            if (Equals(arg, default(T))) return true;

            var methodType = typeof(T);
            if (Nullable.GetUnderlyingType(methodType) is not null) return false;

            var argumentType = arg.GetType();
            if (argumentType.IsValueType && argumentType != methodType)
            {
                var obj = Activator.CreateInstance(arg.GetType());
                return obj.Equals(arg);
            }
            return false;
        }
    }
}