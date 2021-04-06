using System;
using System.Collections.Generic;
using System.Text;

namespace Aws.Int.Domain.Interfaces
{
    public interface IAwsSecretManagerService
    {
        string GetSecret(string secret);
    }
}
