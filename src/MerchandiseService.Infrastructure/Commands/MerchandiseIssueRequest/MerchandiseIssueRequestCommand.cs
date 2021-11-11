using System.Collections.Generic;
using MediatR;

namespace MerchandiseService.Infrastructure.Commands.MerchandiseIssueRequest
{
    public class MerchandiseIssueRequestCommand : IRequest
    {
        private IReadOnlyList<long> SkuCollection { get; set; }
    }
}