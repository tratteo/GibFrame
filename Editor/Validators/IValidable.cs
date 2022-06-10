using System;
using System.Collections.Generic;

namespace GibFrame.Editor.Validators
{
    public interface IValidable
    {
        public List<ValidatorFailure> Validate(Action<Progress> progress = null);

        public struct Progress
        {
            public string phase;

            public string description;

            public float value;

            public Progress(string phase, string description, float value)
            {
                this.phase = phase;
                this.value = value;
                this.description = description;
            }
        }
    }
}