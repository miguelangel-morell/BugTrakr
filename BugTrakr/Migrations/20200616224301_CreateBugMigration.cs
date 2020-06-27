using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BugTrakr.Migrations
{
    public partial class CreateBugMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BugAssignmentHistories",
                columns: table => new
                {
                    BugAssignmentHistoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BugId = table.Column<int>(nullable: false),
                    BugStatus = table.Column<int>(nullable: false),
                    AssignedToId = table.Column<int>(nullable: false),
                    RoleName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BugAssignmentHistories", x => x.BugAssignmentHistoryId);
                });

            migrationBuilder.CreateTable(
                name: "Bugs",
                columns: table => new
                {
                    BugId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BugTitle = table.Column<string>(maxLength: 100, nullable: false),
                    BugStatus = table.Column<int>(nullable: false),
                    AssignedToId = table.Column<int>(nullable: false),
                    AssignedById = table.Column<int>(nullable: false),
                    BugDescription = table.Column<string>(maxLength: 2147483647, nullable: false),
                    BugDifficulty = table.Column<int>(nullable: false),
                    IsComplete = table.Column<bool>(nullable: false),
                    BugCreationTimeStamp = table.Column<DateTime>(nullable: false),
                    BugCompletedTimeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bugs", x => x.BugId);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BugId = table.Column<int>(nullable: false),
                    CommentText = table.Column<string>(maxLength: 2147483647, nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationType = table.Column<int>(nullable: false),
                    NotificationMessage = table.Column<string>(maxLength: 500, nullable: true),
                    ReceipientId = table.Column<int>(nullable: false),
                    SenderId = table.Column<int>(nullable: false),
                    BugId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BugAssignmentHistories");

            migrationBuilder.DropTable(
                name: "Bugs");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Notifications");
        }
    }
}
