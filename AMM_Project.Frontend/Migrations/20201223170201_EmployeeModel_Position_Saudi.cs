using Microsoft.EntityFrameworkCore.Migrations;

namespace AMM_Project.Frontend.Migrations
{
    public partial class EmployeeModel_Position_Saudi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSaudiNational",
                table: "Employee",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSaudiNational",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "Employee");
        }
    }
}
