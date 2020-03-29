using FluentMigrator;
using Serilog;

namespace Scrooge.Task.MigrationsTasks.Migrations
{
    [Migration(202029031613)]
    public class Migration202029031613 : Migration
    {
        public override void Up()
        {
            Alter.Table("markets").AddColumn("isactive").AsBoolean().NotNullable().WithDefaultValue(true);
        }

        public override void Down()
        {
            Delete.Column("isactive").FromTable("markets");
        }
    }

}
