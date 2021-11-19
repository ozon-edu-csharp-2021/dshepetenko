using MediatR;
using MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using MerchandiseService.Domain.Contracts;
using MerchandiseService.Infrastructure.Configuration;
using MerchandiseService.Infrastructure.Handlers.GetAvailableQuantityRequestAggregate;
using MerchandiseService.Infrastructure.Repositories.Implementation;
using MerchandiseService.Infrastructure.Repositories.Infrastructure;
using MerchandiseService.Infrastructure.Repositories.Infrastructure.Interfaces;
using Npgsql;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Класс расширений для типа <see cref="IServiceCollection"/> для регистрации инфраструктурных сервисов
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Добавление в DI контейнер инфраструктурных сервисов
        /// </summary>
        /// <param name="services">Объект IServiceCollection</param>
        /// <returns>Объект <see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddMediatR(typeof(GetAvailableQuantityRequestCommandHandler).Assembly);
            services.AddScoped<IDbConnectionFactory<NpgsqlConnection>, NpgsqlConnectionFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IChangeTracker, ChangeTracker>();

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            return services;
        }

        /// <summary>
        /// Добавление в DI контейнер инфраструктурных репозиториев
        /// </summary>
        /// <param name="services">Объект IServiceCollection</param>
        /// <returns>Объект <see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddInfrastructureRepositories(this IServiceCollection services)
        {
            return services;
        }
    }
}