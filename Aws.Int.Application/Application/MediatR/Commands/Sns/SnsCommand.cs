using Aws.Int.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aws.Int.Application.Application.MediatR.Commands.Sns
{
    public class SnsCommand : IRequest<Response>
    {
        public string TopicArn { get; set; }
        public string Message { get; set; }
    }
}
