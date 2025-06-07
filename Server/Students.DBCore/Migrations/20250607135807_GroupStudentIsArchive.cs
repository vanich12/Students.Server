using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Students.DBCore.Migrations
{
    /// <inheritdoc />
    public partial class GroupStudentIsArchive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsArchive",
                table: "GroupStudent",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "GroupStudent",
                keyColumn: "RequestId",
                keyValue: new Guid("4178e3fa-dca8-4e28-a815-46cfacb61fe5"),
                column: "IsArchive",
                value: false);

            migrationBuilder.UpdateData(
                table: "GroupStudent",
                keyColumn: "RequestId",
                keyValue: new Guid("7ecc61ae-2472-484e-8078-3b34f3448b8e"),
                column: "IsArchive",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchive",
                table: "GroupStudent");
        }
    }
}
