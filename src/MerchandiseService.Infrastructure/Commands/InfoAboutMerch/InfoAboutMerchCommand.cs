using System.Collections.Generic;
using MediatR;
using MerchandiseService.Domain.AggregationModels.MerchItemAggregate;

namespace MerchandiseService.Infrastructure.Commands.InfoAboutMerch
{
    public class InfoAboutMerchCommand : IRequest<List<MerchItem>>
    {
        public long EmployeeId { get; set; }
    }
}