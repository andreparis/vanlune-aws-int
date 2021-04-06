using Aws.Int.Application.MediatR.Base;
using Aws.Int.Domain.Interfaces;
using System.Threading;

namespace Aws.Int.Application.Application.MediatR.Commands.Sns
{
    public class SnsCommandHandler : AbstractRequestHandler<SnsCommand>
    {
        private ISnsClient _snsClient;

        public SnsCommandHandler(ISnsClient snsClient)
        {
            _snsClient = snsClient;
        }

        internal override HandleResponse HandleIt(SnsCommand request, CancellationToken cancellationToken)
        {
            _snsClient.Send(request.TopicArn, request.Message).GetAwaiter().GetResult();

            return new HandleResponse();
        }
    }
}
