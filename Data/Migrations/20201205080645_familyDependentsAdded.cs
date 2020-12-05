using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class familyDependentsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "RoleType");

            migrationBuilder.DropColumn(
                name: "ClearanceDepartment",
                table: "ExitProcessPriorityItem");

            migrationBuilder.AddColumn<bool>(
                name: "IsAllowanceRequested",
                table: "Leaves",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "ExitProcessPriorityItem",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EmployeeFamilyDependents",
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
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    DOB = table.Column<DateTime>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    RelationshipId = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeFamilyDependents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeFamilyDependents_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeFamilyDependents_Relationships_RelationshipId",
                        column: x => x.RelationshipId,
                        principalTable: "Relationships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExitProcessPriorityItem_DepartmentId",
                table: "ExitProcessPriorityItem",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeFamilyDependents_EmployeeId",
                table: "EmployeeFamilyDependents",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeFamilyDependents_RelationshipId",
                table: "EmployeeFamilyDependents",
                column: "RelationshipId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExitProcessPriorityItem_Departments_DepartmentId",
                table: "ExitProcessPriorityItem",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExitProcessPriorityItem_Departments_DepartmentId",
                table: "ExitProcessPriorityItem");

            migrationBuilder.DropTable(
                name: "EmployeeFamilyDependents");

            migrationBuilder.DropIndex(
                name: "IX_ExitProcessPriorityItem_DepartmentId",
                table: "ExitProcessPriorityItem");

            migrationBuilder.DropColumn(
                name: "IsAllowanceRequested",
                table: "Leaves");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "ExitProcessPriorityItem");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "RoleType",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ClearanceDepartment",
                table: "ExitProcessPriorityItem",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
