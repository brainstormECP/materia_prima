using Microsoft.EntityFrameworkCore.Migrations;

namespace MateriasPrimasApp.Migrations
{
    public partial class Ueb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "67875f83-176f-4153-b221-b28446d30c94");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "951c9fec-976a-4575-bad9-f21e31dc67e5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "07ab76d9-6436-4f84-aa45-a7cd7a7b5c2e");

            migrationBuilder.UpdateData(
                table: "Submayor",
                keyColumns: new[] { "AlmacenId", "ProductoId" },
                keyValues: new object[] { 1, 1 },
                column: "Cantidad",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Submayor",
                keyColumns: new[] { "AlmacenId", "ProductoId" },
                keyValues: new object[] { 1, 2 },
                column: "Cantidad",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Submayor",
                keyColumns: new[] { "AlmacenId", "ProductoId" },
                keyValues: new object[] { 2, 1 },
                column: "Cantidad",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Submayor",
                keyColumns: new[] { "AlmacenId", "ProductoId" },
                keyValues: new object[] { 2, 2 },
                column: "Cantidad",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Nombre",
                value: "UEB Matanzar");

            migrationBuilder.UpdateData(
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 2,
                column: "Nombre",
                value: "UEB Colón");

            migrationBuilder.InsertData(
                table: "UnidadesOrganizativas",
                columns: new[] { "Id", "Discriminator", "Nombre", "Telefono", "UebId" },
                values: new object[] { 14, "CasaCompra", "Casa de compra Calimete", null, 2 });

            migrationBuilder.InsertData(
                table: "UnidadesOrganizativas",
                columns: new[] { "Id", "Discriminator", "Nombre", "Telefono", "UebId" },
                values: new object[] { 15, "CasaCompra", "Casa de compra Los Arabos", null, 2 });

            migrationBuilder.InsertData(
                table: "UnidadesOrganizativas",
                columns: new[] { "Id", "Discriminator", "Nombre", "Telefono", "UebId" },
                values: new object[] { 16, "CasaCompra", "Casa de compra Martí", null, 2 });

            migrationBuilder.InsertData(
                table: "UnidadesOrganizativas",
                columns: new[] { "Id", "Discriminator", "Nombre", "Telefono", "UebId" },
                values: new object[] { 12, "CasaCompra", "Casa de compra Colón", null, 2 });

            migrationBuilder.InsertData(
                table: "UnidadesOrganizativas",
                columns: new[] { "Id", "Discriminator", "Nombre", "Telefono", "UebId" },
                values: new object[] { 9, "CasaCompra", "Casa de compra Unión de Reyes", null, 1 });

            migrationBuilder.InsertData(
                table: "UnidadesOrganizativas",
                columns: new[] { "Id", "Discriminator", "Nombre", "Telefono", "UebId" },
                values: new object[] { 8, "CasaCompra", "Casa de Limonar", null, 1 });

            migrationBuilder.InsertData(
                table: "UnidadesOrganizativas",
                columns: new[] { "Id", "Discriminator", "Nombre", "Telefono", "Municipio" },
                values: new object[] { 3, "UEB", "UEB Jovellanos", "312008", "Jovellanos" });

            migrationBuilder.InsertData(
                table: "UnidadesOrganizativas",
                columns: new[] { "Id", "Discriminator", "Nombre", "Telefono", "Municipio" },
                values: new object[] { 4, "UEB", "UEB Jagüey Grande", "572487", "Jagüey Grande" });

            migrationBuilder.InsertData(
                table: "UnidadesOrganizativas",
                columns: new[] { "Id", "Discriminator", "Nombre", "Telefono", "Municipio" },
                values: new object[] { 5, "UEB", "UEB Colón", "371304", "Cárdenas" });

            migrationBuilder.InsertData(
                table: "UnidadesOrganizativas",
                columns: new[] { "Id", "Discriminator", "Nombre", "Telefono", "UebId" },
                values: new object[] { 7, "CasaCompra", "Casa de compra2 Matanzas", null, 1 });

            migrationBuilder.InsertData(
                table: "UnidadesOrganizativas",
                columns: new[] { "Id", "Discriminator", "Nombre", "Telefono", "UebId" },
                values: new object[] { 13, "CasaCompra", "Casa de compra Perico", null, 2 });

            migrationBuilder.InsertData(
                table: "UnidadesOrganizativas",
                columns: new[] { "Id", "Discriminator", "Nombre", "Telefono", "UebId" },
                values: new object[] { 6, "CasaCompra", "Casa de compra1 Matanzar", null, 1 });

            migrationBuilder.UpdateData(
                table: "Submayor",
                keyColumns: new[] { "AlmacenId", "ProductoId" },
                keyValues: new object[] { 3, 1 },
                column: "Cantidad",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Submayor",
                keyColumns: new[] { "AlmacenId", "ProductoId" },
                keyValues: new object[] { 3, 2 },
                column: "Cantidad",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Submayor",
                keyColumns: new[] { "AlmacenId", "ProductoId" },
                keyValues: new object[] { 4, 1 },
                column: "Cantidad",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Submayor",
                keyColumns: new[] { "AlmacenId", "ProductoId" },
                keyValues: new object[] { 4, 2 },
                column: "Cantidad",
                value: 0m);

            migrationBuilder.InsertData(
                table: "UnidadesOrganizativas",
                columns: new[] { "Id", "Discriminator", "Nombre", "Telefono", "UebId" },
                values: new object[] { 10, "CasaCompra", "Casa de compra Jovellanos", null, 3 });

            migrationBuilder.InsertData(
                table: "UnidadesOrganizativas",
                columns: new[] { "Id", "Discriminator", "Nombre", "Telefono", "UebId" },
                values: new object[] { 11, "CasaCompra", "Casa de compra Betancourt", null, 3 });

            migrationBuilder.InsertData(
                table: "UnidadesOrganizativas",
                columns: new[] { "Id", "Discriminator", "Nombre", "Telefono", "UebId" },
                values: new object[] { 17, "CasaCompra", "Casa de compra Jagüey", null, 4 });

            migrationBuilder.InsertData(
                table: "UnidadesOrganizativas",
                columns: new[] { "Id", "Discriminator", "Nombre", "Telefono", "UebId" },
                values: new object[] { 18, "CasaCompra", "Casa de compra Ciénaga de Zapata", null, 4 });

            migrationBuilder.InsertData(
                table: "UnidadesOrganizativas",
                columns: new[] { "Id", "Discriminator", "Nombre", "Telefono", "UebId" },
                values: new object[] { 19, "CasaCompra", "Casa de compra1 Cárdenas", null, 5 });

            migrationBuilder.InsertData(
                table: "UnidadesOrganizativas",
                columns: new[] { "Id", "Discriminator", "Nombre", "Telefono", "UebId" },
                values: new object[] { 20, "CasaCompra", "Casa de compra2 Cárdenas", null, 5 });

            migrationBuilder.InsertData(
                table: "UnidadesOrganizativas",
                columns: new[] { "Id", "Discriminator", "Nombre", "Telefono", "UebId" },
                values: new object[] { 21, "CasaCompra", "Casa de compra Varadero", null, 5 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 5);

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

            migrationBuilder.UpdateData(
                table: "Submayor",
                keyColumns: new[] { "AlmacenId", "ProductoId" },
                keyValues: new object[] { 1, 1 },
                column: "Cantidad",
                value: 100m);

            migrationBuilder.UpdateData(
                table: "Submayor",
                keyColumns: new[] { "AlmacenId", "ProductoId" },
                keyValues: new object[] { 1, 2 },
                column: "Cantidad",
                value: 1000m);

            migrationBuilder.UpdateData(
                table: "Submayor",
                keyColumns: new[] { "AlmacenId", "ProductoId" },
                keyValues: new object[] { 2, 1 },
                column: "Cantidad",
                value: 100m);

            migrationBuilder.UpdateData(
                table: "Submayor",
                keyColumns: new[] { "AlmacenId", "ProductoId" },
                keyValues: new object[] { 2, 2 },
                column: "Cantidad",
                value: 750m);

            migrationBuilder.UpdateData(
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Nombre",
                value: "UEBMtz");

            migrationBuilder.UpdateData(
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 2,
                column: "Nombre",
                value: "UEBCol");

            migrationBuilder.InsertData(
                table: "UnidadesOrganizativas",
                columns: new[] { "Id", "Discriminator", "Nombre", "Telefono", "UebId" },
                values: new object[] { 3, "CasaCompra", "Casa de compras Versalles", null, 1 });

            migrationBuilder.InsertData(
                table: "UnidadesOrganizativas",
                columns: new[] { "Id", "Discriminator", "Nombre", "Telefono", "UebId" },
                values: new object[] { 4, "CasaCompra", "Casa de compras Playa", null, 1 });

            migrationBuilder.UpdateData(
                table: "Submayor",
                keyColumns: new[] { "AlmacenId", "ProductoId" },
                keyValues: new object[] { 3, 1 },
                column: "Cantidad",
                value: 20m);

            migrationBuilder.UpdateData(
                table: "Submayor",
                keyColumns: new[] { "AlmacenId", "ProductoId" },
                keyValues: new object[] { 3, 2 },
                column: "Cantidad",
                value: 400m);

            migrationBuilder.UpdateData(
                table: "Submayor",
                keyColumns: new[] { "AlmacenId", "ProductoId" },
                keyValues: new object[] { 4, 1 },
                column: "Cantidad",
                value: 30m);

            migrationBuilder.UpdateData(
                table: "Submayor",
                keyColumns: new[] { "AlmacenId", "ProductoId" },
                keyValues: new object[] { 4, 2 },
                column: "Cantidad",
                value: 350m);
        }
    }
}
