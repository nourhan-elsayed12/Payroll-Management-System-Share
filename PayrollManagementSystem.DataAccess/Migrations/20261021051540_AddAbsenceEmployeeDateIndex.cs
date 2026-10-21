using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayrollManagementSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddAbsenceEmployeeDateIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Absences_EmployeeId",
                table: "Absences");

            migrationBuilder.CreateIndex(
                name: "IX_Absences_EmployeeId_Date",
                table: "Absences",
                columns: new[] { "EmployeeId", "Date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Absences_EmployeeId_Date",
                table: "Absences");

            migrationBuilder.CreateIndex(
                name: "IX_Absences_EmployeeId",
                table: "Absences",
                column: "EmployeeId");
        }
    }
}
