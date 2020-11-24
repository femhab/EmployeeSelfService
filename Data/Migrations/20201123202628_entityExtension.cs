using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class entityExtension : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EducationalQualification_EducationalLevels_EducationalLevelId",
                table: "EducationalQualification");

            migrationBuilder.DropIndex(
                name: "IX_EducationalQualification_EducationalLevelId",
                table: "EducationalQualification");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "LeaveTypes");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EducationalLevelId",
                table: "EducationalQualification");

            migrationBuilder.AddColumn<int>(
                name: "Class",
                table: "LeaveTypes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualEndDate",
                table: "Leaves",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Emp_No",
                table: "Leaves",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeaveStatus",
                table: "Leaves",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ApprovalWorkItemId",
                table: "EmployeesApprovalConfig",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "EmployeesAddress",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DOB",
                table: "EmployeeNOKDetails",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "EmployeeNOKDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EducationalLevelCode",
                table: "EducationalQualification",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EducationalLevelCode",
                table: "EducationalGrades",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppliedNameUpdates",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    EmployeeId = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    Emp_No = table.Column<string>(nullable: true),
                    ApprovalStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppliedNameUpdates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppliedNameUpdates_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppliedTransfers",
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
                    DivisionId = table.Column<Guid>(nullable: false),
                    DepartmentId = table.Column<Guid>(nullable: false),
                    UnitId = table.Column<Guid>(nullable: false),
                    ApprovalStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppliedTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppliedTransfers_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppliedTransfers_Divisions_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "Divisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppliedTransfers_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AppliedTransfers_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeesApprovalConfig_ApprovalWorkItemId",
                table: "EmployeesApprovalConfig",
                column: "ApprovalWorkItemId");

            migrationBuilder.CreateIndex(
                name: "IX_AppliedNameUpdates_EmployeeId",
                table: "AppliedNameUpdates",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppliedTransfers_DepartmentId",
                table: "AppliedTransfers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppliedTransfers_DivisionId",
                table: "AppliedTransfers",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_AppliedTransfers_EmployeeId",
                table: "AppliedTransfers",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppliedTransfers_UnitId",
                table: "AppliedTransfers",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeesApprovalConfig_ApprovalWorkItems_ApprovalWorkItemId",
                table: "EmployeesApprovalConfig",
                column: "ApprovalWorkItemId",
                principalTable: "ApprovalWorkItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeesApprovalConfig_ApprovalWorkItems_ApprovalWorkItemId",
                table: "EmployeesApprovalConfig");

            migrationBuilder.DropTable(
                name: "AppliedNameUpdates");

            migrationBuilder.DropTable(
                name: "AppliedTransfers");

            migrationBuilder.DropIndex(
                name: "IX_EmployeesApprovalConfig_ApprovalWorkItemId",
                table: "EmployeesApprovalConfig");

            migrationBuilder.DropColumn(
                name: "Class",
                table: "LeaveTypes");

            migrationBuilder.DropColumn(
                name: "ActualEndDate",
                table: "Leaves");

            migrationBuilder.DropColumn(
                name: "Emp_No",
                table: "Leaves");

            migrationBuilder.DropColumn(
                name: "LeaveStatus",
                table: "Leaves");

            migrationBuilder.DropColumn(
                name: "ApprovalWorkItemId",
                table: "EmployeesApprovalConfig");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "EmployeesAddress");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "EmployeeNOKDetails");

            migrationBuilder.DropColumn(
                name: "EducationalLevelCode",
                table: "EducationalQualification");

            migrationBuilder.DropColumn(
                name: "EducationalLevelCode",
                table: "EducationalGrades");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "LeaveTypes",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Password",
                table: "Employees",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DOB",
                table: "EmployeeNOKDetails",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EducationalLevelId",
                table: "EducationalQualification",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EducationalQualification_EducationalLevelId",
                table: "EducationalQualification",
                column: "EducationalLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationalQualification_EducationalLevels_EducationalLevelId",
                table: "EducationalQualification",
                column: "EducationalLevelId",
                principalTable: "EducationalLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
