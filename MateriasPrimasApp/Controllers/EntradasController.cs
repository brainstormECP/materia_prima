using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MateriasPrimaApp.Models;
using MateriasPrimasApp.Data;
using MateriasPrimasApp.Models;
using Microsoft.AspNetCore.Identity;
using MateriasPrimasApp.HelperClass;
using Microsoft.AspNetCore.Authorization;

namespace MateriasPrimasApp.Controllers
{
    [Authorize(Roles = "Usuario")]
    public class EntradasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ControlSubMayor controlSubMayor;

        public EntradasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            controlSubMayor = new ControlSubMayor(context);
        }

        // GET: Entradas
        public async Task<IActionResult> Index()
        {
            var user = _context.Users.Find(_userManager.GetUserId(User));
            if (user.UnidadOrganizativaId != null)
            {
                var entradas = await _context.Entrada.Include(e => e.Cliente).Include(e => e.UnidadOrganizativa).Where(e => e.UnidadOrganizativaId == user.UnidadOrganizativaId).ToListAsync();
                return View(entradas);
            }
            return NotFound();
        }

        // GET: Entradas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entrada = await _context.Entrada
                .Include(e => e.Cliente)
                .Include(e => e.UnidadOrganizativa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entrada == null)
            {
                return NotFound();
            }

            ViewData["Detalles"] = _context.DetallesDeEntradas.Where(d => d.EntradaId == id)
                                                      .Include(p => p.Producto)
                                                      .ToList<DetalleDeEntrada>(); ;
            return View(entrada);
        }

        // GET: Entradas/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Codigo");
            //ViewData["UnidadOrganizativaId"] = new SelectList(_context.Set<UnidadOrganizativa>(), "Id", "Nombre");
            return View();
        }

        // POST: Entradas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fecha,ClienteId")] Entrada entrada)
        {
            var user = _context.Users.Find(_userManager.GetUserId(User));

            entrada.UnidadOrganizativaId = (int)user.UnidadOrganizativaId;

            if (ModelState.IsValid)
            {
                _context.Add(entrada);
                await _context.SaveChangesAsync();
                return RedirectToAction("DetallesDeEntrada", new { id = entrada.Id });
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Codigo", entrada.ClienteId);
            ViewData["UnidadOrganizativaId"] = new SelectList(_context.Set<UnidadOrganizativa>(), "Id", "Nombre", entrada.UnidadOrganizativaId);
            return View(entrada);
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
        public async Task<IActionResult> AddDetalle(DetalleDeEntrada detalle)
        {
            //Agregando el monto total de la entrada de un producto en base a su precio de compra
            var producto = _context.Producto.Find(detalle.ProductoId);
            detalle.PrecioMn = detalle.Cantidad * producto.PrecioCompraMn;
            detalle.PrecioMlc = detalle.Cantidad * producto.PrecioCompraMlc;

            if (ModelState.IsValid)
            {
                await _context.DetallesDeEntradas.AddAsync(detalle);
                await _context.SaveChangesAsync();
                return ViewComponent("Detalles", new { EntradaId = detalle.EntradaId });
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
            DetalleDeEntrada detalle = _context.DetallesDeEntradas.FirstOrDefault(d => d.Id == id);

            if (detalle == null)
            {
                return BadRequest(ModelState);
            }

            _context.DetallesDeEntradas.Remove(detalle);

            await _context.SaveChangesAsync();
            return ViewComponent("Detalles", new { EntradaId = detalle.EntradaId });


        }

        // GET: Entradas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = _context.Users.Find(_userManager.GetUserId(User));

            var entrada = await _context.Entrada.FindAsync(id);
            if (entrada == null || entrada.UnidadOrganizativaId != user.UnidadOrganizativaId || entrada.Confirmada)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Codigo", entrada.ClienteId);
            ViewData["UnidadOrganizativaId"] = new SelectList(_context.Set<UnidadOrganizativa>(), "Id", "Nombre", entrada.UnidadOrganizativaId);
            return View(entrada);
        }

        // POST: Entradas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha,ClienteId,UnidadOrganizativaId")] Entrada entrada)
        {
            if (id != entrada.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entrada);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntradaExists(entrada.Id))
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
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Codigo", entrada.ClienteId);
            ViewData["UnidadOrganizativaId"] = new SelectList(_context.Set<UnidadOrganizativa>(), "Id", "Nombre", entrada.UnidadOrganizativaId);
            return View(entrada);
        }

        // GET: Entradas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = _context.Users.Find(_userManager.GetUserId(User));

            var entrada = await _context.Entrada
                .Include(e => e.Cliente)
                .Include(e => e.UnidadOrganizativa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entrada == null || entrada.UnidadOrganizativaId != user.UnidadOrganizativaId || entrada.Confirmada)
            {
                return NotFound();
            }

            return View(entrada);
        }

        // GET: Entradas/Confirmar/5
        public async Task<IActionResult> Confirmar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entrada = await _context.Entrada
                .Include(e => e.Cliente)
                .Include(e => e.UnidadOrganizativa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entrada == null || entrada.Confirmada)
            {
                return NotFound();
            }
            List<DetalleDeEntrada> detalles = _context.DetallesDeEntradas
                                                      .Where(d => d.EntradaId == id)
                                                      .Include(p => p.Producto)
                                                      .ToList<DetalleDeEntrada>();
            ViewData["Detalles"] = detalles;
            return View(entrada);
        }

        // POST: Entradas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entrada = await _context.Entrada.FindAsync(id);
            _context.Entrada.Remove(entrada);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // POST: Entradas/Delete/5
        [HttpPost, ActionName("Confirmar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarConfirmed(int id)
        {
            var entrada = await _context.Entrada.FindAsync(id);
            entrada.Confirmada = true;
            _context.Entrada.Update(entrada);
            controlSubMayor.DarEntrada(entrada);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EntradaExists(int id)
        {
            return _context.Entrada.Any(e => e.Id == id);
        }
    }
}
