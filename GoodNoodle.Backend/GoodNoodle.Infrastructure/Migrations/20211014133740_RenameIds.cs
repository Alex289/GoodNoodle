using Microsoft.EntityFrameworkCore.Migrations;

namespace GoodNoodle.Infrastructure.Migrations;

public partial class RenameIds : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "NoodleUserID",
            table: "UserInGroup",
            newName: "NoodleUserId");

        migrationBuilder.RenameColumn(
            name: "NoodleGroupID",
            table: "UserInGroup",
            newName: "NoodleGroupId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "NoodleUserId",
            table: "UserInGroup",
            newName: "NoodleUserID");

        migrationBuilder.RenameColumn(
            name: "NoodleGroupId",
            table: "UserInGroup",
            newName: "NoodleGroupID");
    }
}
