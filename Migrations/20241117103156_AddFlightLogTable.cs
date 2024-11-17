using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KASCFlightLog.Migrations
{
    /// <inheritdoc />
    public partial class AddFlightLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlightLogs_AspNetUsers_UserId",
                table: "FlightLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_FlightLogs_AspNetUsers_ValidatedById",
                table: "FlightLogs");

            migrationBuilder.AddColumn<DateTime>(
                name: "FlightDate",
                table: "FlightLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TargetUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_TargetUserId",
                        column: x => x.TargetUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_TargetUserId",
                table: "Notifications",
                column: "TargetUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FlightLogs_AspNetUsers_UserId",
                table: "FlightLogs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlightLogs_AspNetUsers_ValidatedById",
                table: "FlightLogs",
                column: "ValidatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlightLogs_AspNetUsers_UserId",
                table: "FlightLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_FlightLogs_AspNetUsers_ValidatedById",
                table: "FlightLogs");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropColumn(
                name: "FlightDate",
                table: "FlightLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_FlightLogs_AspNetUsers_UserId",
                table: "FlightLogs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlightLogs_AspNetUsers_ValidatedById",
                table: "FlightLogs",
                column: "ValidatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
