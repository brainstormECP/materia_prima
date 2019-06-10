using Microsoft.EntityFrameworkCore.Migrations;

namespace MateriasPrimasApp.Migrations
{
    public partial class algunossubproductos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "250e7fbb-ff6d-4093-8b05-fa7c7b9bf554");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "c0d7adbb-312a-4b28-8372-0b21b9443f1d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "4ce94ff9-f09d-4cd2-b270-7df6b4cca3ff");

            migrationBuilder.InsertData(
                table: "Producto",
                columns: new[] { "Id", "CategoriaId", "Codigo", "Descripcion", "Discriminator", "Nombre", "PrecioCompraMlc", "PrecioCompraMn", "PrecioVentaMlc", "PrecioVentaMn", "TipoId", "UnidadId", "ProductoOrigenId" },
                values: new object[] { 5, 2, "f679", "Aluminio Perfilado", "Derivado", "Aluminio Perfilado", 20m, 500m, 48m, 1200m, 1, 1, 3 });

            migrationBuilder.InsertData(
                table: "Producto",
                columns: new[] { "Id", "CategoriaId", "Codigo", "Descripcion", "Discriminator", "Nombre", "PrecioCompraMlc", "PrecioCompraMn", "PrecioVentaMlc", "PrecioVentaMn", "TipoId", "UnidadId", "ProductoOrigenId" },
                values: new object[] { 6, 2, "f680", "Aluminio Fundido", "Derivado", "Aluminio Fundido", 20m, 500m, 48m, 1200m, 1, 1, 3 });

            migrationBuilder.InsertData(
                table: "Producto",
                columns: new[] { "Id", "CategoriaId", "Codigo", "Descripcion", "Discriminator", "Nombre", "PrecioCompraMlc", "PrecioCompraMn", "PrecioVentaMlc", "PrecioVentaMn", "TipoId", "UnidadId", "ProductoOrigenId" },
                values: new object[] { 7, 1, "f681", "Hierro Fundido", "Derivado", "Hierro Fundido", 4m, 100m, 10m, 250m, 1, 1, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "c61e862a-a823-4868-920b-3dc43a5e0d4e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "b262579f-8f41-4995-9350-c06fdaba89b2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "b55bc5f3-e06c-4b3f-b97e-004e5c4147b2");
        }
    }
}
