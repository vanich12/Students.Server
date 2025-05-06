using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Students.DBCore.Migrations
{
    /// <inheritdoc />
    public partial class AddHistoryOfStudentData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OldFamily = table.Column<string>(type: "text", nullable: false),
                    NewFamily = table.Column<string>(type: "text", nullable: false),
                    OldName = table.Column<string>(type: "text", nullable: false),
                    NewName = table.Column<string>(type: "text", nullable: false),
                    OldPatron = table.Column<string>(type: "text", nullable: false),
                    NewPatron = table.Column<string>(type: "text", nullable: false),
                    ChangeDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentHistory_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentHistory_StudentId",
                table: "StudentHistory",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentHistory");
        }
    }
}
