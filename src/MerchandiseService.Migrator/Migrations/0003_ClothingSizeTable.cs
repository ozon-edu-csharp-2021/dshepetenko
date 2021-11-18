using FluentMigrator;

namespace MerchandiseService.Migrator.Migrations
{
    [Migration(3)]
    public class ClothingSizeTable : Migration 
    {
        public override void Up()
        {
            Execute.Sql(@"
                        CREATE TABLE IF NOT EXISTS clothing_sizes(
                        id BIGSERIAL PRIMARY KEY,
                        name TEXT NOT NULL);"
            );
        }

        public override void Down()
        {
            Execute.Sql("DROP TABLE if exists clothing_sizes;");
        }
    }
}