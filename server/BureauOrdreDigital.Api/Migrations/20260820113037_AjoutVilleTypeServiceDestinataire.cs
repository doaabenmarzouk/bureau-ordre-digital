using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BureauOrdreDigital.Api.Migrations
{
    /// <inheritdoc />
    public partial class AjoutVilleTypeServiceDestinataire : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TypeOrganisme",
                table: "ServiceDestinataire",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ville",
                table: "ServiceDestinataire",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeOrganisme",
                table: "ServiceDestinataire");

            migrationBuilder.DropColumn(
                name: "Ville",
                table: "ServiceDestinataire");
        }
    }
}
