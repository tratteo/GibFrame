using System;
using System.Reflection;
using System.Text;
using GibFrame.Extensions;
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
        StringBuilder logBuilder = new StringBuilder();
        MonoBehaviour[] objs = Resources.FindObjectsOfTypeAll<MonoBehaviour>();
        objs.ForEach(o =>
        {
            bool persistent = EditorUtility.IsPersistent(o);
            if (scene && prefabs || (persistent && prefabs) || (!persistent && scene))
            {
                LogRecursively(o, o, logBuilder);
            }
        });
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
                logBuilder.AppendFormat("Field <b>{0}</b> of class <b>{1}</b> sitting on GameObject <b>{2}</b> is set to default value: <b>{3}</b>", field, parentClass, obj.name, fieldVal).Append("\n").Append(guarded.Message);
                Debug.Log(logBuilder.ToString());
                break;

            case GuardedAttribute.MissingValueGravity.WARNING:
                logBuilder.AppendFormat("Field <color=yellow><b>{0}</b></color> of class <color=yellow><b>{1}</b></color> sitting on GameObject <b>{2}</b> is set to default value: <color=yellow><b>{3}</b></color>", field, parentClass, obj.name, fieldVal).Append("\n").Append(guarded.Message);
                Debug.LogWarning(logBuilder.ToString());
                break;

            case GuardedAttribute.MissingValueGravity.ERROR:
                logBuilder.AppendFormat("Field <color=red><b>{0}</b></color> of class <color=red><b>{1}</b></color> sitting on GameObject <b>{2}</b> is set to default value: <color=red><b>{3}</b></color>", field, parentClass, obj.name, fieldVal).Append("\n").Append(guarded.Message);
                Debug.LogError(logBuilder.ToString());
                break;
        }
    }

    private static void LogRecursively(object obj, UnityEngine.Object parentObj, StringBuilder logBuilder)
    {
        FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        fields.ForEach(field =>
        {
            GuardedAttribute attribute = Attribute.GetCustomAttribute(field, typeof(GuardedAttribute)) as GuardedAttribute;
            if (attribute != null)
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
                    LogRecursively(val, parentObj, logBuilder);
                }
            }
        });
    }

    private static bool IsNullOrDefault<T>(T arg)
    {
        if (arg is UnityEngine.Object && !(arg as UnityEngine.Object)) return true;
        if (arg == null) return true;
        if (object.Equals(arg, default(T))) return true;
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
