using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class employeeStaffTypeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppraisalCategoryItems_AppraisalCategories_AppraisalCategoryId",
                table: "AppraisalCategoryItems");

            migrationBuilder.AddColumn<string>(
                name: "StaffType",
                table: "Employees",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AppraisalCategoryId",
                table: "AppraisalCategoryItems",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<string>(
                name: "AppraisalCategoryCode",
                table: "AppraisalCategoryItems",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppraisalCategoryItemID",
                table: "AppraisalCategoryItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_AppraisalCategoryItems_AppraisalCategories_AppraisalCategoryId",
                table: "AppraisalCategoryItems",
                column: "AppraisalCategoryId",
                principalTable: "AppraisalCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppraisalCategoryItems_AppraisalCategories_AppraisalCategoryId",
                table: "AppraisalCategoryItems");

            migrationBuilder.DropColumn(
                name: "StaffType",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "AppraisalCategoryCode",
                table: "AppraisalCategoryItems");

            migrationBuilder.DropColumn(
                name: "AppraisalCategoryItemID",
                table: "AppraisalCategoryItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "AppraisalCategoryId",
                table: "AppraisalCategoryItems",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppraisalCategoryItems_AppraisalCategories_AppraisalCategoryId",
                table: "AppraisalCategoryItems",
                column: "AppraisalCategoryId",
                principalTable: "AppraisalCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
