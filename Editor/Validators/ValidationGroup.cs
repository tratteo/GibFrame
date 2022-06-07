﻿using System.Collections.Generic;
using UnityEngine;

namespace GibFrame.Editor.Validators
{
    /// <summary>
    ///   A collection of <see cref="IValidable"/> elements that can be validated in the Editor
    /// </summary>
    [CreateAssetMenu(menuName = "GibFrame/Validators/Group", fileName = "validator_group")]
    public sealed class ValidationGroup : ScriptableObject
    {
        [SerializeField] private Validator[] validables;

        /// <summary>
        ///   Validate all the <see cref="IValidable"/> in this group
        /// </summary>
        /// <returns> </returns>
        public List<ValidatorFailure> ValidateAll()
        {
            if (validables is null || validables.Length < 0) return new List<ValidatorFailure>();
            var failures = new List<ValidatorFailure>();
            foreach (var v in validables)
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