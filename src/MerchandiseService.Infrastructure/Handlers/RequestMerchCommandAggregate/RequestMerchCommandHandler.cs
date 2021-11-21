using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using MerchandiseService.Domain.AggregationModels.MerchItemAggregate;
using MerchandiseService.Domain.Contracts;
using MerchandiseService.Infrastructure.Commands.RequestMerch;

namespace MerchandiseService.Infrastructure.Handlers.RequestMerchCommandAggregate
{
    public class RequestMerchCommandHandler : IRequestHandler<RequestMerchCommand, bool>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RequestMerchCommandHandler(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork)
        {
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(RequestMerchCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.StartTransaction(cancellationToken);
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
                
                //Вызов MerchandiseIssueRequest
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}