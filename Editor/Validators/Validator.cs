using System.Collections.Generic;
using UnityEngine;

namespace GibFrame.Editor.Validators
{
    /// <summary>
    ///   Extend this base class to create any kind of validator <see cref="ScriptableObject"/> able to be validated
    /// </summary>
    public abstract class Validator : ScriptableObject, IValidable
    {
        public bool IsBeingValidated { get; private set; }

        public List<ValidatorFailure> Validate()
        {
            IsBeingValidated = true;
            var failures = new List<ValidatorFailure>();
            Validate(failures);
            IsBeingValidated = false;
            return failures;
        }

        public abstract void Validate(List<ValidatorFailure> failures);
    }
}