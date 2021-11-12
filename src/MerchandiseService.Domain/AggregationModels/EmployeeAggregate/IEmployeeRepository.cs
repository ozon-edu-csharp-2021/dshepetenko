using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MerchandiseService.Domain.AggregationModels.MerchItemAggregate;
using MerchandiseService.Domain.AggregationModels.ValueObjects;
using MerchandiseService.Domain.Contracts;

namespace MerchandiseService.Domain.AggregationModels.EmployeeAggregate
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee> FindByIdAsync(long id, CancellationToken cancellationToken = default);

        Task<Employee> FindByEmployeeIdAsync(EmployeeId employeeId, CancellationToken cancellationToken = default);

        Task<bool> GiveOutMerchItemAsync(Employee employee, MerchItem merchItem,
            CancellationToken cancellationToken = default);

        Task<bool> CheckIfMerchItemIsGivenBySkuAsync(Employee employee, Sku sku,
            CancellationToken cancellationToken = default);

        Task<List<MerchItem>> GetAllGivenMerchItemsOfEmployeeById(EmployeeId employeeId,
            CancellationToken cancellationToken = default);
    }
}