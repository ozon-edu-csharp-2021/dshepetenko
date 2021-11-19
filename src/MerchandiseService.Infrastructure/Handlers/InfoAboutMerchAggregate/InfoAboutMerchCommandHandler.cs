using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using MerchandiseService.Domain.AggregationModels.MerchItemAggregate;
using MerchandiseService.Domain.Contracts;
using MerchandiseService.Infrastructure.Commands.InfoAboutMerch;

namespace MerchandiseService.Infrastructure.Handlers.InfoAboutMerchAggregate
{
    public class InfoAboutMerchCommandHandler : IRequestHandler<InfoAboutMerchCommand, List<MerchItem>>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InfoAboutMerchCommandHandler(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork)
        {
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<MerchItem>> Handle(InfoAboutMerchCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.StartTransaction(cancellationToken);
            var employee =
                await _employeeRepository.FindByEmployeeIdAsync(new EmployeeId(request.EmployeeId), cancellationToken);
            if (employee is null)
            {
                throw new Exception($"The employee with id = {request.EmployeeId} does not exists!");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return employee.GivenMerchItems;
        }
    }
}