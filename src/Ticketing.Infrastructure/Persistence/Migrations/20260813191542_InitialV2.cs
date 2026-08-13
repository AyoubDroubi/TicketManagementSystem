using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticketing.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "workspaces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Slug = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Status = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workspaces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequesterId = table.Column<Guid>(type: "uuid", nullable: false),
                    Summary = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(20000)", maxLength: 20000, nullable: true),
                    Priority = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    AssignedTeamId = table.Column<Guid>(type: "uuid", nullable: true),
                    AssignedAgentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Resolution = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ResolvedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ClosedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tickets_workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tickets_WorkspaceId_Priority_CreatedAtUtc",
                table: "tickets",
                columns: new[] { "WorkspaceId", "Priority", "CreatedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_tickets_WorkspaceId_RequesterId_CreatedAtUtc",
                table: "tickets",
                columns: new[] { "WorkspaceId", "RequesterId", "CreatedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_tickets_WorkspaceId_Status_CreatedAtUtc",
                table: "tickets",
                columns: new[] { "WorkspaceId", "Status", "CreatedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_workspaces_Slug",
                table: "workspaces",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "tickets");
            migrationBuilder.DropTable(name: "workspaces");
        }
    }
}
