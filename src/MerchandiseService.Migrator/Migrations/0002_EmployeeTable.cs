using FluentMigrator;

namespace MerchandiseService.Migrator.Migrations
{
    [Migration(2)]
    public class EmployeeTable : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                        CREATE TABLE IF NOT EXISTS employees(
                        id BIGSERIAL PRIMARY KEY,
                        email TEXT NOT NULL);"
            );
        }

        public override void Down()
        {
            Execute.Sql("DROP TABLE if exists employees;");
        }
    }
}