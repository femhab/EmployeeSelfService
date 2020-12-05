using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class approvalWorkItemAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ApprovalWorkItems",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "IsDeleted", "ModifiedBy", "Name", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("a589d2b2-4126-4d7c-9844-f990c436b50f"), "Data Seed", new DateTime(2020, 12, 5, 22, 56, 54, 859, DateTimeKind.Local).AddTicks(8654), "Leave Service", false, null, "Leave", null },
                    { new Guid("8e087cb4-479d-41fa-bb63-520c19afd4ce"), "Data Seed", new DateTime(2020, 12, 5, 22, 56, 54, 860, DateTimeKind.Local).AddTicks(4805), "Loan Service", false, null, "Loan", null },
                    { new Guid("5de656a6-e9fb-4636-a45b-b54532f455c5"), "Data Seed", new DateTime(2020, 12, 5, 22, 56, 54, 860, DateTimeKind.Local).AddTicks(4819), "Appraisal Service", false, null, "Appraisal", null },
                    { new Guid("162252a9-b394-4b2e-b632-617e87c749e9"), "Data Seed", new DateTime(2020, 12, 5, 22, 56, 54, 860, DateTimeKind.Local).AddTicks(4832), "Transfer Service", false, null, "Transfer", null },
                    { new Guid("206f9e17-a09e-4381-bbb3-278003e0c95d"), "Data Seed", new DateTime(2020, 12, 5, 22, 56, 54, 860, DateTimeKind.Local).AddTicks(4833), "Diciplinary Service", false, null, "Diciplinary", null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ApprovalWorkItems",
                keyColumn: "Id",
                keyValue: new Guid("162252a9-b394-4b2e-b632-617e87c749e9"));

            migrationBuilder.DeleteData(
                table: "ApprovalWorkItems",
                keyColumn: "Id",
                keyValue: new Guid("206f9e17-a09e-4381-bbb3-278003e0c95d"));

            migrationBuilder.DeleteData(
                table: "ApprovalWorkItems",
                keyColumn: "Id",
                keyValue: new Guid("5de656a6-e9fb-4636-a45b-b54532f455c5"));

            migrationBuilder.DeleteData(
                table: "ApprovalWorkItems",
                keyColumn: "Id",
                keyValue: new Guid("8e087cb4-479d-41fa-bb63-520c19afd4ce"));

            migrationBuilder.DeleteData(
                table: "ApprovalWorkItems",
                keyColumn: "Id",
                keyValue: new Guid("a589d2b2-4126-4d7c-9844-f990c436b50f"));
        }
    }
}
