using FluentMigrator;
using Serilog;

namespace Scrooge.Task.MigrationsTasks.Migrations
{
    [Migration(201911261647)]
    public class Migration201911261647 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                CREATE TABLE Assets(
	                Id SERIAL PRIMARY KEY,
	                Name varchar(10),
	                IsMain boolean,
	                IsStable boolean
                );

                insert into Assets values (nextval('assets_id_seq'), 'BTC', true, false);
                insert into Assets values (nextval('assets_id_seq'), 'USDT', true, true);
                insert into Assets values (nextval('assets_id_seq'), 'ETH', true, false);                
                insert into Assets values (nextval('assets_id_seq'), 'BNB', true, false);
            ");
        }

        public override void Down()
        {
            Execute.Sql(@"
                drop table Assets;
            ");
        }
    }

}
