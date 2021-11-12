using System.Collections.Generic;
using MediatR;
using MerchandiseService.Domain.AggregationModels.MerchItemAggregate;

namespace MerchandiseService.Infrastructure.Commands.MerchandiseIssueRequest
{
    public class MerchandiseIssueRequestCommand : IRequest
    {
        private IReadOnlyList<MerchItem> MerchItemCollection { get; set; }
    }
}