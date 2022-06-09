using GibFrame.Editor.Validators;
using System.Text;
using UnityEngine;

namespace GibFrame.Editor
{
    public static class ValidatorManager
    {
        /// <summary>
        ///   Validate all <see cref="ValidatorGroup"/> in the <i> Asset </i> folder and sub-folders
        /// </summary>
        public static bool ValidateAllGroups()
        {
            var objs = GibEditor.GetAllBehavioursInAsset<ValidatorGroup>();
            var failure = false;
            foreach (var o in objs)
            {
                var res = o.Validate();
                if (res.Count <= 0)
                {
                    Debug.Log($"{o.name} -> <color=#55d971>Validation successful! <b>:D</b></color>", o);
                }
                else
                {
                    failure = true;
                    var builder = new StringBuilder($"{o.name} -> <color=#ed4e4e>Validation failed with {res.Count} errors</color>\nClick for more info\n");
                    foreach (var r in res)
                    {
                        builder.Append(r.ToString() + "\n");
                    }
                    Debug.LogError(builder.ToString(), o);
                }
            }
            return !failure;
        }

        /// <summary>
        ///   Validate all the fields marked with the <see cref="Meta.GuardedAttribute"/> using the default <see cref="GuardedValidator"/>
        ///   embedded in GibFrame. In order to override the default validator, create a new <see cref="GuardedValidator"/> in the <i>
        ///   Asset/Editor </i> folder of the project.
        /// </summary>
        public static bool ValidateGuarded()
        {
            var res = GibEditor.GetAllBehavioursInAsset<GuardedValidator>($"Editor");
            if (res.Count > 0)
            {
                Debug.Log($"Using override of {nameof(GuardedValidator)}");
            }
            else
            {
                res = GibEditor.GetAllBehavioursAtPath<GuardedValidator>(Resources.PackageEditorPath);
                if (res.Count <= 0)
                {
                    Debug.LogWarning($"Unable to find {nameof(GuardedValidator)}. Try creating a new one in the Editor folder of the project");
                    return false;
                }
            }
            var failure = false;
            foreach (var o in res)
            {
                var validatorName = o.name;
                var failures = o.Validate();
                if (failures.Count <= 0)
                {
                    Debug.Log($"{validatorName} -> <color=#55d971>Validation successful! <b>:D</b></color>");
                }
                else
                {
                    failure = true;
                    var builder = new StringBuilder($"{validatorName} -> <color=#ed4e4e>Validation failed with {failures.Count} errors</color>\nClick for more info\n");
                    foreach (var r in failures)
                    {
                        builder.Append(r.ToString() + "\n");
                    }
                    Debug.LogError(builder.ToString());
                }
            }
            return !failure;
        }

        /// <summary>
        ///   Validate everything that is validable <b> :D </b>
        /// </summary>
        /// <returns> </returns>
        public static bool Validate() => ValidateAllGroups() | ValidateGuarded();
    }
}