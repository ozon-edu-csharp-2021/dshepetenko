using FluentMigrator;

namespace MerchandiseService.Migrator.Migrations
{
    [Migration(1)]
    public class SkuTable : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                        CREATE TABLE IF NOT EXISTS skus(
                        id BIGSERIAL PRIMARY KEY,
                        name TEXT NOT NULL,
                        merch_type_id INT NOT NULL,
                        clothing_size_id INT NOT NULL);"
            );
        }

        public override void Down()
        {
            Execute.Sql("DROP TABLE if exists skus;");
        }
    }
}