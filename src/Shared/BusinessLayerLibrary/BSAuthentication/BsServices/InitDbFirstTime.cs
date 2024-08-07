using Microsoft.EntityFrameworkCore.Migrations;
namespace BSAuthentication.BsServices;

public partial class InitDbFirstTime : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        //...

        var sp = @"CREATE PROCEDURE [dbo].[GetStudents]
        AS
        BEGIN
            select * from Students
        END";

        migrationBuilder.Sql(sp);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        //...
    }
}
