using Microsoft.EntityFrameworkCore.Migrations;

namespace GoodNoodle.Infrastructure.Migrations;

public partial class AddStarValidations : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "NoodleUserID",
            table: "Star",
            newName: "NoodleUserId");

        migrationBuilder.RenameColumn(
            name: "NoodleGoupID",
            table: "Star",
            newName: "NoodleGoupId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "NoodleUserId",
            table: "Star",
            newName: "NoodleUserID");

        migrationBuilder.RenameColumn(
            name: "NoodleGoupId",
            table: "Star",
            newName: "NoodleGoupID");
    }
}
