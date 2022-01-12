using Microsoft.EntityFrameworkCore.Migrations;

namespace GoodNoodle.Infrastructure.Migrations;

public partial class ChangeNoodleUserNameCases : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "Lastname",
            table: "NoodleUser",
            newName: "LastName");

        migrationBuilder.RenameColumn(
            name: "Firstname",
            table: "NoodleUser",
            newName: "FirstName");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "LastName",
            table: "NoodleUser",
            newName: "Lastname");

        migrationBuilder.RenameColumn(
            name: "FirstName",
            table: "NoodleUser",
            newName: "Firstname");
    }
}
