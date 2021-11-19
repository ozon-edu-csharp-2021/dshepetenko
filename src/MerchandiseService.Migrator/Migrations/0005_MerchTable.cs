using FluentMigrator;

namespace MerchandiseService.Migrator.Migrations
{
    [Migration(5)]
    public class MerchTable : Migration 
    {
        public override void Up()
        {
            Execute.Sql(@"
                        CREATE TABLE IF NOT EXISTS merch(
                        id BIGSERIAL PRIMARY KEY,
                        sku_id INT NOT NULL,
                        employee_id BIGINT NOT NULL,
                        date_of_issue TIMESTAMP,
                        quantity INT NOT NULL,                        
                        is_given BOOLEAN NOT NULL);"
            );
        }

        public override void Down()
        {
            Execute.Sql("DROP TABLE if exists merch;");
        }
    }
}