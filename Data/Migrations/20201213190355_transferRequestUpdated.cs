using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class transferRequestUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppliedTransfers_Units_UnitId",
                table: "AppliedTransfers");

            migrationBuilder.AlterColumn<Guid>(
                name: "UnitId",
                table: "AppliedTransfers",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "SectionId",
                table: "AppliedTransfers",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AppliedTransfers_SectionId",
                table: "AppliedTransfers",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppliedTransfers_Sections_SectionId",
                table: "AppliedTransfers",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppliedTransfers_Units_UnitId",
                table: "AppliedTransfers",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppliedTransfers_Sections_SectionId",
                table: "AppliedTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_AppliedTransfers_Units_UnitId",
                table: "AppliedTransfers");

            migrationBuilder.DropIndex(
                name: "IX_AppliedTransfers_SectionId",
                table: "AppliedTransfers");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "AppliedTransfers");

            migrationBuilder.AlterColumn<Guid>(
                name: "UnitId",
                table: "AppliedTransfers",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppliedTransfers_Units_UnitId",
                table: "AppliedTransfers",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
