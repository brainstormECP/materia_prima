using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MateriasPrimasApp.Models;
using MateriasPrimasApp.Data;

namespace MateriasPrimasApp.Controllers
{
    public class TipoDeProductoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TipoDeProductoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TipoDeProductoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoDeProducto.ToListAsync());
        }

        // GET: TipoDeProductoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoDeProducto = await _context.TipoDeProducto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoDeProducto == null)
            {
                return NotFound();
            }

            return View(tipoDeProducto);
        }

        // GET: TipoDeProductoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoDeProductoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion")] TipoDeProducto tipoDeProducto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoDeProducto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoDeProducto);
        }

        // GET: TipoDeProductoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoDeProducto = await _context.TipoDeProducto.FindAsync(id);
            if (tipoDeProducto == null)
            {
                return NotFound();
            }
            return View(tipoDeProducto);
        }

        // POST: TipoDeProductoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion")] TipoDeProducto tipoDeProducto)
        {
            if (id != tipoDeProducto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoDeProducto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoDeProductoExists(tipoDeProducto.Id))
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
            return View(tipoDeProducto);
        }

        // GET: TipoDeProductoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoDeProducto = await _context.TipoDeProducto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoDeProducto == null)
            {
                return NotFound();
            }

            return View(tipoDeProducto);
        }

        // POST: TipoDeProductoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoDeProducto = await _context.TipoDeProducto.FindAsync(id);
            _context.TipoDeProducto.Remove(tipoDeProducto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoDeProductoExists(int id)
        {
            return _context.TipoDeProducto.Any(e => e.Id == id);
        }
    }
}
