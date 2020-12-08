using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class disciplinaryActionUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisciplinaryActions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DisciplinaryActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Action = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Emp_No = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    QueryActionComment = table.Column<string>(nullable: true),
                    QueryActionDate = table.Column<DateTime>(nullable: true),
                    QueryDate = table.Column<DateTime>(nullable: false),
                    QueryMessage = table.Column<string>(nullable: true),
                    QueryReply = table.Column<string>(nullable: true),
                    QueryReplyDate = table.Column<DateTime>(nullable: true),
                    QuerySubject = table.Column<string>(nullable: true),
                    TargetEmployeeId = table.Column<Guid>(nullable: false),
                    TargetEmployeeNo = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisciplinaryActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisciplinaryActions_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DisciplinaryActions_EmployeeId",
                table: "DisciplinaryActions",
                column: "EmployeeId");
        }
    }
}
