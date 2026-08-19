using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinal_GarroRojasRosa.Data.Migrations
{
    /// <inheritdoc />
    public partial class MejorarModeloDocente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Correo",
                table: "Docentes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Especialidad",
                table: "Docentes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Estado",
                table: "Docentes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Correo",
                table: "Docentes");

            migrationBuilder.DropColumn(
                name: "Especialidad",
                table: "Docentes");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Docentes");
        }
    }
}
