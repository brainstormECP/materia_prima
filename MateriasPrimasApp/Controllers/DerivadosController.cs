using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MateriasPrimasApp.Data;
using MateriasPrimasApp.Models;

namespace MateriasPrimasApp.Controllers
{
    public class DerivadosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DerivadosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Derivados
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Derivados.Include(d => d.Categoria).Include(d => d.Tipo).Include(d => d.Unidad).Include(d => d.ProductoOrigen);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Derivados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var derivado = await _context.Derivados
                .Include(d => d.Categoria)
                .Include(d => d.Tipo)
                .Include(d => d.Unidad)
                .Include(d => d.ProductoOrigen)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (derivado == null)
            {
                return NotFound();
            }

            return View(derivado);
        }

        // GET: Derivados/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nombre");
            ViewData["TipoId"] = new SelectList(_context.TipoDeProducto, "Id", "Nombre");
            ViewData["UnidadId"] = new SelectList(_context.UM, "Id", "Unidad");
            ViewData["ProductoOrigenId"] = new SelectList(_context.Producto, "Id", "Nombre");
            return View();
        }

        // POST: Derivados/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductoOrigenId,Id,Codigo,Nombre,Descripcion,UnidadId,CategoriaId,TipoId,PrecioVentaMn,PrecioVentaMlc,PrecioCompraMn,PrecioCompraMlc")] Derivado derivado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(derivado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nombre", derivado.CategoriaId);
            ViewData["TipoId"] = new SelectList(_context.TipoDeProducto, "Id", "Nombre", derivado.TipoId);
            ViewData["UnidadId"] = new SelectList(_context.UM, "Id", "Unidad", derivado.UnidadId);
            ViewData["ProductoOrigenId"] = new SelectList(_context.Producto, "Id", "Nombre", derivado.ProductoOrigenId);
            return View(derivado);
        }

        // GET: Derivados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var derivado = await _context.Derivados.FindAsync(id);
            if (derivado == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nombre", derivado.CategoriaId);
            ViewData["TipoId"] = new SelectList(_context.TipoDeProducto, "Id", "Nombre", derivado.TipoId);
            ViewData["UnidadId"] = new SelectList(_context.UM, "Id", "Unidad", derivado.UnidadId);
            ViewData["ProductoOrigenId"] = new SelectList(_context.Producto, "Id", "Nombre", derivado.ProductoOrigenId);
            return View(derivado);
        }

        // POST: Derivados/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductoOrigenId,Id,Codigo,Nombre,Descripcion,UnidadId,CategoriaId,TipoId,PrecioVentaMn,PrecioVentaMlc,PrecioCompraMn,PrecioCompraMlc")] Derivado derivado)
        {
            if (id != derivado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(derivado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DerivadoExists(derivado.Id))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nombre", derivado.CategoriaId);
            ViewData["TipoId"] = new SelectList(_context.TipoDeProducto, "Id", "Nombre", derivado.TipoId);
            ViewData["UnidadId"] = new SelectList(_context.UM, "Id", "Unidad", derivado.UnidadId);
            ViewData["ProductoOrigenId"] = new SelectList(_context.Producto, "Id", "Nombre", derivado.ProductoOrigenId);
            return View(derivado);
        }

        // GET: Derivados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var derivado = await _context.Derivados
                .Include(d => d.Categoria)
                .Include(d => d.Tipo)
                .Include(d => d.Unidad)
                .Include(d => d.ProductoOrigen)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (derivado == null)
            {
                return NotFound();
            }

            return View(derivado);
        }

        // POST: Derivados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var derivado = await _context.Derivados.FindAsync(id);
            _context.Derivados.Remove(derivado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DerivadoExists(int id)
        {
            return _context.Derivados.Any(e => e.Id == id);
        }
    }
}
