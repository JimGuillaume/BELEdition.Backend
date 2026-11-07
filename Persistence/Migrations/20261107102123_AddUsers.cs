using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Utilisateur",
                columns: table => new
                {
                    id_utilisateur = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    prenom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    mot_de_passe_hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    eid_identifiant_hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    est_actif = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    est_suspendu = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    tentatives_echouees = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    verrouille_jusqua = table.Column<DateTime>(type: "datetime2", nullable: true),
                    date_inscription = table.Column<DateTime>(type: "datetime2", nullable: false),
                    date_suppression = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilisateur", x => x.id_utilisateur);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateur_eid_identifiant_hash",
                table: "Utilisateur",
                column: "eid_identifiant_hash",
                unique: true,
                filter: "[eid_identifiant_hash] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateur_email",
                table: "Utilisateur",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Utilisateur");
        }
    }
}
