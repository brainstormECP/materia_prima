using System;
using System.Linq;
using MateriasPrimasApp.Models;
using MateriasPrimasApp.HelperClass;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GestionBaresTest
{
    public class ControlSubmayorTest : BaseTest
    {
        public void AddDatos()
        {

            _db.SaveChanges();
        }
        [Fact]
        public void EntradaTest()
        {
            AddDatos();
            var controlSubmayor = new ControlSubMayor(_db);
            controlSubmayor.DarEntrada(new Entrada { ClienteId = 1, Confirmada = true, Fecha = DateTime.Now, UnidadOrganizativaId = 1 });
            Assert.Equal(1, _db.Set<Entrada>().Count());
        }
    }
}
