using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MateriasPrimasApp.Data;
using MateriasPrimasApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using MateriasPrimasApp.HelperClass;

namespace MateriasPrimasApp.Controllers
{
    [Authorize]
    public class ReportesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["UebId"] = new SelectList(await _context.UEB.ToListAsync(), "Id", "Nombre");
            return View();
        }

        // GET: Procesamientos
        public async Task<IActionResult> ExistenciasPorUeb(int Id)
        {
            var helper = new ReporteExistenciaHelper(_context);
            var ueb = await _context.UEB.FindAsync(Id);
            ViewBag.UEB = ueb.Nombre;
            return View(helper.GetExistencias(Id));
        }
    }
}
