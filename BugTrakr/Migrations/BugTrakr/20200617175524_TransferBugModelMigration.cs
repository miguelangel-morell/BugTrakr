using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BugTrakr.Migrations.BugTrakr
{
    public partial class TransferBugModelMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bugs");
        }
    }
}
