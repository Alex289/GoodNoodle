using Microsoft.EntityFrameworkCore.Migrations;

namespace GoodNoodle.Infrastructure.Migrations;

public partial class AddEmail : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Email",
            table: "NoodleUser",
            type: "nvarchar(max)",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Email",
            table: "NoodleUser");
    }
}
