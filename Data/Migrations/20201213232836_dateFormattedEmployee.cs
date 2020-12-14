using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class dateFormattedEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateConf",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveDate",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PreAppDate",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProRetireDate",
                table: "Employees",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateConf",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EffectiveDate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PreAppDate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ProRetireDate",
                table: "Employees");
        }
    }
}
