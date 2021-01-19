using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class recallReformedUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<string>(
            //    name: "Emp_No",
            //    table: "LeaveRecalls",
            //    nullable: true);

            //migrationBuilder.AddColumn<Guid>(
            //    name: "EmployeeId",
            //    table: "LeaveRecalls",
            //    nullable: false,
            //    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            //migrationBuilder.CreateIndex(
            //    name: "IX_LeaveRecalls_EmployeeId",
            //    table: "LeaveRecalls",
            //    column: "EmployeeId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_LeaveRecalls_Employees_EmployeeId",
            //    table: "LeaveRecalls",
            //    column: "EmployeeId",
            //    principalTable: "Employees",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_LeaveRecalls_Employees_EmployeeId",
            //    table: "LeaveRecalls");

            //migrationBuilder.DropIndex(
            //    name: "IX_LeaveRecalls_EmployeeId",
            //    table: "LeaveRecalls");

            //migrationBuilder.DropColumn(
            //    name: "Emp_No",
            //    table: "LeaveRecalls");

            //migrationBuilder.DropColumn(
            //    name: "EmployeeId",
            //    table: "LeaveRecalls");
        }
    }
}
