using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinal_GarroRojasRosa.Data.Migrations
{
    /// <inheritdoc />
    public partial class RelacionCursoCarrera : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdCarrera",
                table: "Cursos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Asignar una carrera existente a los cursos que ya estaban registrados.
            // Se toma la primera carrera disponible.
            migrationBuilder.Sql(@"
        UPDATE Cursos
        SET IdCarrera = (
            SELECT TOP 1 IdCarrera
            FROM Carreras
            ORDER BY IdCarrera
        )
        WHERE IdCarrera = 0;
    ");

            migrationBuilder.CreateIndex(
                name: "IX_Cursos_IdCarrera",
                table: "Cursos",
                column: "IdCarrera");

            migrationBuilder.AddForeignKey(
                name: "FK_Cursos_Carreras_IdCarrera",
                table: "Cursos",
                column: "IdCarrera",
                principalTable: "Carreras",
                principalColumn: "IdCarrera",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cursos_Carreras_IdCarrera",
                table: "Cursos");

            migrationBuilder.DropIndex(
                name: "IX_Cursos_IdCarrera",
                table: "Cursos");

            migrationBuilder.DropColumn(
                name: "IdCarrera",
                table: "Cursos");
        }
    }
}