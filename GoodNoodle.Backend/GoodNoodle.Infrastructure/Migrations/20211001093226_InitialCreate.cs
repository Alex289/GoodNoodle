using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace GoodNoodle.Infrastructure.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "NoodleGroup",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_NoodleGroup", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "NoodleUser",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Firstname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Lastname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Status = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_NoodleUser", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Star",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                NoodleUserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                NoodleGoupID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Reason = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Star", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "UserInGroups",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                NoodleUserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                NoodleGroupID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Role = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserInGroups", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "NoodleGroup");

        migrationBuilder.DropTable(
            name: "NoodleUser");

        migrationBuilder.DropTable(
            name: "Star");

        migrationBuilder.DropTable(
            name: "UserInGroups");
    }
}
