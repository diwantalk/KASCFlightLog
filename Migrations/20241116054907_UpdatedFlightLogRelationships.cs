using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KASCFlightLog.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedFlightLogRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FlightLogs_RegistrationNO",
                table: "FlightLogs");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "IDNumber",
                table: "AspNetUsers",
                newName: "IdNumber");

            migrationBuilder.AlterColumn<bool>(
                name: "IsValid",
                table: "FlightLogs",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublished",
                table: "FlightLogs",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "FlightLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "FlightLogs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidatedAt",
                table: "FlightLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValidatedById",
                table: "FlightLogs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "IdNumber",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_FlightLogs_UserId",
                table: "FlightLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightLogs_ValidatedById",
                table: "FlightLogs",
                column: "ValidatedById");

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
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.DropIndex(
                name: "IX_FlightLogs_UserId",
                table: "FlightLogs");

            migrationBuilder.DropIndex(
                name: "IX_FlightLogs_ValidatedById",
                table: "FlightLogs");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "FlightLogs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FlightLogs");

            migrationBuilder.DropColumn(
                name: "ValidatedAt",
                table: "FlightLogs");

            migrationBuilder.DropColumn(
                name: "ValidatedById",
                table: "FlightLogs");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "IdNumber",
                table: "AspNetUsers",
                newName: "IDNumber");

            migrationBuilder.AlterColumn<bool>(
                name: "IsValid",
                table: "FlightLogs",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublished",
                table: "FlightLogs",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "IDNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_FlightLogs_RegistrationNO",
                table: "FlightLogs",
                column: "RegistrationNO");
        }
    }
}
