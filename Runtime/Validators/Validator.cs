using System.Collections.Generic;
using UnityEngine;

namespace GibFrame.Validators
{
    /// <summary>
    ///   Extend this base class to create any kind of validator <see cref="ScriptableObject"/> able to be validated
    /// </summary>
    public abstract class Validator : ScriptableObject
    {
        public abstract List<ValidatorFailure> Validate();
    }
}