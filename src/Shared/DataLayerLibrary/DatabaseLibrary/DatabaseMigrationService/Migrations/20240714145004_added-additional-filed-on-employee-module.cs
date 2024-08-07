using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBOperationsLayer.Migrations
{
    /// <inheritdoc />
    public partial class addedadditionalfiledonemployeemodule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Achivements",
                table: "EmployeeStudentParentMaster",
                newName: "Achievements");

            migrationBuilder.AddColumn<int>(
                name: "Citizenship",
                table: "EmployeeStudentParentMaster",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ReportingHeadId",
                table: "EmployeeStudentParentMaster",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Citizenship",
                table: "EmployeeStudentParentMaster");

            migrationBuilder.DropColumn(
                name: "ReportingHeadId",
                table: "EmployeeStudentParentMaster");

            migrationBuilder.RenameColumn(
                name: "Achievements",
                table: "EmployeeStudentParentMaster",
                newName: "Achivements");
        }
    }
}
