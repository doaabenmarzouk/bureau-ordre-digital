using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BureauOrdreDigital.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTotpAuthentication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TotpEnabled",
                table: "Utilisateurs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TotpFailedAttempts",
                table: "Utilisateurs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TotpLockedUntil",
                table: "Utilisateurs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TotpSecret",
                table: "Utilisateurs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotpEnabled",
                table: "Utilisateurs");

            migrationBuilder.DropColumn(
                name: "TotpFailedAttempts",
                table: "Utilisateurs");

            migrationBuilder.DropColumn(
                name: "TotpLockedUntil",
                table: "Utilisateurs");

            migrationBuilder.DropColumn(
                name: "TotpSecret",
                table: "Utilisateurs");
        }
    }
}
