using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MerchandiseService.Infrastructure.Commands.GetAvailableQuantityRequest;

namespace MerchandiseService.Infrastructure.Handlers.GetAvailableQuantityRequestAggregate
{
    public class GetAvailableQuantityRequestCommandHandler : IRequestHandler<GetAvailableQuantityRequestCommand, int>
    {
        public Task<int> Handle(GetAvailableQuantityRequestCommand request, CancellationToken cancellationToken)
        {
            // Запрос к stock-api
            throw new System.NotImplementedException();
        }
    }
}