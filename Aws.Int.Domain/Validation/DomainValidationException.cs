using System.Collections.Generic;

namespace Aws.Int.Domain.Validation
{
    public class DomainValidationException : System.Exception
    {
        public DomainValidationException(IEnumerable<DomainValidationMessage> messages) : base()
        {
            this.ValidationErrors = messages;
        }

        public IEnumerable<DomainValidationMessage> ValidationErrors { get; }
    }
}
