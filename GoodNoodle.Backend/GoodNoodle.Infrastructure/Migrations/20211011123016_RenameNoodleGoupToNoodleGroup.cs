using Microsoft.EntityFrameworkCore.Migrations;

namespace GoodNoodle.Infrastructure.Migrations;

public partial class RenameNoodleGoupToNoodleGroup : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "NoodleGoupId",
            table: "Star",
            newName: "NoodleGroupId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "NoodleGroupId",
            table: "Star",
            newName: "NoodleGoupId");
    }
}
