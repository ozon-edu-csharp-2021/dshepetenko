using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using MerchandiseService.Domain.AggregationModels.MerchItemAggregate;
using MerchandiseService.Infrastructure.Commands.RequestMerch;

namespace MerchandiseService.Infrastructure.Handlers.RequestMerchCommandAggregate
{
    public class RequestMerchCommandHandler : IRequestHandler<RequestMerchCommand, bool>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public RequestMerchCommandHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public Task<bool> Handle(RequestMerchCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}