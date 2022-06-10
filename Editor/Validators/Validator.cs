using System;
using System.Collections.Generic;
using UnityEngine;
using static GibFrame.Editor.Validators.IValidable;

namespace GibFrame.Editor.Validators
{
    /// <summary>
    ///   Extend this base class to create any kind of validator <see cref="ScriptableObject"/> able to be validated
    /// </summary>
    public abstract class Validator : ScriptableObject, IValidable
    {
        [SerializeField, HideInInspector] private bool showResults;
        [SerializeField, HideInInspector] private List<string> latestResultsStrings;
        [SerializeField, HideInInspector] private string lastValidationTime = string.Empty;

        public bool IsBeingValidated { get; private set; }

        public List<ValidatorFailure> Validate(Action<Progress> progress = null)
        {
            IsBeingValidated = true;
            var res = new List<ValidatorFailure>();
            Validate(res, progress);
            IsBeingValidated = false;
            latestResultsStrings = res.ConvertAll(s => s.ToString());
            lastValidationTime = DateTime.Now.ToString();
            return res;
        }

        public abstract void Validate(List<ValidatorFailure> failures, Action<Progress> progress = null);
    }
}