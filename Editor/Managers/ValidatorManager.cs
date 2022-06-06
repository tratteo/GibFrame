using GibFrame.Validators;
using System.Text;
using UnityEngine;

namespace GibFrame.Editor
{
    public static class ValidatorManager
    {
        /// <summary>
        ///   Validate all groups found in the Asset folder and sub-folders
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
            return failure;
        }

        public static bool ValidateGuarded()
        {
            var res = GibEditor.GetAllBehavioursInAsset<EditorGuardedValidator>($"Editor");
            if (res.Count > 0)
            {
                Debug.Log("User editor guard override");
            }
            else
            {
                res = GibEditor.GetAllBehavioursAtPath<EditorGuardedValidator>(Resources.PackageEditorPath);
                if (res.Count <= 0)
                {
                    Debug.LogWarning($"Unable to find {nameof(EditorGuardedValidator)}");
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
            return failure;
        }
    }
}