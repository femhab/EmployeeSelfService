using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class educationDetailUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Course",
                table: "EmployeesEducationDetail",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Emp_No",
                table: "EmployeesEducationDetail",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "EmployeesEducationDetail",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Institution",
                table: "EmployeesEducationDetail",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "EmployeesEducationDetail",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Course",
                table: "EmployeesEducationDetail");

            migrationBuilder.DropColumn(
                name: "Emp_No",
                table: "EmployeesEducationDetail");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "EmployeesEducationDetail");

            migrationBuilder.DropColumn(
                name: "Institution",
                table: "EmployeesEducationDetail");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "EmployeesEducationDetail");
        }
    }
}
