using FluentMigrator;
using Serilog;

namespace Scrooge.Task.MigrationsTasks.Migrations
{
    [Migration(202003140840)]
    public class Migration202003140840 : Migration
    {
        public override void Up()
        {
            Delete.Column("price").FromTable("prices");
            Alter.Table("prices").AddColumn("value").AsDecimal().NotNullable();
        }

        public override void Down()
        {
            Delete.Column("value").FromTable("prices");
            Alter.Table("prices").AddColumn("price").AsDecimal().NotNullable();
        }
    }

}
