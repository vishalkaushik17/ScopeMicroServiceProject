using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBOperationsLayer.Migrations
{
    /// <inheritdoc />
    public partial class addIsHeadPisitonColumInDesignation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportingHeadId",
                table: "EmployeeStudentParentMaster");

            migrationBuilder.AddColumn<bool>(
                name: "IsHeadPosition",
                table: "DesignationMaster",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHeadPosition",
                table: "DesignationMaster");

            migrationBuilder.AddColumn<string>(
                name: "ReportingHeadId",
                table: "EmployeeStudentParentMaster",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
