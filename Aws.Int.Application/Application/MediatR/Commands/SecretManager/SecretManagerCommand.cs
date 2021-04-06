using Aws.Int.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aws.Int.Application.Application.MediatR.Commands.SecretManager
{
    public class SecretManagerCommand : IRequest<Response>
    {
        public string Secret { get; set; }
    }
}
