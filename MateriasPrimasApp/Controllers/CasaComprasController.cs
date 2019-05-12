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
    public class CasaComprasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CasaComprasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CasaCompras
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CasaCompra.Include(c => c.Ueb);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CasaCompras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var casaCompra = await _context.CasaCompra
                .Include(c => c.Ueb)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (casaCompra == null)
            {
                return NotFound();
            }

            return View(casaCompra);
        }

        // GET: CasaCompras/Create
        public IActionResult Create()
        {
            ViewData["UebId"] = new SelectList(_context.UEB, "Id", "Nombre");
            return View();
        }

        // POST: CasaCompras/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,UebId,Nombre")] CasaCompra casaCompra)
        {
            if (ModelState.IsValid)
            {
                _context.Add(casaCompra);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["UebId"] = new SelectList(_context.UEB, "Id", "Nombre", casaCompra.UebId);
            return View(casaCompra);
        }

        // GET: CasaCompras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var casaCompra = await _context.CasaCompra.FindAsync(id);
            if (casaCompra == null)
            {
                return NotFound();
            }
            ViewData["UebId"] = new SelectList(_context.UEB, "Id", "Nombre", casaCompra.UebId);
            return View(casaCompra);
        }

        // POST: CasaCompras/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,UebId")] CasaCompra casaCompra)
        {
            if (id != casaCompra.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(casaCompra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CasaCompraExists(casaCompra.Id))
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
            ViewData["UebId"] = new SelectList(_context.UEB, "Id", "Nombre", casaCompra.UebId);
            return View(casaCompra);
        }

        // GET: CasaCompras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var casaCompra = await _context.CasaCompra
                .Include(c => c.Ueb)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (casaCompra == null)
            {
                return NotFound();
            }

            return View(casaCompra);
        }

        // POST: CasaCompras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var casaCompra = await _context.CasaCompra.FindAsync(id);
            _context.CasaCompra.Remove(casaCompra);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CasaCompraExists(int id)
        {
            return _context.CasaCompra.Any(e => e.Id == id);
        }
    }
}
