using FluentMigrator;

namespace Scrooge.Task.MigrationsTasks.Migrations
{
    [Migration(201911261707)]
    public class Migration201911261707 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                insert into Assets values (nextval('AssetsSequence'), 'BTC', true, false);
                insert into Assets values (nextval('AssetsSequence'), 'USDT', true, true);
                insert into Assets values (nextval('AssetsSequence'), 'ETH', true, false);                
                insert into Assets values (nextval('AssetsSequence'), 'BNB', true, false);
            ");
        }

        public override void Down()
        {

        }
    }

}
