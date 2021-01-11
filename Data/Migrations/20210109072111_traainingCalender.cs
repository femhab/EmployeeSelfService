using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class traainingCalender : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainingCalender",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    HRTrainingCalenderID = table.Column<int>(nullable: false),
                    TrainingYear = table.Column<int>(nullable: false),
                    TopicId = table.Column<Guid>(nullable: false),
                    StartDate = table.Column<string>(nullable: true),
                    EndDate = table.Column<string>(nullable: true),
                    Organiser = table.Column<string>(nullable: true),
                    Venue = table.Column<string>(nullable: true),
                    AmtPerHead = table.Column<decimal>(nullable: false),
                    InternalFlag = table.Column<bool>(nullable: false),
                    TrainingRoomID = table.Column<int>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    HoursPerDay = table.Column<int>(nullable: false),
                    IsInternational = table.Column<int>(nullable: false),
                    Emp_no = table.Column<string>(nullable: true),
                    TrainingCategory = table.Column<string>(nullable: true),
                    Attended = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingCalender", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingCalender_TrainingTopics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "TrainingTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCalender_TopicId",
                table: "TrainingCalender",
                column: "TopicId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingCalender");
        }
    }
}
