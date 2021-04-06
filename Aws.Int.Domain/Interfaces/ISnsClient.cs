using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aws.Int.Domain.Interfaces
{
    public interface ISnsClient
    {
        Task Send(string topicArn, string message);
    }
}
