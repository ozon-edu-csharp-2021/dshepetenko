﻿using System.Collections.Generic;
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
using MerchandiseService.Infrastructure.Repositories.Infrastructure;

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
            string sql = @$"
                            INSERT INTO {Tables.EmployeeTable} (id, email)
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

        public async Task<Employee> UpdateAsync(Employee itemToUpdate, CancellationToken cancellationToken = default)
        {
            string sql = @$"
                            UPDATE {Tables.EmployeeTable}
                            SET email = (@Email)                          
                            WHERE {Tables.EmployeeTable}.id = (@EmployeeId);
                            ";
            var parameters = new
            {
                EmployeeId = itemToUpdate.EmployeeId.Value,
                Email = itemToUpdate.Email.Value
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            await connection.ExecuteAsync(commandDefinition);
            foreach (var givenMerchItem in itemToUpdate.GivenMerchItems)
            {
                await AddGivenMerch(givenMerchItem, itemToUpdate.EmployeeId, cancellationToken);
            }

            foreach (var expectedMerchItem in itemToUpdate.ExpectedMerchItems)
            {
                await AddExpectedMerch(expectedMerchItem, itemToUpdate.EmployeeId, cancellationToken);
            }

            _changeTracker.Track(itemToUpdate);
            return itemToUpdate;
        }

        public async Task AddGivenMerch(MerchItem merchItem, EmployeeId employeeId, CancellationToken cancellationToken)
        {
            string sql = @$"
                            INSERT INTO {Tables.MerchTable}(sku_id, employee_id, date_of_issue, quantity, is_given)
                            VALUES (@Sku, @EmployeeId, @DateOfIssue, @Quantity, true)
                            ON CONFLICT (employee_id, sku_id) DO                                                    
                            UPDATE  {Tables.MerchTable}
                            SET is_given = true                          
                            WHERE employee_id = (@EmployeeId) AND sku_id = (@Sku);
                            INSERT INTO {Tables.SkuTable}(id, name, merch_type_id, clothing_size_id)
                            VALUES (@Sku, @Name, @Type, @Size)
                            ON CONFLICT (id) DO NOTHING";
            var parameters = new
            {
                Sku = merchItem.Sku.Value,
                EmployeeId = employeeId.Value,
                DateOfIssue = merchItem.DateOfIssue,
                Quantity = merchItem.Quantity.Value,
                Name = merchItem.Name.Value,
                Type = merchItem.MerchType.Id,
                Size = merchItem.Size.Id
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            await connection.ExecuteAsync(commandDefinition);
            _changeTracker.Track(merchItem);
        }

        public async Task AddExpectedMerch(MerchItem merchItem, EmployeeId employeeId,
            CancellationToken cancellationToken)
        {
            string sql = @$"
                            INSERT INTO {Tables.MerchTable}(sku_id, employee_id, date_of_issue, quantity, is_given)
                            VALUES (@Sku, @EmployeeId, @DateOfIssue, @Quantity, false)
                            ON CONFLICT (employee_id, sku_id) DO                                                    
                            UPDATE {Tables.MerchTable}
                            SET is_given = true                          
                            WHERE employee_id = (@EmployeeId) AND sku_id = (@Sku);
                            INSERT INTO {Tables.SkuTable}(id, name, merch_type_id, clothing_size_id)
                            VALUES (@Sku, @Name, @Type, @Size)
                            ON CONFLICT (id) DO NOTHING";
            var parameters = new
            {
                Sku = merchItem.Sku.Value,
                EmployeeId = employeeId.Value,
                DateOfIssue = merchItem.DateOfIssue,
                Quantity = merchItem.Quantity.Value,
                Name = merchItem.Name.Value,
                Type = merchItem.MerchType.Id,
                Size = merchItem.Size.Id
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            await connection.ExecuteAsync(commandDefinition);
            _changeTracker.Track(merchItem);
        }

        public async Task<Employee> FindByEmployeeIdAsync(EmployeeId employeeId,
            CancellationToken cancellationToken = default)
        {
            var givenMerch = await GetAllGivenMerchItemsOfEmployeeById(employeeId, cancellationToken);
            var expectedMerch = await GetAllExprectedMerchItemsOfEmployeeById(employeeId, cancellationToken);
            string sql = @$"
                            SELECT id, email
                            FROM {Tables.EmployeeTable}
                            WHERE id = (@EmployeeId)
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
            string sql = @$"
                            UPDATE {Tables.MerchTable}
                            SET is_given = true                          
                            WHERE employee_id = (@EmployeeId) AND sku_id = (@Sku);
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
            string sql = @$"
                            SELECT is_given
                            FROM {Tables.MerchTable}                            
                            WHERE employee_id = (@EmployeeId) AND sku_id = (@Sku);
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
            string sql = @$"
                            SELECT {Tables.SkuTable}.name, {Tables.SkuTable}.clothing_size_id, {Tables.SkuTable}.id, {Tables.SkuTable}.merch_type_id,
                                    {Tables.MerchTable}.quantity, {Tables.MerchTable}.date_of_issue
                            FROM {Tables.SkuTable}
                            INNER JOIN {Tables.MerchTable} on {Tables.MerchTable}.sku_id = {Tables.SkuTable}.id
                            WHERE {Tables.MerchTable}.employee_id = (@EmployeeId) AND {Tables.MerchTable}.is_given = true;
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
            string sql = @$"
                            SELECT {Tables.SkuTable}.name, {Tables.SkuTable}.clothing_size_id, {Tables.SkuTable}.id, {Tables.SkuTable}.merch_type_id,
                                    {Tables.MerchTable}.quantity, {Tables.MerchTable}.date_of_issue
                            FROM {Tables.SkuTable}
                            INNER JOIN {Tables.MerchTable} on {Tables.MerchTable}.sku_id = {Tables.SkuTable}.id
                            WHERE {Tables.MerchTable}.employee_id = (@EmployeeId) AND {Tables.MerchTable}.is_given = false;
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