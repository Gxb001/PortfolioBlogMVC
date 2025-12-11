using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PorfolioBlogMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConfigurationComplete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_AspNetUsers_AuteurId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Categories_CategorieId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Commentaires_Articles_ArticleId",
                table: "Commentaires");

            migrationBuilder.DropForeignKey(
                name: "FK_Commentaires_AspNetUsers_AuteurId",
                table: "Commentaires");

            migrationBuilder.DropForeignKey(
                name: "FK_ElementsPortfolio_AspNetUsers_CreateurId",
                table: "ElementsPortfolio");

            migrationBuilder.AddColumn<string>(
                name: "Couleur",
                table: "Tags",
                type: "nvarchar(7)",
                maxLength: 7,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PortfolioImages",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EstImagePrincipale",
                table: "PortfolioImages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "ElementsPortfolio",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Articles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PortfolioTag",
                columns: table => new
                {
                    ElementPortfolioId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioTag", x => new { x.ElementPortfolioId, x.TagId });
                    table.ForeignKey(
                        name: "FK_PortfolioTag_ElementsPortfolio_ElementPortfolioId",
                        column: x => x.ElementPortfolioId,
                        principalTable: "ElementsPortfolio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PortfolioTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Nom",
                table: "Tags",
                column: "Nom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ElementsPortfolio_ApplicationUserId",
                table: "ElementsPortfolio",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Nom",
                table: "Categories",
                column: "Nom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ApplicationUserId",
                table: "Articles",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioTag_TagId",
                table: "PortfolioTag",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_AspNetUsers_ApplicationUserId",
                table: "Articles",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_AspNetUsers_AuteurId",
                table: "Articles",
                column: "AuteurId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Categories_CategorieId",
                table: "Articles",
                column: "CategorieId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Commentaires_Articles_ArticleId",
                table: "Commentaires",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Commentaires_AspNetUsers_AuteurId",
                table: "Commentaires",
                column: "AuteurId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ElementsPortfolio_AspNetUsers_ApplicationUserId",
                table: "ElementsPortfolio",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ElementsPortfolio_AspNetUsers_CreateurId",
                table: "ElementsPortfolio",
                column: "CreateurId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_AspNetUsers_ApplicationUserId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Articles_AspNetUsers_AuteurId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Categories_CategorieId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Commentaires_Articles_ArticleId",
                table: "Commentaires");

            migrationBuilder.DropForeignKey(
                name: "FK_Commentaires_AspNetUsers_AuteurId",
                table: "Commentaires");

            migrationBuilder.DropForeignKey(
                name: "FK_ElementsPortfolio_AspNetUsers_ApplicationUserId",
                table: "ElementsPortfolio");

            migrationBuilder.DropForeignKey(
                name: "FK_ElementsPortfolio_AspNetUsers_CreateurId",
                table: "ElementsPortfolio");

            migrationBuilder.DropTable(
                name: "PortfolioTag");

            migrationBuilder.DropIndex(
                name: "IX_Tags_Nom",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_ElementsPortfolio_ApplicationUserId",
                table: "ElementsPortfolio");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Nom",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Articles_ApplicationUserId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Couleur",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "PortfolioImages");

            migrationBuilder.DropColumn(
                name: "EstImagePrincipale",
                table: "PortfolioImages");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "ElementsPortfolio");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Articles");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_AspNetUsers_AuteurId",
                table: "Articles",
                column: "AuteurId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Categories_CategorieId",
                table: "Articles",
                column: "CategorieId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Commentaires_Articles_ArticleId",
                table: "Commentaires",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Commentaires_AspNetUsers_AuteurId",
                table: "Commentaires",
                column: "AuteurId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ElementsPortfolio_AspNetUsers_CreateurId",
                table: "ElementsPortfolio",
                column: "CreateurId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
