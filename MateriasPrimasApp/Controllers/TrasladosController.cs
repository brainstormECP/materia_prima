using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MateriasPrimaApp.Models;
using MateriasPrimasApp.Data;
using Microsoft.AspNetCore.Identity;
using MateriasPrimasApp.Models;
using Microsoft.AspNetCore.Authorization;
using MateriasPrimasApp.HelperClass;

namespace MateriasPrimasApp.Controllers
{
    [Authorize(Roles = "Usuario")]
    public class TrasladosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ControlSubMayor controlSubMayor;


        public TrasladosController(ApplicationDbContext context)
        {
            _context = context;
            controlSubMayor = new ControlSubMayor(context);
        }

        // GET: Trasladoes
        public async Task<IActionResult> Index()
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var traslados = _context.Traslado.Where(t => t.OrigenId == user.UnidadOrganizativaId || t.DestinoId == t.DestinoId).Include(t => t.Cliente).Include(t => t.Destino).Include(t => t.Origen);
            return View(await traslados.ToListAsync());
        }

        // GET: Trasladoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var traslado = await _context.Traslado
                .Include(t => t.Cliente)
                .Include(t => t.Destino)
                .Include(t => t.Origen)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (traslado == null)
            {
                return NotFound();
            }
            ViewData["Detalles"] = _context.DetallesDeTraslado.Where(d => d.TrasladoId == id)
                                          .Include(p => p.Producto)
                                          .ToList(); ;


            return View(traslado);
        }

        // GET: Trasladoes/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Nombre");
            ViewData["DestinoId"] = new SelectList(_context.Set<UnidadOrganizativa>(), "Id", "Nombre");
            ViewData["OrigenId"] = new SelectList(_context.Set<UnidadOrganizativa>(), "Id", "Nombre");
            return View();
        }

        // POST: Trasladoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fecha,ClienteId,OrigenId,DestinoId")] Traslado traslado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(traslado);
                await _context.SaveChangesAsync();
                return RedirectToAction("DetallesDeTraslado", new { id = traslado.Id });
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Codigo", traslado.ClienteId);
            ViewData["DestinoId"] = new SelectList(_context.Set<UnidadOrganizativa>(), "Id", "Nombre", traslado.DestinoId);
            ViewData["OrigenId"] = new SelectList(_context.Set<UnidadOrganizativa>(), "Id", "Nombre", traslado.OrigenId);
            return View(traslado);
        }

        public IActionResult DetallesDeTraslado(int id)
        {
            var traslado = _context.Traslado.Include(e => e.Origen).Include(e => e.Cliente).Include(e => e.Destino).FirstOrDefault(e => e.Id == id);
            if (traslado == null)
            {
                return NotFound();
            }

            ViewData["Productos"] = new SelectList(_context.Producto.ToList(), "Id", "Nombre");
            ViewData["UnidadesDeMedidas"] = new SelectList(_context.UM.ToList(), "Id", "Unidad");
            return View(traslado);
        }

        [HttpPost]
        public async Task<IActionResult> AddDetalle(DetalleDeTraslado detalle)
        {
            var producto = _context.Producto.Find(detalle.ProductoId);
            detalle.PrecioMn = detalle.Cantidad * producto.PrecioCompraMn;
            detalle.PrecioMlc = detalle.Cantidad * producto.PrecioCompraMlc;

            if (ModelState.IsValid)
            {
                await _context.DetallesDeTraslado.AddAsync(detalle);
                await _context.SaveChangesAsync();
                return ViewComponent("DetallesDeTraslado", new { TrasladoId = detalle.TrasladoId });
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
            DetalleDeTraslado detalle = _context.DetallesDeTraslado.FirstOrDefault(d => d.Id == id);

            if (detalle == null)
            {
                return BadRequest(ModelState);
            }

            _context.DetallesDeTraslado.Remove(detalle);

            await _context.SaveChangesAsync();
            return ViewComponent("DetallesDeTraslado", new { TrasladoId = detalle.TrasladoId });


        }


        // GET: Trasladoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var traslado = await _context.Traslado.FindAsync(id);
            if (traslado == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Codigo", traslado.ClienteId);
            ViewData["DestinoId"] = new SelectList(_context.Set<UnidadOrganizativa>(), "Id", "Nombre", traslado.DestinoId);
            ViewData["OrigenId"] = new SelectList(_context.Set<UnidadOrganizativa>(), "Id", "Nombre", traslado.OrigenId);
            return View(traslado);
        }

        // POST: Trasladoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha,ClienteId,OrigenId,DestinoId")] Traslado traslado)
        {
            if (id != traslado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(traslado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrasladoExists(traslado.Id))
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
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Codigo", traslado.ClienteId);
            ViewData["DestinoId"] = new SelectList(_context.Set<UnidadOrganizativa>(), "Id", "Nombre", traslado.DestinoId);
            ViewData["OrigenId"] = new SelectList(_context.Set<UnidadOrganizativa>(), "Id", "Nombre", traslado.OrigenId);
            return View(traslado);
        }

        // GET: Trasladoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var traslado = await _context.Traslado
                .Include(t => t.Cliente)
                .Include(t => t.Destino)
                .Include(t => t.Origen)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (traslado == null)
            {
                return NotFound();
            }

            return View(traslado);
        }

        // POST: Trasladoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var traslado = await _context.Traslado.FindAsync(id);
            _context.Traslado.Remove(traslado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Confirmar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var traslado = await _context.Traslado
                .Include(e => e.Cliente)
                .Include(e => e.Origen).Include(e => e.Destino)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (traslado == null || traslado.Confirmada)
            {
                return NotFound();
            }
            List<DetalleDeTraslado> detalles = _context.DetallesDeTraslado
                                                      .Where(d => d.TrasladoId == id)
                                                      .Include(p => p.Producto)
                                                      .ToList();
            ViewData["Detalles"] = detalles;
            return View(traslado);
        }

        [HttpPost, ActionName("Confirmar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarConfirmed(int id)
        {
            var traslado = await _context.Traslado.FindAsync(id);
            var success = controlSubMayor.ProcesarTraslado(traslado);
            if (success)
            {
                traslado.Confirmada = true;
                _context.Traslado.Update(traslado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }



        private bool TrasladoExists(int id)
        {
            return _context.Traslado.Any(e => e.Id == id);
        }
    }
}
