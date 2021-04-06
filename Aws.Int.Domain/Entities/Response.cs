using System;
using System.Collections.Generic;
using System.Text;

namespace Aws.Int.Domain.Entities
{
    public class Response
    {
        public long Id { get; set; }
        public object Content { get; set; }
        public string Error { get; set; }
        public IEnumerable<Validation.DomainValidationMessage> DomainValidationMessages { get; set; }
    }
}
