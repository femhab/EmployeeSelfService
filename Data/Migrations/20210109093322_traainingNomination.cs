using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class traainingNomination : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainingNomination",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    EmployeeId = table.Column<Guid>(nullable: false),
                    TrainingCalenderId = table.Column<Guid>(nullable: false),
                    Emp_No = table.Column<string>(nullable: true),
                    HRTrainingNominationID = table.Column<int>(nullable: false),
                    HRTrainingCalendarID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingNomination", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingNomination_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingNomination_TrainingCalender_TrainingCalenderId",
                        column: x => x.TrainingCalenderId,
                        principalTable: "TrainingCalender",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingNomination_EmployeeId",
                table: "TrainingNomination",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingNomination_TrainingCalenderId",
                table: "TrainingNomination",
                column: "TrainingCalenderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingNomination");
        }
    }
}
