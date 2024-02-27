using FluentMigrator;
using FluentMigrator.Runner;

namespace BBS.Data.Migrations
{
    [Migration(1)]
    public class Initial : Migration
    {
        public override void Up()
        {
            Create.Table("User")
                .WithColumn("Id").AsInt32().Unique().PrimaryKey().Identity()
                .WithColumn("Name").AsString().Unique().NotNullable()
                .WithColumn("Password").AsString().NotNullable()
                .WithColumn("Email").AsString().Nullable()
                .WithColumn("CreatedDate").AsDateTime2().WithDefault(SystemMethods.CurrentDateTime)
                .WithColumn("LastLogin").AsDateTime2();

            Create.Table("Post")
                .WithColumn("Id").AsInt32().Unique().PrimaryKey().Identity()
                .WithColumn("Title").AsString().Unique().NotNullable()
                .WithColumn("Content").AsString().NotNullable()
                .WithColumn("UserId").AsInt32().NotNullable()
                .WithColumn("CreatedDate").AsDateTime2().WithDefault(SystemMethods.CurrentDateTime)
                .WithColumn("ModifiedDate").AsDateTime2()
                .WithColumn("Featured").AsBoolean().WithDefaultValue("FALSE")
                .WithColumn("Visibility").AsBoolean().WithDefaultValue("TRUE")
                .WithColumn("Tags").AsString().Nullable();

            Create.Table("Reply")
                .WithColumn("Id").AsInt32().Unique().PrimaryKey().Identity()
                .WithColumn("Content").AsString().Unique().NotNullable()
                .WithColumn("UserId").AsInt32().NotNullable()
                .WithColumn("UserName").AsString().Unique().NotNullable()
                .WithColumn("CreatedDate").AsDateTime2().WithDefault(SystemMethods.CurrentDateTime)
                .WithColumn("ModifiedDate").AsDateTime2()
                .WithColumn("Visibility").AsBoolean().WithDefaultValue("TRUE");

            Create.Table("Tag")
                .WithColumn("Id").AsInt32().Unique().PrimaryKey().Identity()
                .WithColumn("Name").AsString().Unique().NotNullable()
                .WithColumn("Posts").AsString().Nullable();
        }

        public override void Down()
        {
            Delete.Table("User");
            Delete.Table("Post");
            Delete.Table("Reply");
            Delete.Table("Tag");
        }
    }
    public class Test
    {
        public ServiceProvider CreateServices()
        {
            return new ServiceCollection()
                // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // Add SQLite support to FluentMigrator
                    .AddSQLite()
                    // Set the connection string
                    .WithGlobalConnectionString("Data Source=C:\\BBS.db")
                    // Define the assembly containing the migrations
                    .ScanIn(typeof(Initial).Assembly).For.Migrations())
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build the service provider
                .BuildServiceProvider(false);
        }
        public void UpdateDatabase(IServiceProvider serviceProvider)
        {
            // Instantiate the runner
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            // Execute the migrations
            runner.MigrateUp();
        }
    }
}
