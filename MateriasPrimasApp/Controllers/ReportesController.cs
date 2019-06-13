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

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Consultor"))
            {
                ViewData["UebId"] = new SelectList(await _context.UEB.ToListAsync(), "Id", "Nombre");
                return View();
            }
            else
            {
                ApplicationUser usuario = await _userManager.FindByNameAsync(User.Identity.Name);
                if (User.IsInRole("Comercial") && usuario.UnidadOrganizativaId !=null )
                {
                    return RedirectToAction("ExistenciasPorUeb", new { id = usuario.UnidadOrganizativaId });
                }
                ViewData["UebId"] = new SelectList(await _context.UEB.ToListAsync(), "Id", "Nombre");
                return View();
            }
        }

        // GET: Procesamientos
        public async Task<IActionResult> ExistenciasPorUeb(int Id)
        {
            int UOId = 0;
            var ueb = await _context.UEB.FindAsync(Id);
            var helper = new ReporteExistenciaHelper(_context);

            if (ueb != null)
            {
                ViewBag.UEB = ueb.Nombre;
                UOId = Id;
            }
            else
            {
                var casacompra = await _context.CasaCompra.Include(c=>c.Ueb).FirstOrDefaultAsync(c=>c.Id == Id);
                if(casacompra != null)
                {
                    ViewBag.UEB = casacompra.Ueb.Nombre;
                    UOId = casacompra.UebId;
                    TempData["notice"] = "Lo sentimos. Este usuario pertenece a una Casa de Compra y aún no se ha implementado este tipo de reporte.";
                }
            }
            return View(helper.GetExistencias(UOId));
        }

        public async Task<IActionResult> Transferencias()
        {
            ViewData["ProductoId"] = new SelectList(await _context.Producto.ToListAsync(), "Id", "Nombre");
            ViewData["OrigenId"] = new SelectList(await _context.UnidadesOrganizativas.ToListAsync(), "Id", "Nombre");
            ViewData["DestinoId"] = new SelectList(await _context.UEB.ToListAsync(), "Id", "Nombre");
            return View();
        }

        public async Task<IActionResult> ConciliacionVentas()
        {
            ViewBag.Ueb = new SelectList(await _context.UEB.ToListAsync(), "Id", "Nombre");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Transferencias(FiltroReporteTransferenciaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                var transeferencias = await _context.Transferencias.Include(t => t.DetallesDeTransferencia).ThenInclude(t=>t.Producto).ThenInclude(p=>p.Unidad).Include(t=>t.Origen).Include(t=>t.Destino).ToListAsync();
                if (viewModel.FechaInicio != null)
                {
                    transeferencias = transeferencias.Where(t => t.Fecha >= viewModel.FechaInicio).ToList();
                }
                if (viewModel.FechaFin != null)
                {
                    if (viewModel.FechaInicio != null && viewModel.FechaFin < viewModel.FechaInicio)
                    {
                        ModelState.AddModelError("FechaFin", "la fecha de fin debe ser mayor a la de inicio.");
                        ViewData["ProductoId"] = new SelectList(await _context.Producto.ToListAsync(), "Id", "Nombre", viewModel.ProductoId);
                        ViewData["OrigenId"] = new SelectList(await _context.UnidadesOrganizativas.ToListAsync(), "Id", "Nombre", viewModel.OrigenId);
                        ViewData["DestinoId"] = new SelectList(await _context.UEB.ToListAsync(), "Id", "Nombre", viewModel.DestinoId);
                        return View(viewModel);
                    }
                    transeferencias = transeferencias.Where(t => t.Fecha <= viewModel.FechaFin).ToList();
                }
                if (viewModel.OrigenId != null)
                {
                    transeferencias = transeferencias.Where(t => t.OrigenId == viewModel.OrigenId).ToList();
                }
                if (viewModel.DestinoId != null)
                {
                    transeferencias = transeferencias.Where(t => t.DestinoId == viewModel.DestinoId).ToList();
                }
                if (viewModel.ProductoId != null)
                {
                    transeferencias = transeferencias.Where(t => t.DetallesDeTransferencia.Any(d => d.ProductoId == viewModel.ProductoId)).ToList();
                }
                return View("ReporteTransferencias", transeferencias.OrderByDescending(t => t.Fecha));
            };

            ViewData["ProductoId"] = new SelectList(await _context.Producto.ToListAsync(), "Id", "Nombre", viewModel.ProductoId);
            ViewData["OrigenId"] = new SelectList(await _context.UnidadesOrganizativas.ToListAsync(), "Id", "Nombre", viewModel.OrigenId);
            ViewData["Destino"] = new SelectList(await _context.UEB.ToListAsync(), "Id", "Nombre", viewModel.DestinoId);
            return View(viewModel);
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

        public async Task<IActionResult> GraficoCompraVentas()
        {
            ViewBag.Ueb = new SelectList(_context.Set<UEB>(), "Id", "Nombre");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GraficoCompraVentas(ParametroVentasVM parametros)
        {
            var ventas = _context.Set<Venta>()
                .Include(v => v.DetallesDeVenta)
                .Where(v => v.Fecha.Year == parametros.Año)
                .GroupBy(v => v.Fecha.Month)
                .Select(v => new
                {
                    Mes = v.Key,
                    Ventas = v.Sum(d => d.DetallesDeVenta.Sum(e => e.PrecioVentaMlc)) + v.Sum(d => d.DetallesDeVenta.Sum(e => e.PrecioVentaMn)),
                }).ToList();
            var compras = _context.Set<Entrada>()
                .Include(c => c.DetallesDeEntrada)
                .Where(c => c.Fecha.Year == parametros.Año)
                .GroupBy(v => v.Fecha.Month)
                .Select(v => new
                {
                    Mes = v.Key,
                    Compras = v.Sum(d => d.DetallesDeEntrada.Sum(e => e.PrecioMlc)) + v.Sum(d => d.DetallesDeEntrada.Sum(e => e.PrecioMn)),
                }).ToList();

            var result = ventas.Join(compras, v => v.Mes, c => c.Mes, (v, c) => new
            {
                Mes = v.Mes,
                Ventas = v.Ventas,
                Compras = c.Compras,
            }).ToList();


            var labels = result.OrderBy(r => r.Mes).Select(r => r.Mes + "/" + parametros.Año).ToList();
            var datosCosto = new DatosGraficas()
            {
                Labels = labels,
            };
            var datosVentas = new DatosGraficas()
            {
                Labels = labels,
            };
            var index = 0;
            datosVentas.Datasets.Add(new Dataset
            {
                Label = "Ventas",
                BackgroundColor = "#177ed8",
                BorderColor = "#177ed8",
                Fill = false,
                Data = labels.Select(c => ventas.Any(d => d.Mes + "/" + parametros.Año == c) ? ventas.Where(d => d.Mes + "/" + parametros.Año == c).Sum(s => s.Ventas) : 0).ToList()
            });
            datosVentas.Datasets.Add(new Dataset
            {
                Label = "Compras",
                BackgroundColor = "#d8174c",
                BorderColor = "#d8174c",
                Fill = false,
                Data = labels.Select(c => compras.Any(d => d.Mes + "/" + parametros.Año == c) ? compras.Where(d => d.Mes + "/" + parametros.Año == c).Sum(s => s.Compras) : 0).ToList()
            });
            ViewBag.Ueb = new SelectList(_context.Set<UEB>(), "Id", "Nombre");
            ViewBag.Ventas = datosVentas;
            return View();
        }
    }
}
