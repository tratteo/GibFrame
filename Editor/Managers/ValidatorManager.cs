using GibFrame.Editor.Validators;
using System.Text;
using UnityEngine;

namespace GibFrame.Editor
{
    public static class ValidatorManager
    {
        /// <summary>
        ///   Validate all <see cref="ValidationGroup"/> found in the <i> Asset </i> folder and sub-folders
        /// </summary>
        public static bool ValidateAllGroupsInAssets()
        {
            var objs = GibEditor.GetAllBehavioursInAsset<ValidationGroup>();

            var failure = false;
            foreach (var o in objs)
            {
                var res = o.ValidateAll();
                if (res.Count <= 0)
                {
                    Debug.Log($"{o.name} -> Validation successful! :D", o);
                }
                else
                {
                    failure = true;
                    var builder = new StringBuilder($"{o.name} -> Validation failed with {res.Count} errors X(\nClick me for more info\n");
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
        ///   Asset/Editor </i> folder.
        /// </summary>
        public static bool ValidateGuarded()
        {
            var res = GibEditor.GetAllBehavioursInAsset<GuardedValidator>($"Editor");
            if (res.Count > 0)
            {
                Debug.Log("User editor guard override");
            }
            else
            {
                res = GibEditor.GetAllBehavioursAtPath<GuardedValidator>(Resources.PackageEditorPath);
                if (res.Count <= 0)
                {
                    Debug.LogWarning($"Unable to find {nameof(GuardedValidator)}");
                    return false;
                }
            }
            var failure = false;
            foreach (var o in res)
            {
                Debug.Log(o.name + " | Verifying with policy: " + o.GuardPolicy);
                var failures = o.Validate();
                if (failures.Count <= 0)
                {
                    Debug.Log($"{o.name} -> Validation successful! :D", o);
                }
                else
                {
                    failure = true;
                    var builder = new StringBuilder($"{o.name} -> Validation failed with {failures.Count} errors X(\nClick me for more info\n");
                    foreach (var r in failures)
                    {
                        builder.Append(r.ToString() + "\n");
                    }
                    Debug.LogError(builder.ToString(), o);
                }
            }
            return !failure;
        }
    }
}