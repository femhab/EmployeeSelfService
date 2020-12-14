using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class employeeLocationatallUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Employees");

            migrationBuilder.AddColumn<Guid>(
                name: "AvalaibilityStatusId",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CountryId",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CourtesyId",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeTitleId",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LGAId",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MaritalStatusId",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SectionId",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StateId",
                table: "Employees",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_AvalaibilityStatusId",
                table: "Employees",
                column: "AvalaibilityStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CountryId",
                table: "Employees",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CourtesyId",
                table: "Employees",
                column: "CourtesyId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeTitleId",
                table: "Employees",
                column: "EmployeeTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_LGAId",
                table: "Employees",
                column: "LGAId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_LocationId",
                table: "Employees",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_MaritalStatusId",
                table: "Employees",
                column: "MaritalStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SectionId",
                table: "Employees",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_StateId",
                table: "Employees",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_AvalaibilityStatus_AvalaibilityStatusId",
                table: "Employees",
                column: "AvalaibilityStatusId",
                principalTable: "AvalaibilityStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Countries_CountryId",
                table: "Employees",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Courtesy_CourtesyId",
                table: "Employees",
                column: "CourtesyId",
                principalTable: "Courtesy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_EmployeeTitles_EmployeeTitleId",
                table: "Employees",
                column: "EmployeeTitleId",
                principalTable: "EmployeeTitles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_LGAs_LGAId",
                table: "Employees",
                column: "LGAId",
                principalTable: "LGAs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Locations_LocationId",
                table: "Employees",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_MaritalStatus_MaritalStatusId",
                table: "Employees",
                column: "MaritalStatusId",
                principalTable: "MaritalStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Sections_SectionId",
                table: "Employees",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_States_StateId",
                table: "Employees",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AvalaibilityStatus_AvalaibilityStatusId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Countries_CountryId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Courtesy_CourtesyId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeeTitles_EmployeeTitleId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_LGAs_LGAId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Locations_LocationId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_MaritalStatus_MaritalStatusId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Sections_SectionId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_States_StateId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_AvalaibilityStatusId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CountryId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CourtesyId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_EmployeeTitleId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_LGAId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_LocationId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_MaritalStatusId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_SectionId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_StateId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "AvalaibilityStatusId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CourtesyId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EmployeeTitleId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "LGAId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "MaritalStatusId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Employees",
                nullable: true);
        }
    }
}
