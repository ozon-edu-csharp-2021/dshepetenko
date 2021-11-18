using FluentMigrator;

namespace MerchandiseService.Migrator.Migrations
{
    [Migration(4)]
    public class MerchTypeTable : Migration 
    {
        public override void Up()
        {
            Execute.Sql(@"
                        CREATE TABLE IF NOT EXISTS merch_types(
                        id BIGSERIAL PRIMARY KEY,
                        name TEXT NOT NULL);"
            );
        }

        public override void Down()
        {
            Execute.Sql("DROP TABLE if exists merch_types;");
        }
    }
}