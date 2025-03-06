using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWishListTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
            name: "WishLists",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                StudentId = table.Column<string>(nullable: false),
                WishedCourseId = table.Column<int>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_WishLists", x => x.Id);
                table.ForeignKey(
                    name: "FK_WishLists_AspNetUsers_StudentId",
                    column: x => x.StudentId,
                    principalTable: "AspNetUsers",  // Reference to Identity table
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_WishLists_Courses_WishedCourseId",
                    column: x => x.WishedCourseId,
                    principalTable: "Courses",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

            migrationBuilder.CreateIndex(
                name: "IX_WishLists_StudentId",
                table: "WishLists",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_WishLists_WishedCourseId",
                table: "WishLists",
                column: "WishedCourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "WishLists");

        }
    }
}
