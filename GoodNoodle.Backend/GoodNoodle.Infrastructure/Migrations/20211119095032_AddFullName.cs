using Microsoft.EntityFrameworkCore.Migrations;

namespace GoodNoodle.Infrastructure.Migrations;

public partial class AddFullName : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {

        migrationBuilder.AddColumn<string>(
            name: "FullName",
            table: "NoodleUser",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: false,
            defaultValue: "");

        migrationBuilder.Sql("UPDATE NoodleUser SET FullName = FirstName + ' ' + LastName");

        migrationBuilder.DropColumn(
            name: "FirstName",
            table: "NoodleUser");

        migrationBuilder.DropColumn(
            name: "LastName",
            table: "NoodleUser");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "FullName",
            table: "NoodleUser");

        migrationBuilder.AddColumn<string>(
            name: "FirstName",
            table: "NoodleUser",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "LastName",
            table: "NoodleUser",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: false,
            defaultValue: "");
    }
}
