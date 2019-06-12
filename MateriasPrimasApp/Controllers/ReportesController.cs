using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MateriasPrimasApp.Data;
using MateriasPrimasApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using MateriasPrimasApp.HelperClass;
using MateriasPrimasApp.ViewModels;

namespace MateriasPrimasApp.Controllers
{
    [Authorize]
    public class ReportesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Procesamientos
        public async Task<IActionResult> ExistenciasPorUeb()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var helper = new ReporteExistenciaHelper(_context);
            var ueb = await _context.UEB.FindAsync(user.UnidadOrganizativaId);
            ViewBag.UEB = ueb.Nombre;
            return View(helper.GetExistencias((int)user.UnidadOrganizativaId));
        }

        public async Task<IActionResult> ConciliacionVentas()
        {
            ViewBag.Ueb = new SelectList(_context.Set<UEB>(), "Id", "Nombre");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConciliacionVentas(ParametroVentasVM parametros)
        {
            var result = new List<ConciliacionVentasVM>();
            var ventas = _context.Set<DetalleDeVenta>()
                .Include(v => v.Producto.Unidad)
                .Include(v => v.Venta)
                .Where(v => v.Venta.Fecha.Year == parametros.Año);
            result = ventas.GroupBy(v => v.Producto).Select(v => new ConciliacionVentasVM
            {
                Producto = v.Key.Nombre,
                Um = v.Key.Unidad.Descripcion,
                VentaMn = v.Where(d => d.Venta.Fecha.Month == parametros.Mes).Sum(d => d.PrecioVentaMn * d.Cantidad),
                VentaCuc = v.Where(d => d.Venta.Fecha.Month == parametros.Mes).Sum(d => d.PrecioVentaMlc * d.Cantidad),
                AcumuladoMn = v.Sum(d => d.PrecioVentaMn * d.Cantidad),
                AcumuladoCuc = v.Sum(d => d.PrecioVentaMlc * d.Cantidad),
            }).ToList();
            ViewBag.Ueb = _context.Set<UEB>().SingleOrDefault(u => u.Id == parametros.Ueb).Nombre;
            ViewBag.Mes = $"{parametros.Mes}/{parametros.Año}";
            return View("ConciliacionVentasData", result);
        }
    }
}
