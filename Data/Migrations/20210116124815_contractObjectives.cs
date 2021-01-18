using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class contractObjectives : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContractObjectives",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    EmployeeId = table.Column<Guid>(nullable: false),
                    Emp_No = table.Column<string>(nullable: true),
                    TotalWeightedSore = table.Column<decimal>(nullable: false),
                    IsSignedOff = table.Column<bool>(nullable: false),
                    SignedOffDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractObjectives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractObjectives_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ContractObjectiveId = table.Column<Guid>(nullable: false),
                    SmartObjective = table.Column<string>(nullable: true),
                    EvaluationCiteria = table.Column<string>(nullable: true),
                    Timeline = table.Column<DateTime>(nullable: false),
                    Weighting = table.Column<int>(nullable: false),
                    ScoreAchieved = table.Column<int>(nullable: false),
                    WeightedSore = table.Column<decimal>(nullable: false),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractItems_ContractObjectives_ContractObjectiveId",
                        column: x => x.ContractObjectiveId,
                        principalTable: "ContractObjectives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractItems_ContractObjectiveId",
                table: "ContractItems",
                column: "ContractObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractObjectives_EmployeeId",
                table: "ContractObjectives",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractItems");

            migrationBuilder.DropTable(
                name: "ContractObjectives");
        }
    }
}
