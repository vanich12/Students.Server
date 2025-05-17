using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Students.DBCore.Migrations
{
    /// <inheritdoc />
    public partial class FixPendingReq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PendingRequest",
                table: "PendingRequest");

            migrationBuilder.DropColumn(
                name: "tranid",
                table: "PendingRequest");

            migrationBuilder.RenameTable(
                name: "PendingRequest",
                newName: "PendingRequests");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PendingRequests",
                table: "PendingRequests",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PendingRequests",
                table: "PendingRequests");

            migrationBuilder.RenameTable(
                name: "PendingRequests",
                newName: "PendingRequest");

            migrationBuilder.AddColumn<string>(
                name: "tranid",
                table: "PendingRequest",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PendingRequest",
                table: "PendingRequest",
                column: "Id");
        }
    }
}
