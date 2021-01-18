using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class pipUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentAdvanceTracks");

            migrationBuilder.CreateTable(
                name: "PIP",
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
                    PIPSubject = table.Column<string>(nullable: true),
                    PIPMessage = table.Column<string>(nullable: true),
                    DateOfReview = table.Column<DateTime>(nullable: false),
                    LineManager = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PIP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PIP_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PIPItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PIPId = table.Column<Guid>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    PublishBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PIPItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PIPItems_PIP_PIPId",
                        column: x => x.PIPId,
                        principalTable: "PIP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PIP_EmployeeId",
                table: "PIP",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PIPItems_PIPId",
                table: "PIPItems",
                column: "PIPId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PIPItems");

            migrationBuilder.DropTable(
                name: "PIP");

            migrationBuilder.CreateTable(
                name: "PaymentAdvanceTracks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Emp_No = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentAdvanceTracks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentAdvanceTracks_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentAdvanceTracks_EmployeeId",
                table: "PaymentAdvanceTracks",
                column: "EmployeeId");
        }
    }
}
