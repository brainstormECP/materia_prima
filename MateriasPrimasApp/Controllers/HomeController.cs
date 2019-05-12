using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MateriasPrimasApp.Models;
using Microsoft.AspNetCore.Authorization;
using MateriasPrimasApp.Data;
using Microsoft.AspNetCore.Identity;

namespace MateriasPrimasApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
        }

        public IActionResult Index()
        {
           
            ViewData["entradas"] = _context.Entrada.Count(e => !e.Confirmada);
            ViewData["traslados"] = _context.Traslado.Count();

            return View();
        }      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
