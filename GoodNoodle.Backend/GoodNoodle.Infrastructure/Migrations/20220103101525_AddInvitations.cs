using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace GoodNoodle.Infrastructure.Migrations;
public partial class AddInvitations : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Invitations",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                NoodleUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                NoodleGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Role = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Invitations", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Invitations");
    }
}
