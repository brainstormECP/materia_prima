﻿using System;
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
using MateriasPrimasApp.ViewModels;

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

        public async Task<IActionResult> Transferencias()
        {
            ViewData["ProductoId"] = new SelectList(await _context.Producto.ToListAsync(), "Id", "Nombre");
            ViewData["OrigenId"] = new SelectList(await _context.UnidadesOrganizativas.ToListAsync(), "Id", "Nombre");
            ViewData["DestinoId"] = new SelectList(await _context.UEB.ToListAsync(), "Id", "Nombre");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Transferencias(FiltroReporteTransferenciaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                var transeferencias = await _context.Transferencias.Include(t => t.DetallesDeTransferencia).ThenInclude(t=>t.Producto).Include(t=>t.Origen).Include(t=>t.Destino).ToListAsync();
                if (viewModel.FechaInicio != null)
                {
                    transeferencias = transeferencias.Where(t => t.Fecha >= viewModel.FechaInicio).ToList();
                }
                if (viewModel.FechaFin != null)
                {
                    if(viewModel.FechaInicio !=null && viewModel.FechaFin < viewModel.FechaInicio)
                    {
                        ModelState.AddModelError("FechaFin", "la fecha de fin debe ser mayor a la de inicio.");
                        ViewData["ProductoId"] = new SelectList(await _context.Producto.ToListAsync(), "Id", "Nombre", viewModel.ProductoId);
                        ViewData["OrigenId"] = new SelectList(await _context.UnidadesOrganizativas.ToListAsync(), "Id", "Nombre", viewModel.OrigenId);
                        ViewData["DestinoId"] = new SelectList(await _context.UEB.ToListAsync(), "Id", "Nombre", viewModel.DestinoId);
                        return View(viewModel);
                    }
                    transeferencias = transeferencias.Where(t => t.Fecha <= viewModel.FechaFin).ToList();
                }
                if (viewModel.OrigenId != null)
                {
                    transeferencias = transeferencias.Where(t => t.OrigenId == viewModel.OrigenId).ToList();
                }
                if (viewModel.DestinoId != null)
                {
                    transeferencias = transeferencias.Where(t => t.DestinoId == viewModel.DestinoId).ToList();
                }
                if(viewModel.ProductoId != null)
                {
                    transeferencias = transeferencias.Where(t => t.DetallesDeTransferencia.Any(d => d.ProductoId == viewModel.ProductoId)).ToList();
                }
                return View("ReporteTransferencias", transeferencias.OrderByDescending(t=>t.Fecha));
            };

            ViewData["ProductoId"] = new SelectList(await _context.Producto.ToListAsync(), "Id", "Nombre", viewModel.ProductoId);
            ViewData["OrigenId"] = new SelectList(await _context.UnidadesOrganizativas.ToListAsync(), "Id", "Nombre", viewModel.OrigenId);
            ViewData["Destino"] = new SelectList(await _context.UEB.ToListAsync(), "Id", "Nombre", viewModel.DestinoId);
            return View();
        }

    }
}
