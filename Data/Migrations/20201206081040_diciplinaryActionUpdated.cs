using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class diciplinaryActionUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TargetEmployeeNo",
                table: "DisciplinaryActions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetEmployeeNo",
                table: "DisciplinaryActions");

            migrationBuilder.RenameColumn(
                name: "TargetEmployeeId",
                table: "DisciplinaryActions",
                newName: "InitiatorId");
        }
    }
}
