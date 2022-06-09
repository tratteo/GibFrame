using System.Collections.Generic;
using UnityEngine;

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

        public List<ValidatorFailure> Validate()
        {
            var failures = new List<ValidatorFailure>();
            if (validators is null || validators.Count < 0) return failures;
            foreach (var v in validators)
            {
                if (v is null) continue;
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