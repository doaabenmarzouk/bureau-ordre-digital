using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BureauOrdreDigital.Api.Migrations
{
    /// <inheritdoc />
    public partial class OrganismeDestinataire : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courriers_ServiceDestinataire_ServiceDestinataireId",
                table: "Courriers");

            migrationBuilder.DropTable(
                name: "ServiceDestinataire");

            migrationBuilder.RenameColumn(
                name: "ServiceDestinataireId",
                table: "Courriers",
                newName: "OrganismeId");

            migrationBuilder.RenameIndex(
                name: "IX_Courriers_ServiceDestinataireId",
                table: "Courriers",
                newName: "IX_Courriers_OrganismeId");

            migrationBuilder.CreateTable(
                name: "Organismes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NomAbrege = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Pays = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ville = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Scope = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Mode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Marche = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Partenaire = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Contrat = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateAnniversaire = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Actif = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organismes", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Courriers_Organismes_OrganismeId",
                table: "Courriers",
                column: "OrganismeId",
                principalTable: "Organismes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courriers_Organismes_OrganismeId",
                table: "Courriers");

            migrationBuilder.DropTable(
                name: "Organismes");

            migrationBuilder.RenameColumn(
                name: "OrganismeId",
                table: "Courriers",
                newName: "ServiceDestinataireId");

            migrationBuilder.RenameIndex(
                name: "IX_Courriers_OrganismeId",
                table: "Courriers",
                newName: "IX_Courriers_ServiceDestinataireId");

            migrationBuilder.CreateTable(
                name: "ServiceDestinataire",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Actif = table.Column<bool>(type: "bit", nullable: false),
                    NomOrganisme = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NomService = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TypeOrganisme = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ville = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceDestinataire", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Courriers_ServiceDestinataire_ServiceDestinataireId",
                table: "Courriers",
                column: "ServiceDestinataireId",
                principalTable: "ServiceDestinataire",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
