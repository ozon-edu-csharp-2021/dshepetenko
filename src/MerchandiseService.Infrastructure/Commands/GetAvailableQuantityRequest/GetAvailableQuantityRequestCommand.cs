using MediatR;

namespace MerchandiseService.Infrastructure.Commands.GetAvailableQuantityRequest
{
    public class GetAvailableQuantityRequestCommand : IRequest<int>
    {
        public long Sku { get; set; }
    }
}