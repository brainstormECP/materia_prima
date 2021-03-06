﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MateriasPrimasApp.Models;
using MateriasPrimasApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using MateriasPrimasApp.HelperClass;
using Microsoft.Extensions.Logging;

namespace MateriasPrimasApp.Controllers
{
    [Authorize(Roles = "Comercial, Consultor, Comercial_General")]
    public class TransferenciasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ControlSubMayor controlSubMayor;
        private readonly ILogger<TransferenciasController> _logger;


        public TransferenciasController(ApplicationDbContext context, ILogger<TransferenciasController> logger)
        {
            _context = context;
            _logger = logger;
            controlSubMayor = new ControlSubMayor(context);
        }

        [Authorize(Roles = "Comercial")]
        // GET: Trasladoes
        public async Task<IActionResult> Index()
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var transferencias = _context.Transferencias.Where(t => t.OrigenId == user.UnidadOrganizativaId || t.DestinoId == user.UnidadOrganizativaId).Include(t => t.Destino).Include(t => t.Origen).Include(t => t.DetallesDeTransferencia).OrderByDescending(s => s.Fecha);
            return View(await transferencias.ToListAsync());
        }

        [Authorize(Roles = "Consultor, Comercial, Comercial_General")]
        public async Task<IActionResult> TodasLasTransferencias()
        {
            var transferencias = _context.Transferencias.Where(t => t.Confirmada).Include(t => t.Destino).Include(t => t.Origen).Include(t => t.DetallesDeTransferencia);
            return View(await transferencias.ToListAsync());
        }

        [Authorize(Roles = "Consultor, Comercial, Comercial_General")]
        // GET: Trasladoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transferencia = await _context.Transferencias
                .Include(t => t.Destino)
                .Include(t => t.Origen)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transferencia == null)
            {
                return NotFound();
            }
            ViewData["Detalles"] = _context.DetallesDeTransferencia.Where(d => d.TransferenciaId == id)
                                          .Include(p => p.Producto)
                                          .ToList(); ;


            return View(transferencia);
        }

        // GET: Trasladoes/Create
        [Authorize(Roles = "Comercial")]
        public IActionResult Create()
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var unidad = _context.UnidadesOrganizativas.Find(user.UnidadOrganizativaId);

            ViewData["DestinoId"] = new SelectList(_context.UnidadesOrganizativas.Where(u => !(u is CasaCompra) && u.Id != unidad.Id), "Id", "Nombre");
            ViewData["Origen"] = unidad.Nombre;
            return View();
        }

        // POST: Trasladoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Comercial")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fecha,DestinoId")] Transferencia transferencia)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var unidad = _context.UnidadesOrganizativas.Find(user.UnidadOrganizativaId);
            transferencia.OrigenId = unidad.Id;
            if (ModelState.IsValid)
            {
                _context.Add(transferencia);
                await _context.SaveChangesAsync();

                TempData["exito"] = "Transeferencia creada satisfactoriamente";
                return RedirectToAction("DetallesDeTransferencia", new { id = transferencia.Id });
            }

            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Nombre");
            ViewData["DestinoId"] = new SelectList(_context.UnidadesOrganizativas.Where(u => !(u is CasaCompra) && u.Id != unidad.Id), "Id", "Nombre");
            ViewData["Origen"] = unidad.Nombre;
            return View(transferencia);
        }

        [Authorize(Roles = "Comercial")]
        public IActionResult DetallesDeTransferencia(int id)
        {
            var transferencia = _context.Transferencias.Include(e => e.Origen).Include(e => e.Destino).FirstOrDefault(e => e.Id == id);
            if (transferencia == null)
            {
                return NotFound();
            }
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var unidad = _context.UnidadesOrganizativas.Find(user.UnidadOrganizativaId);


            ViewData["Productos"] = new SelectList(_context.Submayor.Include(s => s.Producto)
                .Where(s => s.AlmacenId == user.UnidadOrganizativaId && s.Cantidad > 0)
                .Select(s => s.Producto)
                .Include(p => p.Unidad)
                .Include(p => p.Categoria).ToList(), "Id", "Nombre");
            ViewData["UnidadesDeMedidas"] = new SelectList(_context.UM.ToList(), "Id", "Unidad");
            return View(transferencia);
        }

        [Authorize(Roles = "Comercial")]
        [HttpPost]
        public async Task<IActionResult> AddDetalle(DetalleDeTransferencia detalle)
        {
            var producto = _context.Producto.Find(detalle.ProductoId);
            detalle.PrecioMn = detalle.Cantidad * producto.PrecioCompraMn;
            detalle.PrecioMlc = detalle.Cantidad * producto.PrecioCompraMlc;
            if (_context.DetallesDeTransferencia.Any(d => d.ProductoId == detalle.ProductoId && d.TransferenciaId == detalle.TransferenciaId))
            {
                ModelState.AddModelError("ProductoId", "El producto selecionado ya se encuentra en esta transferencia");
            }

            //Controlar excepción en caso de que la cantidad a vender sea mayor a la existente en el almacén
            var transferencia = _context.Transferencias.Find(detalle.TransferenciaId);
            if (detalle.Cantidad == 0)
            {
                ModelState.AddModelError("Cantidad", "Debe especificar una cantidad para este Producto");
            }

            if (detalle.Cantidad > controlSubMayor.GetExistenciaPorUO(detalle.ProductoId, transferencia.OrigenId))
            {
                ModelState.AddModelError("Cantidad", "No existe esa cantidad en el almacén de orígen");
            }
            if (ModelState.IsValid)
            {
                await _context.DetallesDeTransferencia.AddAsync(detalle);
                await _context.SaveChangesAsync();
                return ViewComponent("DetallesDeTransferencia", new { TransferenciaId = detalle.TransferenciaId });
            }
            return BadRequest(ModelState);
        }

        [Authorize(Roles = "Comercial")]
        [HttpPost]
        public async Task<IActionResult> EliminarDetalle(int? id)
        {
            if (id == null)
            {
                return BadRequest(ModelState);
            }
            DetalleDeTransferencia detalle = _context.DetallesDeTransferencia.FirstOrDefault(d => d.Id == id);

            if (detalle == null)
            {
                return BadRequest(ModelState);
            }

            _context.DetallesDeTransferencia.Remove(detalle);

            await _context.SaveChangesAsync();
            return ViewComponent("DetallesDeTransferencia", new { TransferenciaId = detalle.TransferenciaId });


        }


        [Authorize(Roles = "Comercial")]
        // GET: Trasladoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var unidad = _context.UnidadesOrganizativas.Find(user.UnidadOrganizativaId);

            if (id == null)
            {
                return NotFound();
            }

            var transferencia = await _context.Transferencias.FindAsync(id);
            if (transferencia == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Nombre");
            ViewData["DestinoId"] = new SelectList(_context.UnidadesOrganizativas.Where(u => !(u is CasaCompra) && u.Id != unidad.Id), "Id", "Nombre");
            ViewData["Origen"] = unidad.Nombre;
            return View(transferencia);
        }

        // POST: Trasladoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Comercial")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha,OrigenId,DestinoId")] Transferencia transferencia)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var unidad = _context.UnidadesOrganizativas.Find(user.UnidadOrganizativaId);
            transferencia.OrigenId = unidad.Id;
            if (id != transferencia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transferencia);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Editar: " + transferencia.ToString());

                    TempData["exito"] = "Transferencia editada satisfactoriamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransferenciaExists(transferencia.Id))
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
            TempData["error"] = "Se ha producido un error al crear la Transferencia";
            ViewData["DestinoId"] = new SelectList(_context.UnidadesOrganizativas.Where(u => !(u is CasaCompra) && u.Id != unidad.Id), "Id", "Nombre");
            ViewData["Origen"] = unidad.Nombre;
            return View(transferencia);
        }

        [Authorize(Roles = "Comercial")]
        // GET: Trasladoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transferencia = await _context.Transferencias
                .Include(t => t.Destino)
                .Include(t => t.Origen)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transferencia == null)
            {
                return NotFound();
            }

            return View(transferencia);
        }

        // POST: Trasladoes/Delete/5
        [Authorize(Roles = "Comercial")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transferencia = await _context.Transferencias.FindAsync(id);
            _context.Transferencias.Remove(transferencia);
            await _context.SaveChangesAsync();
            TempData["exito"] = "Transferencia eliminada satisfactoriamente";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Comercial")]
        public async Task<IActionResult> Confirmar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transferencia = await _context.Transferencias
                .Include(e => e.Origen).Include(e => e.Destino)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transferencia == null || transferencia.Confirmada)
            {
                return NotFound();
            }

            ViewData["Detalles"] = _context.DetallesDeTransferencia
                                                      .Where(d => d.TransferenciaId == id)
                                                      .Include(p => p.Producto).ThenInclude(p => p.Unidad)
                                                      .ToList();
            return View(transferencia);
        }

        [Authorize(Roles = "Comercial")]
        [HttpPost, ActionName("Confirmar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarConfirmed(int id)
        {
            var transferencia = await _context.Transferencias.FindAsync(id);
            var success = controlSubMayor.ProcesarTraslado(transferencia);
            if (success)
            {
                transferencia.Confirmada = true;
                _context.Transferencias.Update(transferencia);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Crear: " + transferencia.ToString());

                TempData["exito"] = "Transeferencia confirmada satisfactoriamente";

                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Error confirmando la transferencia";

            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult origen(int id)
        {
            var uo = _context.Set<UnidadOrganizativa>().Find(id);
            if (uo is CasaCompra)
            {
                ViewData["DestinoId"] = new SelectList(_context.UEB.ToList(), "Id", "Nombre");
            }
            else
            {
                ViewData["DestinoId"] = new SelectList(_context.UEB.Where(u => u.Id != id), "Id", "Nombre");
            }
            return PartialView("_ListaDestinosPartial");
        }



        private bool TransferenciaExists(int id)
        {
            return _context.Transferencias.Any(e => e.Id == id);
        }
    }
}
