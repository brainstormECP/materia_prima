using System.Collections.Generic;
using System.Linq;
using MateriasPrimasApp.Models;
using MateriasPrimasApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MateriasPrimasApp.HelperClass
{
    public class ReporteExistenciaHelper
    {
        private DbContext _db;

        public ReporteExistenciaHelper(DbContext context)
        {
            _db = context;
        }

        public List<ExistenciaVM> GetExistencias(int uebId)
        {
            var existencias = new List<ExistenciaVM>();
            var enUeb = _db.Set<Submayor>().Where(s => s.AlmacenId == uebId);
            existencias.AddRange(enUeb.GroupBy(s => s.Producto).Select(s => new ExistenciaVM
            {
                Producto = s.Key.Nombre,
                EnUEB = s.Sum(p => p.Cantidad)
            }));
            var casasDeCompra = _db.Set<CasaCompra>().Where(u => u.UebId == uebId).Select(u => u.Id);
            var enCasaDeCompra = _db.Set<Submayor>()
                .Where(s => casasDeCompra.Contains(s.AlmacenId))
                .GroupBy(s => s.Producto)
                .Select(s => new ExistenciaVM { Producto = s.Key.Nombre, EnCasaDeCompra = s.Sum(e => e.Cantidad) });
            foreach (var item in enCasaDeCompra)
            {
                if (existencias.Any(e => e.Producto == item.Producto))
                {
                    var existencia = existencias.SingleOrDefault(e => e.Producto == item.Producto);
                    existencia.EnCasaDeCompra = item.EnCasaDeCompra;
                }
                else
                {
                    existencias.Add(item);
                }
            }
            var enProcesamiento = _db.Set<Procesamiento>()
                .Where(p => p.UnidadOrganizativaId == uebId && !p.Confirmado)
                .GroupBy(p => p.Producto)
                .Select(p => new ExistenciaVM
                {
                    Producto = p.Key.Nombre,
                    EnProceso = p.Sum(s => s.Cantidad)
                });
            foreach (var item in enProcesamiento)
            {
                if (existencias.Any(e => e.Producto == item.Producto))
                {
                    var existencia = existencias.SingleOrDefault(e => e.Producto == item.Producto);
                    existencia.EnProceso = item.EnProceso;
                }
                else
                {
                    existencias.Add(item);
                }
            }
            return existencias;
        }
    }
}