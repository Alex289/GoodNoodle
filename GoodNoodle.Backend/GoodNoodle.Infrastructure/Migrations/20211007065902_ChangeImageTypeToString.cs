using Microsoft.EntityFrameworkCore.Migrations;

namespace GoodNoodle.Infrastructure.Migrations;

public partial class ChangeImageTypeToString : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "Image",
            table: "NoodleGroup",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(byte[]),
            oldType: "varbinary(max)");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<byte[]>(
            name: "Image",
            table: "NoodleGroup",
            type: "varbinary(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");
    }
}
