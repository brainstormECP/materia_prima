using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MateriasPrimasApp.Models;

namespace MateriasPrimasApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole[] {
                    new IdentityRole {Id = "1", Name = "Administrador", NormalizedName = "ADMINISTRADOR" },
                    new IdentityRole {Id = "2", Name = "Comercial", NormalizedName = "COMERCIAL" },
                    new IdentityRole {Id = "3", Name = "Consultor", NormalizedName = "CONSULTOR" },
             });
            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "f42559a2-2776-4e9b-9ba1-268597eff72b",
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@materiaprima.cu",
                    NormalizedEmail = "ADMIN@MATERIAPRIMA.CU",
                    PasswordHash = "AQAAAAEAACcQAAAAEP4OedI6m26WUn/2C4AcBkzdT6SnL/6E+xakQ/9mGAkqqp3t9PwyIR6l9obLouKIVg==",
                    SecurityStamp = "43VMKYQKNTENYZVJNU2TII26X23H5PGV",
                    ConcurrencyStamp = "36fd2616-8e8a-4cc6-8a5a-52d963207836",
                    Active = true
                },
                new ApplicationUser
                {
                    Id = "e4acfaab-e3c9-42ef-9e21-1902da5374af",
                    UserName = "user1",
                    NormalizedUserName = "USER1",
                    Email = "user1@mp.cu",
                    NormalizedEmail = "USER1@MP.CU",
                    PasswordHash = "AQAAAAEAACcQAAAAECmA0XlVxV7cpw5UFHBIIDAKwZ9RSjLf0g5QOiC9UwYIi8cn0Lw+2QqwOsDdtWkEyw==",
                    SecurityStamp = "4AVYZFGP54QSAT3RJN4KI5R327NN4ID2",
                    ConcurrencyStamp = "e3730b4c-284a-48da-ace3-b5f31f5671df",
                    Active = true,
                    UnidadOrganizativaId = 1,
                },
                new ApplicationUser
                {
                    Id = "4396dcc4-83c3-4e66-9416-c7df16be8b4a",
                    UserName = "Juan",
                    NormalizedUserName = "JUAN",
                    Email = "juan@mp.cu",
                    NormalizedEmail = "JUAN@MP.CU",
                    PasswordHash = "AQAAAAEAACcQAAAAEOXJ8QHXja0i7s0kzcxlgJeT8xXS69ir2aCIkkIRoWjXP+GMHbeQsL/hAnMnfHziPg==",
                    SecurityStamp = "CLPB5NJX34OWXJDQZSTE3NUCQKSHDTI2",
                    ConcurrencyStamp = "22331850-3299-4df8-b760-4f58cf646be0",
                    Active = true,
                });
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = "f42559a2-2776-4e9b-9ba1-268597eff72b",
                    RoleId = "1"
                },
                new IdentityUserRole<string>
                {
                    UserId = "e4acfaab-e3c9-42ef-9e21-1902da5374af",
                    RoleId = "2"
                },
                new IdentityUserRole<string>
                {
                    UserId = "4396dcc4-83c3-4e66-9416-c7df16be8b4a",
                    RoleId = "3"
                });

            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { Id = 1, Nombre = "Ferroso", Descripcion = "Metales ferrosos tales como Hierro y Acero" },
                new Categoria { Id = 2, Nombre = "No Ferroso", Descripcion = "Metales no ferrosos tales como Aluminio y Cobre" },
                new Categoria { Id = 3, Nombre = "Envases y varios", Descripcion = "Envases de cristal como Botellas de cerveza y otros productos reciclables como el cartón" }
                );
            modelBuilder.Entity<UnidadDeMedida>().HasData(
                new UnidadDeMedida { Id = 1, Unidad = "Ton", Descripcion = "Tonelada" },
                new UnidadDeMedida { Id = 2, Unidad = "Kg", Descripcion = "KiloGramo" },
                new UnidadDeMedida { Id = 3, Unidad = "U", Descripcion = "Unidad" }
                );
            modelBuilder.Entity<TipoDeProducto>().HasData(
                new TipoDeProducto { Id = 1, Nombre = "Metálico", Descripcion = "Metales" },
                new TipoDeProducto { Id = 2, Nombre = "No Metálico", Descripcion = "Otros Materiales no metálicos" }
                );
            modelBuilder.Entity<Producto>().HasData(
                new Producto { Id = 1, Codigo = "1abcc345", CategoriaId = 1, Nombre = "Hierro", Descripcion = "Hierro Fundido", UnidadId = 1, TipoId = 1, PrecioCompraMn = 100M, PrecioCompraMlc = 4M, PrecioVentaMn = 150M, PrecioVentaMlc = 6M },
                new Producto { Id = 2, Codigo = "2def678", CategoriaId = 3, Nombre = "Botella de Ron", Descripcion = "Botella de Ron", UnidadId = 3, TipoId = 2, PrecioCompraMn = 1M, PrecioCompraMlc = 0.05M, PrecioVentaMn = 2M, PrecioVentaMlc = 0.10M },
                new Producto { Id = 3, Codigo = "aldef678", CategoriaId = 2, Nombre = "Aluminio", Descripcion = "Aluminio", UnidadId = 1, TipoId = 1, PrecioCompraMn = 500M, PrecioCompraMlc = 20M, PrecioVentaMn = 800M, PrecioVentaMlc = 32M }

                );
            modelBuilder.Entity<Derivado>().HasData(
                new Derivado { Id = 4, Codigo = "f678", CategoriaId = 2, Nombre = "Aluminio Laminado", Descripcion = "Aluminio Laminado", UnidadId = 1, TipoId = 1, PrecioCompraMn = 500M, PrecioCompraMlc = 20M, PrecioVentaMn = 1200M, PrecioVentaMlc = 48M, ProductoOrigenId = 3 },
                new Derivado { Id = 5, Codigo = "f679", CategoriaId = 2, Nombre = "Aluminio Perfilado", Descripcion = "Aluminio Perfilado", UnidadId = 1, TipoId = 1, PrecioCompraMn = 500M, PrecioCompraMlc = 20M, PrecioVentaMn = 1200M, PrecioVentaMlc = 48M, ProductoOrigenId = 3 },
                new Derivado { Id = 6, Codigo = "f680", CategoriaId = 2, Nombre = "Aluminio Fundido", Descripcion = "Aluminio Fundido", UnidadId = 1, TipoId = 1, PrecioCompraMn = 500M, PrecioCompraMlc = 20M, PrecioVentaMn = 1200M, PrecioVentaMlc = 48M, ProductoOrigenId = 3 },
                new Derivado { Id = 7, Codigo = "f681", CategoriaId = 1, Nombre = "Hierro Fundido", Descripcion = "Hierro Fundido", UnidadId = 1, TipoId = 1, PrecioCompraMn = 100M, PrecioCompraMlc = 4M, PrecioVentaMn = 250M, PrecioVentaMlc = 10M, ProductoOrigenId = 1 }

                );

            modelBuilder.Entity<UEB>().HasData(
                new UEB { Id = 1, Municipio = "Matanzas", Nombre = "UEBMtz", Telefono = "262100" },
                new UEB { Id = 2, Municipio = "Colón", Nombre = "UEBCol", Telefono = "371304" }
                );
            modelBuilder.Entity<CasaCompra>().HasData(
                new CasaCompra { Id = 3, Nombre = "Casa de compras Versalles", UebId = 1 },
                new CasaCompra { Id = 4, Nombre = "Casa de compras Playa", UebId = 1 }
            );
            modelBuilder.Entity<Cliente>().HasData(
                new Cliente { Id = 1, Codigo = "C001", Nombre = "CTEAG", Organismo = "UNE" });

            modelBuilder.Entity<Submayor>().HasKey(k => new { k.AlmacenId, k.ProductoId });
            modelBuilder.Entity<Submayor>().HasData(
                new Submayor { ProductoId = 1, AlmacenId = 1, Cantidad = 0M, UnidadId = 1 },
                new Submayor { ProductoId = 1, AlmacenId = 2, Cantidad = 0M, UnidadId = 1 },
                new Submayor { ProductoId = 1, AlmacenId = 3, Cantidad = 0M, UnidadId = 1 },
                new Submayor { ProductoId = 1, AlmacenId = 4, Cantidad = 0M, UnidadId = 1 },
                new Submayor { ProductoId = 2, AlmacenId = 1, Cantidad = 0M, UnidadId = 3 },
                new Submayor { ProductoId = 2, AlmacenId = 2, Cantidad = 0M, UnidadId = 3 },
                new Submayor { ProductoId = 2, AlmacenId = 3, Cantidad = 0M, UnidadId = 3 },
                new Submayor { ProductoId = 2, AlmacenId = 4, Cantidad = 0M, UnidadId = 3 });
        }



        public DbSet<UnidadDeMedida> UM { get; set; }

        public DbSet<Categoria> Categoria { get; set; }

        public DbSet<Cliente> Cliente { get; set; }

        public DbSet<UEB> UEB { get; set; }

        public DbSet<Producto> Producto { get; set; }

        public DbSet<TipoDeProducto> TipoDeProducto { get; set; }

        public DbSet<Entrada> Entrada { get; set; }

        public DbSet<CasaCompra> CasaCompra { get; set; }

        public DbSet<DetalleDeEntrada> DetallesDeEntradas { get; set; }

        public DbSet<DetalleDeTransferencia> DetallesDeTransferencia { get; set; }

        public DbSet<DetalleDeVenta> DetallesDeVenta { get; set; }

        public DbSet<DetalleDeProcesamiento> DetallesDeProcesamiento { get; set; }

        public DbSet<Transferencia> Transferencias { get; set; }

        public DbSet<Venta> Venta { get; set; }

        public DbSet<Submayor> Submayor { get; set; }

        public DbSet<Procesamiento> Procesamientos { get; set; }

        public DbSet<UnidadOrganizativa> UnidadesOrganizativas { get; set; }

        public DbSet<Derivado> Derivados { get; set; }


    }
}
