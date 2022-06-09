using System.Collections.Generic;
using UnityEngine;

namespace GibFrame.Editor.Validators
{
    public abstract class TypeValidator<T> : Validator where T : Object
    {
        public override void Validate(List<ValidatorFailure> failures)
        {
            var objs = GibEditor.GetAllBehavioursInAsset<T>();
            foreach (var obj in objs)
            {
                var res = ValidateSingle(obj);
                if (res is not null) failures.Add(res);
            }
        }

        protected abstract ValidatorFailure ValidateSingle(T obj);
    }
}