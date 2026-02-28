using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_website.Migrations
{
    /// <inheritdoc />
    public partial class productcategoryupdatedmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_product_tbl_cat_id",
                table: "product_tbl",
                column: "cat_id");

            migrationBuilder.AddForeignKey(
                name: "FK_product_tbl_category_tbl_cat_id",
                table: "product_tbl",
                column: "cat_id",
                principalTable: "category_tbl",
                principalColumn: "category_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_product_tbl_category_tbl_cat_id",
                table: "product_tbl");

            migrationBuilder.DropIndex(
                name: "IX_product_tbl_cat_id",
                table: "product_tbl");
        }
    }
}
