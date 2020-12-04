using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class loanExitProcessDisciplinaryEntityAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeesEducationDetail_EducationalQualification_EducationalQualificationId",
                table: "EmployeesEducationDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EducationalQualification",
                table: "EducationalQualification");

            migrationBuilder.RenameTable(
                name: "EducationalQualification",
                newName: "EducationalQualifications");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EducationalQualifications",
                table: "EducationalQualifications",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DisciplinaryActions",
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
                    QuerySubject = table.Column<string>(nullable: true),
                    QueryMessage = table.Column<string>(nullable: true),
                    QueryDate = table.Column<DateTime>(nullable: false),
                    QueryReply = table.Column<string>(nullable: true),
                    QueryReplyDate = table.Column<DateTime>(nullable: true),
                    QueryActionComment = table.Column<string>(nullable: true),
                    InitiatorId = table.Column<Guid>(nullable: false),
                    QueryActionDate = table.Column<DateTime>(nullable: true),
                    Action = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "ExitProcess",
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
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExitProcess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExitProcess_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExitProcessPriorityItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ExitProcessId = table.Column<Guid>(nullable: false),
                    ExitProcess = table.Column<Guid>(nullable: false),
                    ClearanceDepartmentId = table.Column<Guid>(nullable: false),
                    ClearanceDepartment = table.Column<Guid>(nullable: false),
                    ClearanceOfficer = table.Column<string>(nullable: true),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExitProcessPriorityItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoanTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Loans",
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
                    LoanTypeId = table.Column<Guid>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    NoOfInstallment = table.Column<int>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    AmountRequested = table.Column<decimal>(nullable: false),
                    InterestRate = table.Column<decimal>(nullable: false),
                    AmountApproved = table.Column<decimal>(nullable: false),
                    ApprovedDate = table.Column<DateTime>(nullable: true),
                    InstallmentAmount = table.Column<decimal>(nullable: false),
                    Reason = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    LoanStatus = table.Column<int>(nullable: false),
                    LastApprover = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loans_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Loans_LoanTypes_LoanTypeId",
                        column: x => x.LoanTypeId,
                        principalTable: "LoanTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DisciplinaryActions_EmployeeId",
                table: "DisciplinaryActions",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExitProcess_EmployeeId",
                table: "ExitProcess",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_EmployeeId",
                table: "Loans",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_LoanTypeId",
                table: "Loans",
                column: "LoanTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeesEducationDetail_EducationalQualifications_EducationalQualificationId",
                table: "EmployeesEducationDetail",
                column: "EducationalQualificationId",
                principalTable: "EducationalQualifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeesEducationDetail_EducationalQualifications_EducationalQualificationId",
                table: "EmployeesEducationDetail");

            migrationBuilder.DropTable(
                name: "DisciplinaryActions");

            migrationBuilder.DropTable(
                name: "ExitProcess");

            migrationBuilder.DropTable(
                name: "ExitProcessPriorityItem");

            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "LoanTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EducationalQualifications",
                table: "EducationalQualifications");

            migrationBuilder.RenameTable(
                name: "EducationalQualifications",
                newName: "EducationalQualification");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EducationalQualification",
                table: "EducationalQualification",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeesEducationDetail_EducationalQualification_EducationalQualificationId",
                table: "EmployeesEducationDetail",
                column: "EducationalQualificationId",
                principalTable: "EducationalQualification",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
