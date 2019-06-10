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
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            var user = _context.Users.Find(_userManager.GetUserId(User));

            ViewData["entradas"] = _context.Entrada.Where(e=>e.UnidadOrganizativaId == user.UnidadOrganizativaId).Count(e => !e.Confirmada);
            ViewData["transferencias"] = _context.Transferencias.Where(e => e.OrigenId == user.UnidadOrganizativaId || e.DestinoId == user.UnidadOrganizativaId).Count(t => !t.Confirmada);
            ViewData["ventas"] = _context.Venta.Where(e => e.UnidadOrganizativaId == user.UnidadOrganizativaId).Count(t => !t.Confirmada);
            ViewData["Procesamientos"] = _context.Procesamientos.Where(p => p.UnidadOrganizativaId == user.UnidadOrganizativaId).Count(p=> !p.Confirmado);

            return View();
        }      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
