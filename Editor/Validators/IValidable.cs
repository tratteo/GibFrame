using System.Collections.Generic;

namespace GibFrame.Editor.Validators
{
    public interface IValidable
    {
        public List<ValidatorFailure> Validate();
    }
}