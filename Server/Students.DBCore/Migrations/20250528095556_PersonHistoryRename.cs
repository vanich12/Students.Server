using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Students.DBCore.Migrations
{
    /// <inheritdoc />
    public partial class PersonHistoryRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentHistory_Persons_PersonId",
                table: "StudentHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentHistory",
                table: "StudentHistory");

            migrationBuilder.RenameTable(
                name: "StudentHistory",
                newName: "PersonHistory");

            migrationBuilder.RenameIndex(
                name: "IX_StudentHistory_PersonId",
                table: "PersonHistory",
                newName: "IX_PersonHistory_PersonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonHistory",
                table: "PersonHistory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonHistory_Persons_PersonId",
                table: "PersonHistory",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonHistory_Persons_PersonId",
                table: "PersonHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonHistory",
                table: "PersonHistory");

            migrationBuilder.RenameTable(
                name: "PersonHistory",
                newName: "StudentHistory");

            migrationBuilder.RenameIndex(
                name: "IX_PersonHistory_PersonId",
                table: "StudentHistory",
                newName: "IX_StudentHistory_PersonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentHistory",
                table: "StudentHistory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentHistory_Persons_PersonId",
                table: "StudentHistory",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id");
        }
    }
}
