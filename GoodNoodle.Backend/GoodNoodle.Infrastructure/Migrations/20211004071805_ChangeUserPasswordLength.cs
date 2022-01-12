using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace GoodNoodle.Infrastructure.Migrations;

public partial class ChangeUserPasswordLength : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "UserInGroups");

        migrationBuilder.AlterColumn<string>(
            name: "Password",
            table: "NoodleUser",
            type: "nvarchar(128)",
            maxLength: 128,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(50)",
            oldMaxLength: 50);

        migrationBuilder.CreateTable(
            name: "UserInGroup",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                NoodleUserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                NoodleGroupID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Role = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserInGroup", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "UserInGroup");

        migrationBuilder.AlterColumn<string>(
            name: "Password",
            table: "NoodleUser",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(128)",
            oldMaxLength: 128);

        migrationBuilder.CreateTable(
            name: "UserInGroups",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                NoodleGroupID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                NoodleUserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Role = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserInGroups", x => x.Id);
            });
    }
}
