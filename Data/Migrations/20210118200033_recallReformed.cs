using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class recallReformed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateFrom",
                table: "LeaveRecalls");

            migrationBuilder.DropColumn(
                name: "DateTo",
                table: "LeaveRecalls");

            migrationBuilder.DropColumn(
                name: "RecallDate",
                table: "LeaveRecalls");

            migrationBuilder.DropColumn(
                name: "ResumptionDate",
                table: "LeaveRecalls");

            migrationBuilder.AddColumn<int>(
                name: "ApprovalStatus",
                table: "LeaveRecalls",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "LeaveRecalls");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateFrom",
                table: "LeaveRecalls",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTo",
                table: "LeaveRecalls",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "RecallDate",
                table: "LeaveRecalls",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ResumptionDate",
                table: "LeaveRecalls",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
