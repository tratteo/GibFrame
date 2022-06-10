using System;
using System.Collections.Generic;
using static GibFrame.Editor.Validators.IValidable;

namespace GibFrame.Editor.Validators
{
    public abstract class TypeValidator<T> : Validator where T : UnityEngine.Object
    {
        public override void Validate(List<ValidatorFailure> failures, Action<Progress> progress = null)
        {
            var objs = GibEditor.GetAllBehavioursInAsset<T>();
            var progressVal = new Progress(nameof(TypeValidator<T>), "Validating types", 0);
            for (var i = 0; i < objs.Count; i++)
            {
                var obj = objs[i];
                progressVal.value = (float)i / objs.Count;
                progress?.Invoke(progressVal);
                var res = ValidateSingle(obj);
                if (res is not null) failures.Add(res);
            }
        }

        protected abstract ValidatorFailure ValidateSingle(T obj);
    }
}