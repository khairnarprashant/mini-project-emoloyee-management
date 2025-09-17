using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mini_project_emoloyee_management.Migrations
{
    /// <inheritdoc />
    public partial class AddResignationDateToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ResignationDate",
                table: "Employees",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResignationDate",
                table: "Employees");
        }
    }
}
