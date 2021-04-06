using Aws.Int.Application.MediatR.Base;
using Aws.Int.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Aws.Int.Application.Application.MediatR.Commands.SecretManager
{
    public class SecretManagerCommandHandler : AbstractRequestHandler<SecretManagerCommand>
    {
        private readonly IAwsSecretManagerService _awsSecretManagerService;

        public SecretManagerCommandHandler(IAwsSecretManagerService awsSecretManagerService)
        {
            _awsSecretManagerService = awsSecretManagerService;
        }

        internal override HandleResponse HandleIt(SecretManagerCommand request, CancellationToken cancellationToken)
        {
            var result = _awsSecretManagerService.GetSecret(request.Secret);

            return new HandleResponse() 
            {
                Content = result
            };
        }
    }
}
