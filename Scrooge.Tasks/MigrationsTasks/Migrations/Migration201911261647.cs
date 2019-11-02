using FluentMigrator;

namespace Scrooge.Task.MigrationsTasks.Migrations
{
    [Migration(201911261647)]
    public class Migration201911261647 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                CREATE TABLE Assets(
	                Id SERIAL,
	                Name varchar(10),
	                IsMain boolean,
	                IsStable boolean
                );

                CREATE SEQUENCE AssetsSequence start 1 increment 1;
            ");
        }

        public override void Down()
        {
            Execute.Sql(@"
                drop sequence AssetsSequence;
                drop table Assets;
            ");
        }
    }

}
