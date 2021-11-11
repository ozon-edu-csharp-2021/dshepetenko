using MediatR;

namespace MerchandiseService.Infrastructure.Commands.GetAvailableQuantityRequest
{
    public class GetAvailableQuantityRequestCommand : IRequest
    {
        public long Sku { get; set; }
    }
}