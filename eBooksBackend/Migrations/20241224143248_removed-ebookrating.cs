using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eBooksBackend.Migrations
{
    /// <inheritdoc />
    public partial class removedebookrating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "eBookRatings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "eBookRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    eBookId = table.Column<int>(type: "int", nullable: false),
                    userID = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<float>(type: "real", nullable: false),
                    Review = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eBookRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_eBookRatings_eBook_eBookId",
                        column: x => x.eBookId,
                        principalTable: "eBook",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_eBookRatings_users_userID",
                        column: x => x.userID,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_eBookRatings_eBookId",
                table: "eBookRatings",
                column: "eBookId");

            migrationBuilder.CreateIndex(
                name: "IX_eBookRatings_userID",
                table: "eBookRatings",
                column: "userID");
        }
    }
}
