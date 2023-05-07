using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class additionalColumns2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.AddColumn<int>(
                name: "ResturantID",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Order_ResturantID",
                table: "Order",
                column: "ResturantID");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Resturant_ResturantID",
                table: "Order",
                column: "ResturantID",
                principalTable: "Resturant",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Resturant_ResturantID",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_ResturantID",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ResturantID",
                table: "Order");


        }
    }
}
