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
        public bool IsBeingValidated { get; private set; }

        public List<ValidatorFailure> Validate(Action<Progress> progress = null)
        {
            IsBeingValidated = true;
            var failures = new List<ValidatorFailure>();
            Validate(failures, progress);
            IsBeingValidated = false;
            return failures;
        }

        public abstract void Validate(List<ValidatorFailure> failures, Action<Progress> progress = null);
    }
}