using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mini_project_emoloyee_management.Migrations
{
    /// <inheritdoc />
    public partial class Addsalary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "salary",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "salary",
                table: "Employees");
        }
    }
}
