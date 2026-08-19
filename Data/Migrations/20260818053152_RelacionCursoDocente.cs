using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinal_GarroRojasRosa.Data.Migrations
{
    /// <inheritdoc />
    public partial class RelacionCursoDocente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdDocente",
                table: "Cursos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cursos_IdDocente",
                table: "Cursos",
                column: "IdDocente");

            migrationBuilder.AddForeignKey(
                name: "FK_Cursos_Docentes_IdDocente",
                table: "Cursos",
                column: "IdDocente",
                principalTable: "Docentes",
                principalColumn: "IdDocente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cursos_Docentes_IdDocente",
                table: "Cursos");

            migrationBuilder.DropIndex(
                name: "IX_Cursos_IdDocente",
                table: "Cursos");

            migrationBuilder.DropColumn(
                name: "IdDocente",
                table: "Cursos");
        }
    }
}
