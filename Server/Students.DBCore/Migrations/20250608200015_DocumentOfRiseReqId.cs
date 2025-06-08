using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Students.DBCore.Migrations
{
    /// <inheritdoc />
    public partial class DocumentOfRiseReqId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RequestId",
                table: "DocumentRiseQualifications",
                type: "uuid",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DocumentRiseQualifications",
                keyColumn: "Id",
                keyValue: new Guid("5dde5f20-a247-45b5-b989-800a858d0b70"),
                column: "RequestId",
                value: null);

            migrationBuilder.UpdateData(
                table: "DocumentRiseQualifications",
                keyColumn: "Id",
                keyValue: new Guid("b28d1f29-0aa9-4209-8bab-cd49a8ad548d"),
                column: "RequestId",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "DocumentRiseQualifications");
        }
    }
}
