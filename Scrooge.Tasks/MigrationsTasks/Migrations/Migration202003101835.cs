using FluentMigrator;
using Serilog;

namespace Scrooge.Task.MigrationsTasks.Migrations
{
    [Migration(202003101835)]
    public class Migration202003101835 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                CREATE TABLE Prices(
	                Id SERIAL PRIMARY KEY,
	                MarketId int references Markets(Id),
	                Price money,
                    Date timestamp
                );
            ");
        }

        public override void Down()
        {
            Execute.Sql(@"
                drop table Prices;
            ");
        }
    }

}
