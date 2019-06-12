using Microsoft.EntityFrameworkCore.Migrations;

namespace MateriasPrimasApp.Migrations
{
    public partial class CC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "bdc3d4e3-e636-420f-bdbe-877d242e414d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "a53d2627-7b29-4acc-aa6f-5c59a2f30bf2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "23102dc1-2f87-4a40-93e9-a33797e47241");

            migrationBuilder.UpdateData(
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 6,
                column: "Nombre",
                value: "Casa de compra1 Matanzas");

            migrationBuilder.UpdateData(
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Nombre",
                value: "UEB Matanzas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 6,
                column: "Nombre",
                value: "Casa de compra1 Matanzar");

            migrationBuilder.UpdateData(
                table: "UnidadesOrganizativas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Nombre",
                value: "UEB Matanzar");
        }
    }
}
