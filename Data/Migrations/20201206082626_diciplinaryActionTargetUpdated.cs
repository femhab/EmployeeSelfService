using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class diciplinaryActionTargetUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TargetEmployeeId",
                table: "DisciplinaryActions",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
