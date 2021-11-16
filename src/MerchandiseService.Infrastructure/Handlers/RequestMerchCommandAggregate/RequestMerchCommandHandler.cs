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

        public async Task<bool> Handle(RequestMerchCommand request, CancellationToken cancellationToken)
        {
            Employee employee = await _employeeRepository.FindByEmployeeIdAsync(
                new EmployeeId(request.EmployeeId), cancellationToken);
            List<MerchItem> merch = request.MerchItems;
            foreach (var merchItem in merch)
            {
                if (!employee.IsPossibleToIssue(merchItem.Sku))
                {
                    return false;
                }

                //запрос к stock-api для проверки доступного объема мерча
                if (Stubs.Stubs.StubGetAvailableQuantity(merchItem.Sku.Value) < merchItem.Quantity.Value)
                {
                    return false;
                }
            }

            return true;
        }
    }
}