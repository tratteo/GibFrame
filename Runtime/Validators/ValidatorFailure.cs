using System.Text;

namespace GibFrame.Validators
{
    public class ValidatorFailure
    {
        public Validator Validator { get; private set; }

        public int Code { get; private set; }

        public string Reason { get; private set; }

        public object[] Causers { get; private set; }

        private ValidatorFailure(Validator validator)
        {
            Validator = validator;
            Code = 0;
            Reason = string.Empty;
            Causers = new object[0];
        }

        public static ValidatorFailureBuilder Of(Validator validator) => new ValidatorFailureBuilder(validator);

        public override string ToString()
        {
            var builder = new StringBuilder($"{Validator} failure [{Code}]: {Reason}");
            if (Causers.Length > 0) builder.Append(". Caused by ");
            for (var i = 0; i < Causers.Length; i++)
            {
                var o = Causers[i];

                builder.Append(o);
                if (i < Causers.Length - 1)
                {
                    builder.Append(" | ");
                }
            }
            return builder.ToString();
        }

        public class ValidatorFailureBuilder
        {
            private readonly ValidatorFailure failure;

            public ValidatorFailureBuilder(Validator validator)
            {
                failure = new ValidatorFailure(validator);
            }

            public static implicit operator ValidatorFailure(ValidatorFailureBuilder builder) => builder.Build();

            public ValidatorFailureBuilder Reason(string reason)
            {
                failure.Reason = reason;
                return this;
            }

            public ValidatorFailureBuilder Code(int code)
            {
                failure.Code = code;
                return this;
            }

            public ValidatorFailureBuilder By(params object[] causers)
            {
                failure.Causers = causers;
                return this;
            }

            public ValidatorFailure Build() => failure;
        }
    }
}