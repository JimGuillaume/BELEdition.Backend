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
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_utilisateur = table.Column<int>(type: "int", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Mot_de_passe_hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Eid_identifiant_hash = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Est_actif = table.Column<bool>(type: "bit", nullable: false),
                    Est_suspendu = table.Column<bool>(type: "bit", nullable: false),
                    Tentatives_echouees = table.Column<int>(type: "int", nullable: false),
                    Verrouille_jusqua = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Date_inscription = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_suppression = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Eid_identifiant_hash",
                table: "Users",
                column: "Eid_identifiant_hash",
                unique: true,
                filter: "[Eid_identifiant_hash] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
