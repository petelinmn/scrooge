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
	                Id SERIAL PRIMARY KEY,
	                Name varchar(10),
	                IsMain boolean,
	                IsStable boolean
                );
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
