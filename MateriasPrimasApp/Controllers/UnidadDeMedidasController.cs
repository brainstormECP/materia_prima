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
    public class UnidadDeMedidasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UnidadDeMedidasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UnidadDeMedidas
        public async Task<IActionResult> Index()
        {
            return View(await _context.UM.ToListAsync());
        }

        // GET: UnidadDeMedidas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unidadDeMedida = await _context.UM
                .FirstOrDefaultAsync(m => m.Id == id);
            if (unidadDeMedida == null)
            {
                return NotFound();
            }

            return View(unidadDeMedida);
        }

        // GET: UnidadDeMedidas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UnidadDeMedidas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Unidad,Descripcion")] UnidadDeMedida unidadDeMedida)
        {
            if (ModelState.IsValid)
            {
                _context.Add(unidadDeMedida);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(unidadDeMedida);
        }

        // GET: UnidadDeMedidas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unidadDeMedida = await _context.UM.FindAsync(id);
            if (unidadDeMedida == null)
            {
                return NotFound();
            }
            return View(unidadDeMedida);
        }

        // POST: UnidadDeMedidas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Unidad,Descripcion")] UnidadDeMedida unidadDeMedida)
        {
            if (id != unidadDeMedida.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(unidadDeMedida);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UnidadDeMedidaExists(unidadDeMedida.Id))
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
            return View(unidadDeMedida);
        }

        // GET: UnidadDeMedidas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unidadDeMedida = await _context.UM
                .FirstOrDefaultAsync(m => m.Id == id);
            if (unidadDeMedida == null)
            {
                return NotFound();
            }

            return View(unidadDeMedida);
        }

        // POST: UnidadDeMedidas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var unidadDeMedida = await _context.UM.FindAsync(id);
            _context.UM.Remove(unidadDeMedida);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UnidadDeMedidaExists(int id)
        {
            return _context.UM.Any(e => e.Id == id);
        }
    }
}
