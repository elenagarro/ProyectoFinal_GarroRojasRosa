using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinal_GarroRojasRosa.Data.Migrations
{
    /// <inheritdoc />
    public partial class MejorarModeloMatricula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdCarrera",
                table: "Matriculas",
                newName: "IdCurso");

            migrationBuilder.AddColumn<bool>(
                name: "Estado",
                table: "Matriculas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaMatricula",
                table: "Matriculas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Matriculas_IdCurso",
                table: "Matriculas",
                column: "IdCurso");

            migrationBuilder.CreateIndex(
                name: "IX_Matriculas_IdEstudiante",
                table: "Matriculas",
                column: "IdEstudiante");

            migrationBuilder.AddForeignKey(
                name: "FK_Matriculas_Cursos_IdCurso",
                table: "Matriculas",
                column: "IdCurso",
                principalTable: "Cursos",
                principalColumn: "IdCurso",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matriculas_Estudiantes_IdEstudiante",
                table: "Matriculas",
                column: "IdEstudiante",
                principalTable: "Estudiantes",
                principalColumn: "IdEstudiante",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matriculas_Cursos_IdCurso",
                table: "Matriculas");

            migrationBuilder.DropForeignKey(
                name: "FK_Matriculas_Estudiantes_IdEstudiante",
                table: "Matriculas");

            migrationBuilder.DropIndex(
                name: "IX_Matriculas_IdCurso",
                table: "Matriculas");

            migrationBuilder.DropIndex(
                name: "IX_Matriculas_IdEstudiante",
                table: "Matriculas");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Matriculas");

            migrationBuilder.DropColumn(
                name: "FechaMatricula",
                table: "Matriculas");

            migrationBuilder.RenameColumn(
                name: "IdCurso",
                table: "Matriculas",
                newName: "IdCarrera");
        }
    }
}
