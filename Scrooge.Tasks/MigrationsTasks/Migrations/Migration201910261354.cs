using FluentMigrator;

namespace Scrooge.Task.MigrationsTasks.Migrations
{
    [Migration(201910261354)]
    public class Migration201910261354 : Migration
    {
        public override void Up()
        {
            Create.Schema("scrooge");
        }

        public override void Down()
        {
            Delete.Schema("scrooge");
        }
    }

}
