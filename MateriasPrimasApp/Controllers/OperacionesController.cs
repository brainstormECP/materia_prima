using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MateriasPrimasApp.Data;
using MateriasPrimasApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace MateriasPrimasApp.Controllers
{
    public class OperacionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OperacionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult RecibirMPR()
        {

            return View();
        }

        [HttpPost]
        public IActionResult RecibirMPR(EntradaMateriaPrimaViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {

            }
            return View();
        }

    }
}