using GibFrame.Meta;
using GibFrame.Validators;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace GibFrame.Editor.Validators
{
    [CreateAssetMenu(menuName = "GibFrame/Validators/GuardedValidator", fileName = "guarded_validator")]
    public class EditorGuardedValidator : Validator
    {
        public enum Policy
        { Assets, Scene, Both }

        [SerializeField] private Policy guardPolicy = Policy.Both;

        public Policy GuardPolicy => guardPolicy;

        public override List<ValidatorFailure> Validate()
        {
            var objs = new List<UnityEngine.Object>();
            if (guardPolicy is Policy.Scene or Policy.Both)
            {
                objs.AddRange(GibEditor.GetBehaviours(GibEditor.GetObjectsInScene()));
            }
            if (guardPolicy is Policy.Assets or Policy.Both)
            {
                objs.AddRange(GibEditor.GetBehaviours(GibEditor.GetObjectsInAssets()));
            }
            var failures = new List<ValidatorFailure>();
            for (var i = 0; i < objs.Count; i++)
            {
                var obj = objs[i];
                failures.AddRange(GuardRecursively(obj, obj));
            }
            return failures;
        }

        private ValidatorFailure Analyze(GuardedAttribute guarded, FieldInfo field, object fieldVal, Type parentClass, UnityEngine.Object obj)
        {
            var logBuilder = new StringBuilder();
            if (EditorUtility.IsPersistent(obj))
            {
                logBuilder.Append("<color=cyan><b>Prefab</b></color> => ");
            }
            else
            {
                logBuilder.Append("<color=magenta><b>Scene Object</b></color> => ");
            }
            var path = AssetDatabase.GetAssetPath(obj);

            switch (guarded.Gravity)
            {
                case GuardedAttribute.MissingValueGravity.Info:
                    logBuilder.AppendFormat("<b>{0}</b> | Field <b>{1}</b> of class <b>{2}</b> on Object <b>{3}/{4}</b> is set to default value: <b>{5}</b>", guarded.Message, field.Name, parentClass, path, obj.name, fieldVal);
                    break;

                case GuardedAttribute.MissingValueGravity.Warning:
                    logBuilder.AppendFormat("<color=yellow><b>{0}</b></color> | Field <b>{1}</b> of class <b>{2}</b> on Object <b>{3}/{4}</b> is set to default value: <color=yellow><b>{5}</b></color>", guarded.Message, field.Name, parentClass, path, obj.name, fieldVal);
                    break;

                case GuardedAttribute.MissingValueGravity.Error:
                    logBuilder.AppendFormat("<color=red><b>{0}</b></color> | Field <b>{1}</b> of class <b>{2}</b> on Object <b>{3}/{4}</b> is set to default value: <color=red><b>{5}</b></color>", guarded.Message, field.Name, parentClass, path, obj.name, fieldVal);
                    break;
            }
            return ValidatorFailure.Of(this).Reason(logBuilder.ToString()).By(obj, field.Name, fieldVal);
        }

        private List<ValidatorFailure> GuardRecursively(object obj, UnityEngine.Object parentObj)
        {
            var failures = new List<ValidatorFailure>();
            if (obj is null) return failures;
            var fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (var i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                if (Attribute.GetCustomAttribute(field, typeof(GuardedAttribute)) is GuardedAttribute attribute)
                {
                    var value = field.GetValue(obj);
                    if (GibEditor.IsNullOrDefault(value))
                    {
                        failures.Add(Analyze(attribute, field, value, obj.GetType(), parentObj));
                    }
                }
            }
            return failures;
        }
    }
}