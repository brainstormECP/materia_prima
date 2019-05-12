using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MateriasPrimaApp.Models;
using MateriasPrimasApp.Data;

namespace MateriasPrimasApp.Controllers
{
    public class ProcesamientosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProcesamientosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Procesamientos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Procesamientos.Include(p => p.ProductoOrigen).Include(p => p.ProductoSalida);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Procesamientos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var procesamiento = await _context.Procesamientos
                .Include(p => p.ProductoOrigen)
                .Include(p => p.ProductoSalida)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (procesamiento == null)
            {
                return NotFound();
            }

            return View(procesamiento);
        }

        // GET: Procesamientos/Create
        public IActionResult Create()
        {
            ViewData["ProductoOrigenId"] = new SelectList(_context.Producto, "Id", "Nombre");
            ViewData["ProductoSalidaId"] = new SelectList(_context.Producto, "Id", "Nombre");
            return View();
        }

        // POST: Procesamientos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductoOrigenId,CantidadOrigen,ProductoSalidaId,CantidadSalida")] Procesamiento procesamiento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(procesamiento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductoOrigenId"] = new SelectList(_context.Producto, "Id", "Nombre", procesamiento.ProductoOrigenId);
            ViewData["ProductoSalidaId"] = new SelectList(_context.Producto, "Id", "Nombre", procesamiento.ProductoSalidaId);
            return View(procesamiento);
        }

        // GET: Procesamientos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var procesamiento = await _context.Procesamientos.FindAsync(id);
            if (procesamiento == null)
            {
                return NotFound();
            }
            ViewData["ProductoOrigenId"] = new SelectList(_context.Producto, "Id", "Nombre", procesamiento.ProductoOrigenId);
            ViewData["ProductoSalidaId"] = new SelectList(_context.Producto, "Id", "Nombre", procesamiento.ProductoSalidaId);
            return View(procesamiento);
        }

        // POST: Procesamientos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductoOrigenId,CantidadOrigen,ProductoSalidaId,CantidadSalida")] Procesamiento procesamiento)
        {
            if (id != procesamiento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(procesamiento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcesamientoExists(procesamiento.Id))
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
            ViewData["ProductoOrigenId"] = new SelectList(_context.Producto, "Id", "Nombre", procesamiento.ProductoOrigenId);
            ViewData["ProductoSalidaId"] = new SelectList(_context.Producto, "Id", "Nombre", procesamiento.ProductoSalidaId);
            return View(procesamiento);
        }

        // GET: Procesamientos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var procesamiento = await _context.Procesamientos
                .Include(p => p.ProductoOrigen)
                .Include(p => p.ProductoSalida)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (procesamiento == null)
            {
                return NotFound();
            }

            return View(procesamiento);
        }

        // POST: Procesamientos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var procesamiento = await _context.Procesamientos.FindAsync(id);
            _context.Procesamientos.Remove(procesamiento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Entradas/Confirmar/5
        public async Task<IActionResult> Confirmar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var procesamiento = await _context.Procesamientos
                .Include(e => e.ProductoOrigen)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (procesamiento == null || procesamiento.Confirmada)
            {
                return NotFound();
            }         
            return View(procesamiento);
        }

        // POST: Entradas/Delete/5
        [HttpPost, ActionName("Confirmar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarConfirmed(int id)
        {
            var procesamiento = await _context.Procesamientos.FindAsync(id);
            procesamiento.Confirmada = true;
            _context.Procesamientos.Update(procesamiento);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProcesamientoExists(int id)
        {
            return _context.Procesamientos.Any(e => e.Id == id);
        }
    }
}
