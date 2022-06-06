using System.Collections.Generic;

namespace GibFrame.Validators
{
    /// <summary>
    ///   Something that can be validated by a <see cref="ValidationGroup"/>
    /// </summary>
    public interface IValidable
    {
        public List<ValidatorFailure> Validate();
    }
}