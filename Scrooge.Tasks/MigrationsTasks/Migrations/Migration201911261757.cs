using FluentMigrator;
using Serilog;

namespace Scrooge.Task.MigrationsTasks.Migrations
{
    [Migration(201911261757)]
    public class Migration201911261757 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                CREATE TABLE Markets(
	                Id serial,
	                AssetId1 int references Assets(Id),
	                AssetId2 int references Assets(Id)
                );
            ");
        }

        public override void Down()
        {
            Execute.Sql(@"
                drop table Markets;
            ");
        }
    }

}
