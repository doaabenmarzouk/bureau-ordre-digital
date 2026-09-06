using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BureauOrdreDigital.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailLoginOtp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotpEnabled",
                table: "Utilisateurs");

            migrationBuilder.RenameColumn(
                name: "TotpSecret",
                table: "Utilisateurs",
                newName: "LoginOtpHash");

            migrationBuilder.RenameColumn(
                name: "TotpLockedUntil",
                table: "Utilisateurs",
                newName: "LoginOtpExpiration");

            migrationBuilder.RenameColumn(
                name: "TotpFailedAttempts",
                table: "Utilisateurs",
                newName: "LoginOtpFailedAttempts");

            migrationBuilder.AddColumn<string>(
                name: "LoginChallenge",
                table: "Utilisateurs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LoginChallengeExpiration",
                table: "Utilisateurs",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoginChallenge",
                table: "Utilisateurs");

            migrationBuilder.DropColumn(
                name: "LoginChallengeExpiration",
                table: "Utilisateurs");

            migrationBuilder.RenameColumn(
                name: "LoginOtpHash",
                table: "Utilisateurs",
                newName: "TotpSecret");

            migrationBuilder.RenameColumn(
                name: "LoginOtpFailedAttempts",
                table: "Utilisateurs",
                newName: "TotpFailedAttempts");

            migrationBuilder.RenameColumn(
                name: "LoginOtpExpiration",
                table: "Utilisateurs",
                newName: "TotpLockedUntil");

            migrationBuilder.AddColumn<bool>(
                name: "TotpEnabled",
                table: "Utilisateurs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
