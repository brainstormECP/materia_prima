using System;
using MateriasPrimasApp.Data;
using MateriasPrimasApp.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GestionBaresTest
{
    public class BaseTest : IDisposable
    {
        public ApplicationDbContext _db { get; set; }
        public BaseTest()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;
            _db = new ApplicationDbContext(options);
            _db.Database.EnsureCreated();
            AddData();
        }

        public void AddData()
        {

            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Database.EnsureDeleted();
            _db.Dispose();
        }
    }
}
