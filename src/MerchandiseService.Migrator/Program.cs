using System.IO;
using System.Linq;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace MerchandiseService.Migrator
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsetting.json")
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetSection("DatabaseConnectionOptions:ConnectionSting").Get<string>();

            var services = new ServiceCollection().AddFluentMigratorCore()
                .ConfigureRunner(
                    r =>
                        r.AddPostgres()
                            .WithGlobalConnectionString(connectionString)
                            .ScanIn(typeof(Program).Assembly)
                            .For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole());
            var serviceProvider = services.BuildServiceProvider();
            using (serviceProvider.CreateScope())
            {
                var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
                if (args.Contains("--dryrun"))
                {
                    runner.ListMigrations();
                }
                else
                {
                    runner.MigrateUp();
                }

                using var connection = new NpgsqlConnection(connectionString);
                connection.Open();
                connection.ReloadTypes();
            }
        }
    }
}