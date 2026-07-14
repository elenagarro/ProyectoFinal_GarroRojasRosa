using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinal_GarroRojasRosa.Data.Migrations
{
    /// <inheritdoc />
    public partial class RelacionEstudianteCarrera : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdCarrera",
                table: "Estudiantes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Estudiantes_IdCarrera",
                table: "Estudiantes",
                column: "IdCarrera");

            migrationBuilder.AddForeignKey(
                name: "FK_Estudiantes_Carreras_IdCarrera",
                table: "Estudiantes",
                column: "IdCarrera",
                principalTable: "Carreras",
                principalColumn: "IdCarrera",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estudiantes_Carreras_IdCarrera",
                table: "Estudiantes");

            migrationBuilder.DropIndex(
                name: "IX_Estudiantes_IdCarrera",
                table: "Estudiantes");

            migrationBuilder.DropColumn(
                name: "IdCarrera",
                table: "Estudiantes");
        }
    }
}
