using FluentMigrator;
using FluentMigrator.SqlServer;

namespace Scrooge.Migrations.Migrations
{
    [Migration(201910242000)]
    public class Migration201910242000 : Migration
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
