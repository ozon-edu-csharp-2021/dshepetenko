using FluentMigrator;

namespace MerchandiseService.Migrator.Migrations
{
    [Migration(6)]
    public class FillDictionaries : ForwardOnlyMigration
    {
        public override void Up()
        {
            Execute.Sql(@$"
                        INSERT INTO {Tables.ClothingSizeTable} (id, name)
                        VALUES 
                            (1, 'XS'),
                            (2, 'S'),
                            (3, 'M'),
                            (4, 'L'),
                            (5, 'XL'),
                            (6, 'XXL')
                        ON CONFLICT DO NOTHING
                        ");
            
            Execute.Sql(@$"
                        INSERT INTO {Tables.MerchTypeTable} (id, name)
                        VALUES 
                            (1, 'TShirt'),
                            (2, 'Sweatshirt'),
                            (3, 'Notepad'),
                            (4, 'Bag'),
                            (5, 'Pen'),
                            (6, 'Socks')
                        ON CONFLICT DO NOTHING
                        ");
        }
    }
}