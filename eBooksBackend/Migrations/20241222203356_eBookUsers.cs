using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eBooksBackend.Migrations
{
    /// <inheritdoc />
    public partial class eBookUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "eBookUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EbookId = table.Column<int>(type: "int", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Format = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eBookUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_eBookUsers_eBook_EbookId",
                        column: x => x.EbookId,
                        principalTable: "eBook",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_eBookUsers_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_eBookUsers_EbookId",
                table: "eBookUsers",
                column: "EbookId");

            migrationBuilder.CreateIndex(
                name: "IX_eBookUsers_UserId",
                table: "eBookUsers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "eBookUsers");
        }
    }
}
