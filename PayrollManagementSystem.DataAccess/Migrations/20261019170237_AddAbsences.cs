using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PayrollManagementSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddAbsences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbsenceRuleTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbsenceRuleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Absences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Absences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Absences_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AbsenceRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromDays = table.Column<int>(type: "int", nullable: false),
                    ToDays = table.Column<int>(type: "int", nullable: true),
                    AbsenceRuleTypeId = table.Column<int>(type: "int", nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbsenceRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbsenceRules_AbsenceRuleTypes_AbsenceRuleTypeId",
                        column: x => x.AbsenceRuleTypeId,
                        principalTable: "AbsenceRuleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AbsenceRuleTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "حافز" },
                    { 2, "خصم" }
                });

            migrationBuilder.InsertData(
                table: "AbsenceRules",
                columns: new[] { "Id", "AbsenceRuleTypeId", "FromDays", "Percentage", "ToDays" },
                values: new object[,]
                {
                    { 1, 1, 0, 0m, 1 },
                    { 2, 2, 3, 0m, 5 },
                    { 3, 2, 6, 0m, 10 },
                    { 4, 2, 11, 0m, 15 },
                    { 5, 2, 16, 0m, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbsenceRules_AbsenceRuleTypeId",
                table: "AbsenceRules",
                column: "AbsenceRuleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Absences_EmployeeId",
                table: "Absences",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbsenceRules");

            migrationBuilder.DropTable(
                name: "Absences");

            migrationBuilder.DropTable(
                name: "AbsenceRuleTypes");
        }
    }
}
