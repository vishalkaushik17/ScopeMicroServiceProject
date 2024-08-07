using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBOperationsLayer.Migrations
{
    /// <inheritdoc />
    public partial class add_employmenttype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "EmploymentType",
                table: "EmployeeStudentParentMaster",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmploymentType",
                table: "EmployeeStudentParentMaster");
        }
    }
}
