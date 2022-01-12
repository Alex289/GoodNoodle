using Microsoft.EntityFrameworkCore.Migrations;

namespace GoodNoodle.Infrastructure.Migrations;

public partial class AddForeignKey : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateIndex(
            name: "IX_UserInGroup_NoodleGroupId",
            table: "UserInGroup",
            column: "NoodleGroupId");

        migrationBuilder.CreateIndex(
            name: "IX_UserInGroup_NoodleUserId",
            table: "UserInGroup",
            column: "NoodleUserId");

        migrationBuilder.AddForeignKey(
            name: "FK_UserInGroup_NoodleGroup_NoodleGroupId",
            table: "UserInGroup",
            column: "NoodleGroupId",
            principalTable: "NoodleGroup",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_UserInGroup_NoodleUser_NoodleUserId",
            table: "UserInGroup",
            column: "NoodleUserId",
            principalTable: "NoodleUser",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_UserInGroup_NoodleGroup_NoodleGroupId",
            table: "UserInGroup");

        migrationBuilder.DropForeignKey(
            name: "FK_UserInGroup_NoodleUser_NoodleUserId",
            table: "UserInGroup");

        migrationBuilder.DropIndex(
            name: "IX_UserInGroup_NoodleGroupId",
            table: "UserInGroup");

        migrationBuilder.DropIndex(
            name: "IX_UserInGroup_NoodleUserId",
            table: "UserInGroup");
    }
}
