using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinal_GarroRojasRosa.Data.Migrations
{
    /// <inheritdoc />
    public partial class VincularUsuarioEstudiante : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Estudiantes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Estudiantes_ApplicationUserId",
                table: "Estudiantes",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estudiantes_AspNetUsers_ApplicationUserId",
                table: "Estudiantes",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estudiantes_AspNetUsers_ApplicationUserId",
                table: "Estudiantes");

            migrationBuilder.DropIndex(
                name: "IX_Estudiantes_ApplicationUserId",
                table: "Estudiantes");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Estudiantes");
        }
    }
}
