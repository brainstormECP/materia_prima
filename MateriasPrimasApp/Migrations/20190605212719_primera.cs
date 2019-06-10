using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MateriasPrimasApp.Migrations
{
    public partial class primera : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(nullable: false),
                    Descripcion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Codigo = table.Column<string>(nullable: false),
                    Nombre = table.Column<string>(nullable: false),
                    Organismo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoDeProducto",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(nullable: false),
                    Descripcion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoDeProducto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Unidad = table.Column<string>(nullable: false),
                    Descripcion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UM", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnidadesOrganizativas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(nullable: false),
                    Telefono = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    UebId = table.Column<int>(nullable: true),
                    Municipio = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadesOrganizativas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnidadesOrganizativas_UnidadesOrganizativas_UebId",
                        column: x => x.UebId,
                        principalTable: "UnidadesOrganizativas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Producto",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Codigo = table.Column<string>(nullable: false),
                    Nombre = table.Column<string>(nullable: false),
                    Descripcion = table.Column<string>(nullable: true),
                    UnidadId = table.Column<int>(nullable: false),
                    CategoriaId = table.Column<int>(nullable: false),
                    TipoId = table.Column<int>(nullable: false),
                    PrecioVentaMn = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PrecioVentaMlc = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PrecioCompraMn = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PrecioCompraMlc = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    ProductoOrigenId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Producto_Producto_ProductoOrigenId",
                        column: x => x.ProductoOrigenId,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Producto_Categoria_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Producto_TipoDeProducto_TipoId",
                        column: x => x.TipoId,
                        principalTable: "TipoDeProducto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Producto_UM_UnidadId",
                        column: x => x.UnidadId,
                        principalTable: "UM",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    UnidadOrganizativaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_UnidadesOrganizativas_UnidadOrganizativaId",
                        column: x => x.UnidadOrganizativaId,
                        principalTable: "UnidadesOrganizativas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Entrada",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Fecha = table.Column<DateTime>(nullable: false),
                    ClienteId = table.Column<int>(nullable: false),
                    UnidadOrganizativaId = table.Column<int>(nullable: false),
                    Confirmada = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entrada", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entrada_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entrada_UnidadesOrganizativas_UnidadOrganizativaId",
                        column: x => x.UnidadOrganizativaId,
                        principalTable: "UnidadesOrganizativas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transferencias",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Fecha = table.Column<DateTime>(nullable: false),
                    OrigenId = table.Column<int>(nullable: false),
                    DestinoId = table.Column<int>(nullable: false),
                    Confirmada = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transferencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transferencias_UnidadesOrganizativas_DestinoId",
                        column: x => x.DestinoId,
                        principalTable: "UnidadesOrganizativas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transferencias_UnidadesOrganizativas_OrigenId",
                        column: x => x.OrigenId,
                        principalTable: "UnidadesOrganizativas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Venta",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Fecha = table.Column<DateTime>(nullable: false),
                    ClienteId = table.Column<int>(nullable: false),
                    UnidadOrganizativaId = table.Column<int>(nullable: false),
                    Confirmada = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Venta_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Venta_UnidadesOrganizativas_UnidadOrganizativaId",
                        column: x => x.UnidadOrganizativaId,
                        principalTable: "UnidadesOrganizativas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Procesamientos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Fecha = table.Column<DateTime>(nullable: false),
                    UnidadOrganizativaId = table.Column<int>(nullable: false),
                    ProductoId = table.Column<int>(nullable: false),
                    Cantidad = table.Column<decimal>(nullable: false),
                    Confirmado = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Procesamientos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Procesamientos_Producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Procesamientos_UnidadesOrganizativas_UnidadOrganizativaId",
                        column: x => x.UnidadOrganizativaId,
                        principalTable: "UnidadesOrganizativas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Submayor",
                columns: table => new
                {
                    AlmacenId = table.Column<int>(nullable: false),
                    ProductoId = table.Column<int>(nullable: false),
                    UnidadId = table.Column<int>(nullable: false),
                    Cantidad = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submayor", x => new { x.AlmacenId, x.ProductoId });
                    table.ForeignKey(
                        name: "FK_Submayor_UnidadesOrganizativas_AlmacenId",
                        column: x => x.AlmacenId,
                        principalTable: "UnidadesOrganizativas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Submayor_Producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Submayor_UM_UnidadId",
                        column: x => x.UnidadId,
                        principalTable: "UM",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetallesDeEntradas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EntradaId = table.Column<int>(nullable: false),
                    ProductoId = table.Column<int>(nullable: false),
                    Cantidad = table.Column<decimal>(nullable: false),
                    PrecioMn = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PrecioMlc = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesDeEntradas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesDeEntradas_Entrada_EntradaId",
                        column: x => x.EntradaId,
                        principalTable: "Entrada",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesDeEntradas_Producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetallesDeTransferencia",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TransferenciaId = table.Column<int>(nullable: false),
                    ProductoId = table.Column<int>(nullable: false),
                    Cantidad = table.Column<decimal>(nullable: false),
                    PrecioMn = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PrecioMlc = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesDeTransferencia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesDeTransferencia_Producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesDeTransferencia_Transferencias_TransferenciaId",
                        column: x => x.TransferenciaId,
                        principalTable: "Transferencias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetallesDeVenta",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VentaId = table.Column<int>(nullable: false),
                    ProductoId = table.Column<int>(nullable: false),
                    Cantidad = table.Column<decimal>(nullable: false),
                    PrecioVentaMn = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PrecioVentaMlc = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesDeVenta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesDeVenta_Producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesDeVenta_Venta_VentaId",
                        column: x => x.VentaId,
                        principalTable: "Venta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetallesDeProcesamiento",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProcesamientoId = table.Column<int>(nullable: false),
                    DerivadoId = table.Column<int>(nullable: false),
                    Cantidad = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesDeProcesamiento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesDeProcesamiento_Producto_DerivadoId",
                        column: x => x.DerivadoId,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesDeProcesamiento_Procesamientos_ProcesamientoId",
                        column: x => x.ProcesamientoId,
                        principalTable: "Procesamientos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1", "beb53e6d-3e56-4ec5-915e-5c3b8ca4b811", "Administrador", "ADMINISTRADOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2", "94fca382-a0e5-4def-8508-642de92fb248", "Comercial", "COMERCIAL" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3", "5fe77294-44aa-4e3f-9408-43cde96548e6", "Consultor", "CONSULTOR" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Active", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UnidadOrganizativaId", "UserName" },
                values: new object[] { "f42559a2-2776-4e9b-9ba1-268597eff72b", 0, true, "36fd2616-8e8a-4cc6-8a5a-52d963207836", "admin@materiaprima.cu", false, false, null, "ADMIN@MATERIAPRIMA.CU", "ADMIN", "AQAAAAEAACcQAAAAEP4OedI6m26WUn/2C4AcBkzdT6SnL/6E+xakQ/9mGAkqqp3t9PwyIR6l9obLouKIVg==", null, false, "43VMKYQKNTENYZVJNU2TII26X23H5PGV", false, null, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Active", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UnidadOrganizativaId", "UserName" },
                values: new object[] { "4396dcc4-83c3-4e66-9416-c7df16be8b4a", 0, true, "22331850-3299-4df8-b760-4f58cf646be0", "juan@mp.cu", false, false, null, "JUAN@MP.CU", "JUAN", "AQAAAAEAACcQAAAAEOXJ8QHXja0i7s0kzcxlgJeT8xXS69ir2aCIkkIRoWjXP+GMHbeQsL/hAnMnfHziPg==", null, false, "CLPB5NJX34OWXJDQZSTE3NUCQKSHDTI2", false, null, "Juan" });

            migrationBuilder.InsertData(
                table: "Categoria",
                columns: new[] { "Id", "Descripcion", "Nombre" },
                values: new object[] { 1, "Metales ferrosos tales como Hierro y Acero", "Ferroso" });

            migrationBuilder.InsertData(
                table: "Categoria",
                columns: new[] { "Id", "Descripcion", "Nombre" },
                values: new object[] { 2, "Metales no ferrosos tales como Aluminio y Cobre", "No Ferroso" });

            migrationBuilder.InsertData(
                table: "Categoria",
                columns: new[] { "Id", "Descripcion", "Nombre" },
                values: new object[] { 3, "Envases de cristal como Botellas de cerveza y otros productos reciclables como el cartón", "Envases y varios" });

            migrationBuilder.InsertData(
                table: "Cliente",
                columns: new[] { "Id", "Codigo", "Nombre", "Organismo" },
                values: new object[] { 1, "C001", "CTEAG", "UNE" });

            migrationBuilder.InsertData(
                table: "TipoDeProducto",
                columns: new[] { "Id", "Descripcion", "Nombre" },
                values: new object[] { 1, "Metales", "Metálico" });

            migrationBuilder.InsertData(
                table: "TipoDeProducto",
                columns: new[] { "Id", "Descripcion", "Nombre" },
                values: new object[] { 2, "Otros Materiales no metálicos", "No Metálico" });

            migrationBuilder.InsertData(
                table: "UM",
                columns: new[] { "Id", "Descripcion", "Unidad" },
                values: new object[] { 1, "Tonelada", "Ton" });

            migrationBuilder.InsertData(
                table: "UM",
                columns: new[] { "Id", "Descripcion", "Unidad" },
                values: new object[] { 2, "KiloGramo", "Kg" });

            migrationBuilder.InsertData(
                table: "UM",
                columns: new[] { "Id", "Descripcion", "Unidad" },
                values: new object[] { 3, "Unidad", "U" });

            migrationBuilder.InsertData(
                table: "UnidadesOrganizativas",
                columns: new[] { "Id", "Discriminator", "Nombre", "Telefono", "Municipio" },
                values: new object[] { 1, "UEB", "UEBMtz", "262100", "Matanzas" });

            migrationBuilder.InsertData(
                table: "UnidadesOrganizativas",
                columns: new[] { "Id", "Discriminator", "Nombre", "Telefono", "Municipio" },
                values: new object[] { 2, "UEB", "UEBCol", "371304", "Colón" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "f42559a2-2776-4e9b-9ba1-268597eff72b", "1" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "4396dcc4-83c3-4e66-9416-c7df16be8b4a", "3" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Active", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UnidadOrganizativaId", "UserName" },
                values: new object[] { "e4acfaab-e3c9-42ef-9e21-1902da5374af", 0, true, "e3730b4c-284a-48da-ace3-b5f31f5671df", "user1@mp.cu", false, false, null, "USER1@MP.CU", "USER1", "AQAAAAEAACcQAAAAECmA0XlVxV7cpw5UFHBIIDAKwZ9RSjLf0g5QOiC9UwYIi8cn0Lw+2QqwOsDdtWkEyw==", null, false, "4AVYZFGP54QSAT3RJN4KI5R327NN4ID2", false, 1, "user1" });

            migrationBuilder.InsertData(
                table: "Producto",
                columns: new[] { "Id", "CategoriaId", "Codigo", "Descripcion", "Discriminator", "Nombre", "PrecioCompraMlc", "PrecioCompraMn", "PrecioVentaMlc", "PrecioVentaMn", "TipoId", "UnidadId" },
                values: new object[] { 1, 1, "1abcc345", "Hierro Fundido", "Producto", "Hierro", 4m, 100m, 6m, 150m, 1, 1 });

            migrationBuilder.InsertData(
                table: "Producto",
                columns: new[] { "Id", "CategoriaId", "Codigo", "Descripcion", "Discriminator", "Nombre", "PrecioCompraMlc", "PrecioCompraMn", "PrecioVentaMlc", "PrecioVentaMn", "TipoId", "UnidadId" },
                values: new object[] { 3, 2, "aldef678", "Aluminio", "Producto", "Aluminio", 20m, 500m, 32m, 800m, 1, 1 });

            migrationBuilder.InsertData(
                table: "Producto",
                columns: new[] { "Id", "CategoriaId", "Codigo", "Descripcion", "Discriminator", "Nombre", "PrecioCompraMlc", "PrecioCompraMn", "PrecioVentaMlc", "PrecioVentaMn", "TipoId", "UnidadId" },
                values: new object[] { 2, 3, "2def678", "Botella de Ron", "Producto", "Botella de Ron", 0.05m, 1m, 0.10m, 2m, 2, 3 });

            migrationBuilder.InsertData(
                table: "UnidadesOrganizativas",
                columns: new[] { "Id", "Discriminator", "Nombre", "Telefono", "UebId" },
                values: new object[] { 3, "CasaCompra", "Casa de compras Versalles", null, 1 });

            migrationBuilder.InsertData(
                table: "UnidadesOrganizativas",
                columns: new[] { "Id", "Discriminator", "Nombre", "Telefono", "UebId" },
                values: new object[] { 4, "CasaCompra", "Casa de compras Playa", null, 1 });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "e4acfaab-e3c9-42ef-9e21-1902da5374af", "2" });

            migrationBuilder.InsertData(
                table: "Producto",
                columns: new[] { "Id", "CategoriaId", "Codigo", "Descripcion", "Discriminator", "Nombre", "PrecioCompraMlc", "PrecioCompraMn", "PrecioVentaMlc", "PrecioVentaMn", "TipoId", "UnidadId", "ProductoOrigenId" },
                values: new object[] { 4, 2, "f678", "Aluminio Laminado", "Derivado", "Aluminio Laminado", 20m, 500m, 48m, 1200m, 1, 1, 3 });

            migrationBuilder.InsertData(
                table: "Submayor",
                columns: new[] { "AlmacenId", "ProductoId", "Cantidad", "UnidadId" },
                values: new object[] { 1, 1, 100m, 1 });

            migrationBuilder.InsertData(
                table: "Submayor",
                columns: new[] { "AlmacenId", "ProductoId", "Cantidad", "UnidadId" },
                values: new object[] { 2, 1, 100m, 1 });

            migrationBuilder.InsertData(
                table: "Submayor",
                columns: new[] { "AlmacenId", "ProductoId", "Cantidad", "UnidadId" },
                values: new object[] { 3, 1, 20m, 1 });

            migrationBuilder.InsertData(
                table: "Submayor",
                columns: new[] { "AlmacenId", "ProductoId", "Cantidad", "UnidadId" },
                values: new object[] { 4, 1, 30m, 1 });

            migrationBuilder.InsertData(
                table: "Submayor",
                columns: new[] { "AlmacenId", "ProductoId", "Cantidad", "UnidadId" },
                values: new object[] { 1, 2, 1000m, 3 });

            migrationBuilder.InsertData(
                table: "Submayor",
                columns: new[] { "AlmacenId", "ProductoId", "Cantidad", "UnidadId" },
                values: new object[] { 2, 2, 750m, 3 });

            migrationBuilder.InsertData(
                table: "Submayor",
                columns: new[] { "AlmacenId", "ProductoId", "Cantidad", "UnidadId" },
                values: new object[] { 3, 2, 400m, 3 });

            migrationBuilder.InsertData(
                table: "Submayor",
                columns: new[] { "AlmacenId", "ProductoId", "Cantidad", "UnidadId" },
                values: new object[] { 4, 2, 350m, 3 });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UnidadOrganizativaId",
                table: "AspNetUsers",
                column: "UnidadOrganizativaId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesDeEntradas_EntradaId",
                table: "DetallesDeEntradas",
                column: "EntradaId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesDeEntradas_ProductoId",
                table: "DetallesDeEntradas",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesDeProcesamiento_DerivadoId",
                table: "DetallesDeProcesamiento",
                column: "DerivadoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesDeProcesamiento_ProcesamientoId",
                table: "DetallesDeProcesamiento",
                column: "ProcesamientoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesDeTransferencia_ProductoId",
                table: "DetallesDeTransferencia",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesDeTransferencia_TransferenciaId",
                table: "DetallesDeTransferencia",
                column: "TransferenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesDeVenta_ProductoId",
                table: "DetallesDeVenta",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesDeVenta_VentaId",
                table: "DetallesDeVenta",
                column: "VentaId");

            migrationBuilder.CreateIndex(
                name: "IX_Entrada_ClienteId",
                table: "Entrada",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Entrada_UnidadOrganizativaId",
                table: "Entrada",
                column: "UnidadOrganizativaId");

            migrationBuilder.CreateIndex(
                name: "IX_Procesamientos_ProductoId",
                table: "Procesamientos",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_Procesamientos_UnidadOrganizativaId",
                table: "Procesamientos",
                column: "UnidadOrganizativaId");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_ProductoOrigenId",
                table: "Producto",
                column: "ProductoOrigenId");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_CategoriaId",
                table: "Producto",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_TipoId",
                table: "Producto",
                column: "TipoId");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_UnidadId",
                table: "Producto",
                column: "UnidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Submayor_ProductoId",
                table: "Submayor",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_Submayor_UnidadId",
                table: "Submayor",
                column: "UnidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Transferencias_DestinoId",
                table: "Transferencias",
                column: "DestinoId");

            migrationBuilder.CreateIndex(
                name: "IX_Transferencias_OrigenId",
                table: "Transferencias",
                column: "OrigenId");

            migrationBuilder.CreateIndex(
                name: "IX_UnidadesOrganizativas_UebId",
                table: "UnidadesOrganizativas",
                column: "UebId");

            migrationBuilder.CreateIndex(
                name: "IX_Venta_ClienteId",
                table: "Venta",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Venta_UnidadOrganizativaId",
                table: "Venta",
                column: "UnidadOrganizativaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "DetallesDeEntradas");

            migrationBuilder.DropTable(
                name: "DetallesDeProcesamiento");

            migrationBuilder.DropTable(
                name: "DetallesDeTransferencia");

            migrationBuilder.DropTable(
                name: "DetallesDeVenta");

            migrationBuilder.DropTable(
                name: "Submayor");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Entrada");

            migrationBuilder.DropTable(
                name: "Procesamientos");

            migrationBuilder.DropTable(
                name: "Transferencias");

            migrationBuilder.DropTable(
                name: "Venta");

            migrationBuilder.DropTable(
                name: "Producto");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "UnidadesOrganizativas");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "TipoDeProducto");

            migrationBuilder.DropTable(
                name: "UM");
        }
    }
}
