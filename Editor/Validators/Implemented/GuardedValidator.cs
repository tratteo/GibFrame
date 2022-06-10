using GibFrame.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace GibFrame.Editor.Validators
{
    [CreateAssetMenu(menuName = "GibFrame/Validators/GuardedValidator", fileName = "guarded_validator")]
    public class GuardedValidator : Validator
    {
        [SerializeField] private bool validateAssets = true;
        [SerializeField] private bool allScenes = false;
        [SerializeField] private SceneReference[] scenes;

        public override void Validate(List<ValidatorFailure> failures, Action<IValidable.Progress> progress = null)
        {
            var assets = new List<UnityEngine.Object>();
            var progressVal = new IValidable.Progress(nameof(GuardedValidator), "Retrieving assets...", 0);
            progress?.Invoke(progressVal);
            if (validateAssets) assets.AddRange(Gib.GetAllBehaviours<MonoBehaviour>(GibEditor.GetUnityObjectsInAssets().ToArray()));
            progressVal.description = "Retrieving scenes...";
            progress?.Invoke(progressVal);
            var scenesPaths = new List<string>();
            if (allScenes)
            {
                if (EditorBuildSettings.scenes.Length <= 0)
                {
                    Log("No scenes in build settings", LogType.Warning);
                }
                else
                {
                    scenesPaths.AddRange(from s in EditorBuildSettings.scenes select s.path);
                }
            }
            else if (scenes is not null && scenes.Length > 0)
            {
                scenesPaths.AddRange(from s in scenes select s.Path);
            }
            progressVal.description = "Validating assets...";
            var totalLength = assets.Count + scenesPaths.Count;
            var count = 0;
            for (var i = 0; i < assets.Count; i++, count++)
            {
                var obj = assets[i];
                failures.AddRange(GuardRecursively(obj, obj, AssetDatabase.GetAssetPath(obj)));
                progressVal.value = (float)count / totalLength;
                progress?.Invoke(progressVal);
            }
            Log($"Validated {assets.Count} assets");

            if (scenesPaths.Count > 0)
            {
                for (var i = 0; i < scenesPaths.Count; i++, count++)
                {
                    var scene = scenesPaths[i];
                    progressVal.description = "Validating scene: " + scene;
                    GibEditor.ExecuteForComponentsInScene<MonoBehaviour>(scene, m => failures.AddRange(GuardRecursively(m, m.gameObject, scene)));
                    progressVal.value = (float)count / totalLength;
                    progress?.Invoke(progressVal);
                }
                Log($"Validated {(allScenes ? "all" : scenesPaths.Count)} scenes");
            }
        }

        private void Log(string message, LogType type = LogType.Log)
        {
            switch (type)
            {
                case LogType.Log:
                    Debug.Log($"<b>[GuardedValidator]</b> -> {message}");
                    break;

                case LogType.Warning:
                    Debug.LogWarning($"<b>[GuardedValidator]</b> -> {message}");
                    break;

                default:
                    Debug.LogError($"<b>[GuardedValidator]</b> -> {message}");
                    break;
            }
        }

        private ValidatorFailure BuildFailure(GuardedAttribute guarded, FieldInfo field, Type parentClass, string path, bool isAsset)
        {
            var logBuilder = new StringBuilder();
            if (isAsset)
            {
                logBuilder.Append("<color=cyan><b>Prefab</b></color> => ");
            }
            else
            {
                logBuilder.Append("<color=magenta><b>Scene Object</b></color> => ");
            }

            switch (guarded.Gravity)
            {
                case GuardedAttribute.MissingValueGravity.Info:
                    logBuilder.AppendFormat("<b>{0}</b> | Field <b>{1}</b> of class <b>{2}</b> on Object <b>{3}</b> is set to default value", guarded.Message, field.Name, parentClass, path);
                    break;

                case GuardedAttribute.MissingValueGravity.Warning:
                    logBuilder.AppendFormat("<color=yellow><b>{0}</b></color> | Field <b>{1}</b> of class <b>{2}</b> on Object <b>{3}</b> is set to default value", guarded.Message, field.Name, parentClass, path);
                    break;

                case GuardedAttribute.MissingValueGravity.Error:
                    logBuilder.AppendFormat("<color=red><b>{0}</b></color> | Field <b>{1}</b> of class <b>{2}</b> on Object <b>{3}</b> is set to default value", guarded.Message, field.Name, parentClass, path);
                    break;
            }
            return ValidatorFailure.Of(this).Reason(logBuilder.ToString()).By(path, field.Name);
        }

        private List<ValidatorFailure> GuardRecursively(object behaviour, UnityEngine.Object parentObj, string path)
        {
            var failures = new List<ValidatorFailure>();
            if (behaviour is null) return failures;
            var fields = behaviour.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (var i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                if (Attribute.GetCustomAttribute(field, typeof(GuardedAttribute)) is GuardedAttribute guarded)
                {
                    if (field.GetValue(behaviour).IsNullOrDefault())
                    {
                        failures.Add(BuildFailure(guarded, field, behaviour.GetType(), $"{path}/{parentObj.name}", EditorUtility.IsPersistent(parentObj)));
                    }
                }
            }
            return failures;
        }
    }
}