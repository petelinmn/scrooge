using FluentMigrator;
using Serilog;

namespace Scrooge.Task.MigrationsTasks.Migrations
{
    [Migration(202003291050)]
    public class Migration202003291050 : Migration
    {
        public override void Up()
        {
            Alter.Table("prices").AlterColumn("value").AsDecimal(19, 8).NotNullable();
        }

        public override void Down()
        {
            Alter.Table("prices").AlterColumn("value").AsDecimal().NotNullable();
        }
    }

}
