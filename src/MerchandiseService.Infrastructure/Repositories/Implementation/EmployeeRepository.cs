using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using MerchandiseService.Domain.AggregationModels.MerchItemAggregate;
using MerchandiseService.Domain.AggregationModels.ValueObjects;
using MerchandiseService.Domain.Contracts;
using MerchandiseService.Infrastructure.Repositories.Infrastructure.Interfaces;
using Npgsql;
using Dapper;

namespace MerchandiseService.Infrastructure.Repositories.Implementation
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;

        private readonly IChangeTracker _changeTracker;

        private const int Timeout = 5;

        public EmployeeRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory,
            IChangeTracker changeTracker)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _changeTracker = changeTracker;
        }

        public async Task<Employee> CreateAsync(Employee itemToCreate, CancellationToken cancellationToken = default)
        {
            string sql = @"
                            INSERT INTO employees (id, email)
                            VALUES (@EmployeeId, @Email);";
            var parameters = new
            {
                EmployeeId = itemToCreate.EmployeeId.Value,
                Email = itemToCreate.Email.Value
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            await connection.ExecuteAsync(commandDefinition);
            _changeTracker.Track(itemToCreate);
            return itemToCreate;
        }

        public Task<Employee> UpdateAsync(Employee itemToUpdate, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Employee> FindByEmployeeIdAsync(EmployeeId employeeId,
            CancellationToken cancellationToken = default)
        {
            var givenMerch = await GetAllGivenMerchItemsOfEmployeeById(employeeId, cancellationToken);
            var expectedMerch = await GetAllExprectedMerchItemsOfEmployeeById(employeeId, cancellationToken);
            string sql = @"
                            SELECT employees.id, employees.email
                            FROM employees
                            WHERE employees.id = (@EmployeeId)
                            ";
            var parameters = new
            {
                EmployeeId = employeeId.Value,
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            var employeeDatas = await connection.QueryAsync<Models.Employee>(commandDefinition);
            var employeeData = employeeDatas.First();
            var result = new Employee(
                new EmployeeId(employeeData.Id),
                Email.Create(employeeData.Email),
                givenMerch,
                expectedMerch
            );
            _changeTracker.Track(result);
            return result;
        }

        public async Task<bool> GiveOutMerchItemAsync(Employee employee, MerchItem merchItem,
            CancellationToken cancellationToken = default)
        {
            string sql = @"
                            UPDATE merch
                            SET is_given = true                          
                            WHERE merch.employee_id = (@EmployeeId) AND merch.sku_id = (@Sku);
                            ";
            var parameters = new
            {
                EmployeeId = employee.EmployeeId.Value,
                Sku = merchItem.Sku.Value
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            await connection.ExecuteAsync(commandDefinition);
            _changeTracker.Track(merchItem);
            _changeTracker.Track(employee);
            return true;
        }

        public async Task<bool> CheckIfMerchItemIsGivenBySkuAsync(Employee employee, Sku sku,
            CancellationToken cancellationToken = default)
        {
            string sql = @"
                            SELECT merch.is_given
                            FROM merch                            
                            WHERE merch.employee_id = (@EmployeeId) AND merch.sku_id = (@Sku);
                            ";
            var parameters = new
            {
                EmployeeId = employee.EmployeeId.Value,
                Sku = sku.Value
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            var result = await connection.QueryAsync<bool>(commandDefinition);
            _changeTracker.Track(employee);
            return result.First();
        }

        public async Task<List<MerchItem>> GetAllGivenMerchItemsOfEmployeeById(EmployeeId employeeId,
            CancellationToken cancellationToken = default)
        {
            string sql = @"
                            SELECT skus.name, skus.clothing_size_id, skus.id, skus.merch_type_id,
                                    merch.quantity, merch.date_of_issue
                            FROM skus
                            INNER JOIN merch on merch.sku_id = skus.id
                            WHERE merch.employee_id = (@EmployeeId) AND merch.is_given = true;
                            ";
            var parameters = new
            {
                EmployeeId = employeeId.Value,
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);

            var merchItems = await connection.QueryAsync<Models.Sku, Models.MerchItem, MerchItem>(commandDefinition,
                (skuModel, merchModel) => new MerchItem(
                    new Name(skuModel.Name),
                    Size.CreateSizeById(skuModel.ClothingSize),
                    new Sku(skuModel.Id),
                    MerchType.GetTypeById(skuModel.MerchTypeId),
                    new Quantity(merchModel.Quantity),
                    merchModel.DateOfIssue));
            var result = merchItems.ToList();
            foreach (var merchItem
                in result)
            {
                _changeTracker.Track(merchItem);
            }

            return result;
        }

        public async Task<List<MerchItem>> GetAllExprectedMerchItemsOfEmployeeById(EmployeeId employeeId,
            CancellationToken cancellationToken = default)
        {
            string sql = @"
                            SELECT skus.name, skus.clothing_size_id, skus.id, skus.merch_type_id,
                                    merch.quantity, merch.date_of_issue
                            FROM skus
                            INNER JOIN merch on merch.sku_id = skus.id
                            WHERE merch.employee_id = (@EmployeeId) AND merch.is_given = false;
                            ";
            var parameters = new
            {
                EmployeeId = employeeId.Value,
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);

            var merchItems = await connection.QueryAsync<Models.Sku, Models.MerchItem, MerchItem>(commandDefinition,
                (skuModel, merchModel) => new MerchItem(
                    new Name(skuModel.Name),
                    Size.CreateSizeById(skuModel.ClothingSize),
                    new Sku(skuModel.Id),
                    MerchType.GetTypeById(skuModel.MerchTypeId),
                    new Quantity(merchModel.Quantity),
                    merchModel.DateOfIssue));
            var result = merchItems.ToList();
            foreach (var merchItem
                in result)
            {
                _changeTracker.Track(merchItem);
            }

            return result;
        }
    }
}