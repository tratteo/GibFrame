using System;
using System.Collections.Generic;
using UnityEngine;
using static GibFrame.Editor.Validators.IValidable;

namespace GibFrame.Editor.Validators
{
    /// <summary>
    ///   A collection of <see cref="Validator"/> elements that can be validated in the Editor
    /// </summary>
    [CreateAssetMenu(menuName = "GibFrame/Validators/Group", fileName = "validator_group")]
    public sealed class ValidatorGroup : ScriptableObject, IValidable
    {
        [SerializeField] private List<Validator> validators;

        public IReadOnlyCollection<Validator> Validators => validators;

        public List<ValidatorFailure> Validate(Action<Progress> progress = null)
        {
            var failures = new List<ValidatorFailure>();
            if (validators is null || validators.Count < 0) return failures;
            var progressVal = new Progress(name, "Validating...", 0);
            progress?.Invoke(progressVal);
            for (var i = 0; i < validators.Count; i++)
            {
                var v = validators[i];
                if (v is null) continue;
                progressVal.value = (float)i / validators.Count;
                progress?.Invoke(progressVal);
                var res = v.Validate();
                if (res is not null && res.Count > 0)
                {
                    failures.AddRange(res);
                }
            }
            return failures;
        }
    }
}