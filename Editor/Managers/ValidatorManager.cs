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
        public static void ValidateAllGroupsInAssets()
        {
            var objs = GibEditor.GetAllBehavioursInAsset<ValidationGroup>();

            foreach (var o in objs)
            {
                var res = o.ValidateAll();
                if (res.Count <= 0)
                {
                    Debug.Log($"{o.name} -> Validation successful! :D", o);
                }
                else
                {
                    var builder = new StringBuilder($"{o.name} -> Validation failed with {res.Count} errors X(\nClick me for more info\n");
                    foreach (var r in res)
                    {
                        builder.Append(r.ToString() + "\n");
                    }
                    Debug.LogError(builder.ToString(), o);
                }
            }
        }

        public static void ValidateGuarded()
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
                    return;
                }
            }
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
                    var builder = new StringBuilder($"{o.name} -> Validation failed with {failures.Count} errors X(\nClick me for more info\n");
                    foreach (var r in failures)
                    {
                        builder.Append(r.ToString() + "\n");
                    }
                    Debug.LogError(builder.ToString(), o);
                }
            }
        }
    }
}