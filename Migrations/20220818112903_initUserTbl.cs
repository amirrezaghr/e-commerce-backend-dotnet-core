using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyEshop.Migrations
{
    public partial class initUserTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryToProducts",
                table: "CategoryToProducts");

            migrationBuilder.DropIndex(
                name: "IX_CategoryToProducts_ProductId",
                table: "CategoryToProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryToProducts",
                table: "CategoryToProducts",
                columns: new[] { "ProductId", "CategoryId" });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(maxLength: 300, nullable: false),
                    Password = table.Column<string>(maxLength: 50, nullable: false),
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    IsAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryToProducts_CategoryId",
                table: "CategoryToProducts",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryToProducts",
                table: "CategoryToProducts");

            migrationBuilder.DropIndex(
                name: "IX_CategoryToProducts_CategoryId",
                table: "CategoryToProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryToProducts",
                table: "CategoryToProducts",
                columns: new[] { "CategoryId", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryToProducts_ProductId",
                table: "CategoryToProducts",
                column: "ProductId");
        }
    }
}
