using FluentMigrator;

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
                .WithColumn("CreatedDate").AsDateTime2().WithDefaultValue("datetime()")
                .WithColumn("LastLogin").AsDateTime2();

            Create.Table("Post")
                .WithColumn("Id").AsInt32().Unique().PrimaryKey().Identity()
                .WithColumn("Title").AsString().Unique().NotNullable()
                .WithColumn("Content").AsString().NotNullable()
                .WithColumn("UserId").AsInt32().NotNullable()
                .WithColumn("CreatedDate").AsDateTime2().WithDefaultValue("datetime()")
                .WithColumn("ModifiedDate").AsDateTime2()
                .WithColumn("Featured").AsBoolean().WithDefaultValue("FALSE")
                .WithColumn("Visibility").AsBoolean().WithDefaultValue("TRUE")
                .WithColumn("Tags").AsString().Nullable();

            Create.Table("Reply")
                .WithColumn("Id").AsInt32().Unique().PrimaryKey().Identity()
                .WithColumn("Content").AsString().Unique().NotNullable()
                .WithColumn("UserId").AsInt32().NotNullable()
                .WithColumn("UserName").AsString().Unique().NotNullable()
                .WithColumn("CreatedDate").AsDateTime2().WithDefaultValue("datetime()")
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
}
