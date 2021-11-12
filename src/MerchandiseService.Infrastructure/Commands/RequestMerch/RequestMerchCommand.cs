using System.Collections.Generic;
using MediatR;
using MerchandiseService.Domain.AggregationModels.MerchItemAggregate;

namespace MerchandiseService.Infrastructure.Commands.RequestMerch
{
    public class RequestMerchCommand : IRequest<bool>
    {
        public long EmployeeId { get; set; }
        public List<MerchItem> MerchItems { get; set; }
    }
}