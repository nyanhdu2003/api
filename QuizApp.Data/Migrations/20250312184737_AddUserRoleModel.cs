using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRoleModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizQuestion_Questions_QuestionId",
                table: "QuizQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizQuestion_Quizzes_QuizId",
                table: "QuizQuestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuizQuestion",
                table: "QuizQuestion");

            migrationBuilder.RenameTable(
                name: "QuizQuestion",
                newName: "QuizQuestions");

            migrationBuilder.RenameIndex(
                name: "IX_QuizQuestion_QuestionId",
                table: "QuizQuestions",
                newName: "IX_QuizQuestions_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuizQuestions",
                table: "QuizQuestions",
                columns: new[] { "QuizId", "QuestionId" });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizQuestions_Questions_QuestionId",
                table: "QuizQuestions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizQuestions_Quizzes_QuizId",
                table: "QuizQuestions",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizQuestions_Questions_QuestionId",
                table: "QuizQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizQuestions_Quizzes_QuizId",
                table: "QuizQuestions");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuizQuestions",
                table: "QuizQuestions");

            migrationBuilder.RenameTable(
                name: "QuizQuestions",
                newName: "QuizQuestion");

            migrationBuilder.RenameIndex(
                name: "IX_QuizQuestions_QuestionId",
                table: "QuizQuestion",
                newName: "IX_QuizQuestion_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuizQuestion",
                table: "QuizQuestion",
                columns: new[] { "QuizId", "QuestionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_QuizQuestion_Questions_QuestionId",
                table: "QuizQuestion",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizQuestion_Quizzes_QuizId",
                table: "QuizQuestion",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
