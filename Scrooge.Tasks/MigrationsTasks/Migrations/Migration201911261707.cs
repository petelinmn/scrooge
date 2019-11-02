using FluentMigrator;

namespace Scrooge.Task.MigrationsTasks.Migrations
{
    [Migration(201911261707)]
    public class Migration201911261707 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                insert into Assets values (nextval('assets_id_seq'), 'BTC', true, false);
                insert into Assets values (nextval('assets_id_seq'), 'USDT', true, true);
                insert into Assets values (nextval('assets_id_seq'), 'ETH', true, false);                
                insert into Assets values (nextval('assets_id_seq'), 'BNB', true, false);
            ");
        }

        public override void Down()
        {

        }
    }

}
