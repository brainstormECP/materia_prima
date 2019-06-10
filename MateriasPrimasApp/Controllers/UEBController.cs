using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MateriasPrimasApp.Models;
using MateriasPrimasApp.Data;
using Microsoft.Extensions.Logging;

namespace MateriasPrimasApp.Controllers
{
    public class UEBController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public UEBController(ApplicationDbContext context, ILogger<UEBController> logger)
        {
            _logger = logger;
            _context = context;
        }

        // GET: UEBs
        public async Task<IActionResult> Index()
        {
            return View(await _context.UEB.ToListAsync());
        }

        // GET: UEBs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uEB = await _context.UEB
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uEB == null)
            {
                return NotFound();
            }

            return View(uEB);
        }

        // GET: UEBs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UEBs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Municipio,Id,Nombre")] UEB uEB)
        {
            if (ModelState.IsValid)
            {
                _context.Add(uEB);
                await _context.SaveChangesAsync();
                _logger.LogTrace("Create" + uEB, uEB);

                return RedirectToAction(nameof(Index));
            }
            return View(uEB);
        }

        // GET: UEBs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uEB = await _context.UEB.FindAsync(id);
            if (uEB == null)
            {
                return NotFound();
            }
            return View(uEB);
        }

        // POST: UEBs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Municipio,Id,Nombre")] UEB uEB)
        {
            if (id != uEB.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uEB);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UEBExists(uEB.Id))
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
            return View(uEB);
        }

        // GET: UEBs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uEB = await _context.UEB
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uEB == null)
            {
                return NotFound();
            }

            return View(uEB);
        }

        // POST: UEBs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var uEB = await _context.UEB.FindAsync(id);
            _context.UEB.Remove(uEB);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UEBExists(int id)
        {
            return _context.UEB.Any(e => e.Id == id);
        }
    }
}
