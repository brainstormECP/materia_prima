using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MateriasPrimaApp.Models;
using MateriasPrimasApp.Data;
using MateriasPrimasApp.HelperClass;
using Microsoft.AspNetCore.Identity;
using MateriasPrimasApp.Models;

namespace MateriasPrimasApp.Controllers
{
    public class VentasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ControlSubMayor controlSubMayor;

        public VentasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            controlSubMayor = new ControlSubMayor(context);
        }

        // GET: Ventas
        public async Task<IActionResult> Index()
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var ventas = _context.Venta.Where(v=>v.UnidadOrganizativaId == user.UnidadOrganizativaId).Include(v => v.Cliente).Include(v => v.UnidadOrganizativa).Include(v=>v.DetallesDeVenta);
            return View(await ventas.ToListAsync());
        }

        // GET: Ventas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Venta
                .Include(v => v.Cliente)
                .Include(v => v.UnidadOrganizativa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // GET: Ventas/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Nombre");
            //ViewData["UnidadOrganizativaId"] = new SelectList(_context.Set<UnidadOrganizativa>(), "Id", "Nombre");
            return View();
        }

        // POST: Ventas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fecha,ClienteId")] Venta venta)
        {
            var user = _context.Users.Find(_userManager.GetUserId(User));

            venta.UnidadOrganizativaId = (int)user.UnidadOrganizativaId;

            if (ModelState.IsValid)
            {
                _context.Add(venta);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Nombre", venta.ClienteId);
            ViewData["UnidadOrganizativaId"] = new SelectList(_context.Set<UnidadOrganizativa>(), "Id", "Nombre", venta.UnidadOrganizativaId);
            return View(venta);
        }

        // GET: Ventas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Venta.FindAsync(id);
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (venta == null || venta.UnidadOrganizativaId != user.UnidadOrganizativaId)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Codigo", venta.ClienteId);
            ViewData["UnidadOrganizativaId"] = new SelectList(_context.Set<UnidadOrganizativa>(), "Id", "Nombre", venta.UnidadOrganizativaId);
            return View(venta);
        }

        // POST: Ventas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha,ClienteId,UnidadOrganizativaId")] Venta venta)
        {
            if (id != venta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentaExists(venta.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Codigo", venta.ClienteId);
            ViewData["UnidadOrganizativaId"] = new SelectList(_context.Set<UnidadOrganizativa>(), "Id", "Nombre", venta.UnidadOrganizativaId);
            return View(venta);
        }

        // GET: Ventas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Venta
                .Include(v => v.Cliente)
                .Include(v => v.UnidadOrganizativa)
                .FirstOrDefaultAsync(m => m.Id == id);
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (venta == null || venta.UnidadOrganizativaId == user.UnidadOrganizativaId)
            {
                return NotFound();
            }

            return View(venta);
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venta = await _context.Venta.FindAsync(id);
            _context.Venta.Remove(venta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Entradas/Create
        public IActionResult DetallesDeEntrada(int id)
        {
            var entrada = _context.Entrada.Include(e => e.UnidadOrganizativa).Include(e => e.Cliente).FirstOrDefault(e => e.Id == id);
            if (entrada == null || entrada.Confirmada)
            {
                return NotFound();
            }

            ViewData["Productos"] = new SelectList(_context.Producto.ToList(), "Id", "Nombre");
            ViewData["UnidadesDeMedidas"] = new SelectList(_context.UM.ToList(), "Id", "Unidad");
            return View(entrada);
        }

        [HttpPost]
        public async Task<IActionResult> AddDetalle(DetalleDeVenta detalle)
        {
            //Agregando el monto total de la entrada de un producto en base a su precio de compra
            var producto = _context.Producto.Find(detalle.ProductoId);
            detalle.PrecioMn = detalle.Cantidad * producto.PrecioVentaMn;
            detalle.PrecioMlc = detalle.Cantidad * producto.PrecioVentaMlc;

            if (ModelState.IsValid)
            {
                await _context.DetallesDeVenta.AddAsync(detalle);
                await _context.SaveChangesAsync();
                return ViewComponent("DetallesDeVenta", new { VentaId = detalle.VentaId });
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> EliminarDetalle(int? id)
        {
            if (id == null)
            {
                return BadRequest(ModelState);
            }
            DetalleDeVenta detalle = _context.DetallesDeVenta.FirstOrDefault(d => d.Id == id);

            if (detalle == null)
            {
                return BadRequest(ModelState);
            }

            _context.DetallesDeVenta.Remove(detalle);

            await _context.SaveChangesAsync();
            return ViewComponent("DetallesDeVenta", new { VentaId = detalle.VentaId });

        }

        public IActionResult DetallesDeVenta(int id)
        {
            var venta = _context.Venta.Include(e => e.UnidadOrganizativa).Include(e => e.Cliente).FirstOrDefault(e => e.Id == id);
            if (venta == null || venta.Confirmada)
            {
                return NotFound();
            }

            ViewData["Productos"] = new SelectList(_context.Producto.ToList(), "Id", "Nombre");
            return View(venta);
        }

        [HttpGet]
        public async Task<IActionResult> Confirmar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Venta
                .Include(e => e.Cliente)
                .Include(e => e.UnidadOrganizativa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venta == null || venta.Confirmada)
            {
                return NotFound();
            }
            
            ViewData["Detalles"] = _context.DetallesDeVenta
                                                      .Where(d => d.VentaId == id)
                                                      .Include(p => p.Producto)
                                                      .ToList();
            return View(venta);
        }

        [HttpPost, ActionName("Confirmar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarConfirmed(int id)
        {
            var venta = await _context.Venta.FindAsync(id);
            venta.Confirmada = true;
            _context.Venta.Update(venta);
            controlSubMayor.ProcesarVenta(venta);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        private bool VentaExists(int id)
        {
            return _context.Venta.Any(e => e.Id == id);
        }
    }
}
