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
    public class SubMayorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubMayorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SubMayor
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Submayor.Include(s => s.Almacen).Include(s => s.Producto).Include(s => s.Unidad);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SubMayor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var submayor = await _context.Submayor
                .Include(s => s.Almacen)
                .Include(s => s.Producto)
                .Include(s => s.Unidad)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (submayor == null)
            {
                return NotFound();
            }

            return View(submayor);
        }
        

        private bool SubmayorExists(int id)
        {
            return _context.Submayor.Any(e => e.Id == id);
        }
    }
}
