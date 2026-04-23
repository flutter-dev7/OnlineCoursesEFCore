using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineCourses.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStudenttoReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Reviews_StudentId",
                table: "Reviews",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_StudentId",
                table: "Reviews",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_StudentId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_StudentId",
                table: "Reviews");
        }
    }
}
