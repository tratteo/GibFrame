using GibFrame.Validators;
using System.Collections.Generic;
using UnityEngine;

namespace GibFrame.Editor.Validators
{
    public abstract class TypeValidator<T> : Validator where T : Object
    {
        public override List<ValidatorFailure> Validate()
        {
            var objs = GibEditor.GetAllBehavioursInAsset<T>();
            var failures = new List<ValidatorFailure>();
            foreach (var obj in objs)
            {
                var res = ValidateSingle(obj);
                if (res is not null) failures.Add(res);
            }
            return failures;
        }

        protected abstract ValidatorFailure ValidateSingle(T obj);
    }
}