using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BureauOrdreDigital.Api.Migrations
{
    /// <inheritdoc />
    public partial class AjoutCourrier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceDestinataire",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomService = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NomOrganisme = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Actif = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceDestinataire", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatutCourrier",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Libelle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Ordre = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatutCourrier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeCourrier",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Libelle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeCourrier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courriers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumReference = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    UtilisateurId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeCourrierId = table.Column<int>(type: "int", nullable: false),
                    ServiceDestinataireId = table.Column<int>(type: "int", nullable: false),
                    StatutId = table.Column<int>(type: "int", nullable: false),
                    Objet = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateSoumission = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateDerniereMaj = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courriers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courriers_ServiceDestinataire_ServiceDestinataireId",
                        column: x => x.ServiceDestinataireId,
                        principalTable: "ServiceDestinataire",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Courriers_StatutCourrier_StatutId",
                        column: x => x.StatutId,
                        principalTable: "StatutCourrier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Courriers_TypeCourrier_TypeCourrierId",
                        column: x => x.TypeCourrierId,
                        principalTable: "TypeCourrier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Courriers_Utilisateurs_UtilisateurId",
                        column: x => x.UtilisateurId,
                        principalTable: "Utilisateurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HistoriqueActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatutId = table.Column<int>(type: "int", nullable: false),
                    DateAction = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoriqueActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoriqueActions_Courriers_CourrierId",
                        column: x => x.CourrierId,
                        principalTable: "Courriers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistoriqueActions_StatutCourrier_StatutId",
                        column: x => x.StatutId,
                        principalTable: "StatutCourrier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PiecesJointes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomFichier = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UrlStockage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TailleOctets = table.Column<long>(type: "bigint", nullable: false),
                    Format = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DateUpload = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PiecesJointes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PiecesJointes_Courriers_CourrierId",
                        column: x => x.CourrierId,
                        principalTable: "Courriers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courriers_NumReference",
                table: "Courriers",
                column: "NumReference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courriers_ServiceDestinataireId",
                table: "Courriers",
                column: "ServiceDestinataireId");

            migrationBuilder.CreateIndex(
                name: "IX_Courriers_StatutId",
                table: "Courriers",
                column: "StatutId");

            migrationBuilder.CreateIndex(
                name: "IX_Courriers_TypeCourrierId",
                table: "Courriers",
                column: "TypeCourrierId");

            migrationBuilder.CreateIndex(
                name: "IX_Courriers_UtilisateurId",
                table: "Courriers",
                column: "UtilisateurId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoriqueActions_CourrierId",
                table: "HistoriqueActions",
                column: "CourrierId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoriqueActions_StatutId",
                table: "HistoriqueActions",
                column: "StatutId");

            migrationBuilder.CreateIndex(
                name: "IX_PiecesJointes_CourrierId",
                table: "PiecesJointes",
                column: "CourrierId");

            migrationBuilder.CreateIndex(
                name: "IX_StatutCourrier_Code",
                table: "StatutCourrier",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoriqueActions");

            migrationBuilder.DropTable(
                name: "PiecesJointes");

            migrationBuilder.DropTable(
                name: "Courriers");

            migrationBuilder.DropTable(
                name: "ServiceDestinataire");

            migrationBuilder.DropTable(
                name: "StatutCourrier");

            migrationBuilder.DropTable(
                name: "TypeCourrier");
        }
    }
}
