using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BureauOrdreDigital.Api.Migrations
{
    /// <inheritdoc />
    public partial class AjouterChampsOrganisme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Ville",
                table: "Organismes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ChefProjetCsm",
                table: "Organismes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CodeDivalto",
                table: "Organismes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FonctionContact",
                table: "Organismes",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MailContact",
                table: "Organismes",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "MontantContrat",
                table: "Organismes",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomContact",
                table: "Organismes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NombreUtilisateurs",
                table: "Organismes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TelContact",
                table: "Organismes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VersionElise",
                table: "Organismes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChefProjetCsm",
                table: "Organismes");

            migrationBuilder.DropColumn(
                name: "CodeDivalto",
                table: "Organismes");

            migrationBuilder.DropColumn(
                name: "FonctionContact",
                table: "Organismes");

            migrationBuilder.DropColumn(
                name: "MailContact",
                table: "Organismes");

            migrationBuilder.DropColumn(
                name: "MontantContrat",
                table: "Organismes");

            migrationBuilder.DropColumn(
                name: "NomContact",
                table: "Organismes");

            migrationBuilder.DropColumn(
                name: "NombreUtilisateurs",
                table: "Organismes");

            migrationBuilder.DropColumn(
                name: "TelContact",
                table: "Organismes");

            migrationBuilder.DropColumn(
                name: "VersionElise",
                table: "Organismes");

            migrationBuilder.AlterColumn<string>(
                name: "Ville",
                table: "Organismes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
