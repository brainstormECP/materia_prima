using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MateriasPrimasApp.Models;
using MateriasPrimasApp.Data;
using Microsoft.VisualStudio.Web.CodeGeneration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace MateriasPrimasApp.Controllers
{
    [Authorize(Roles = "Administrador, Consultor")]
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductosController> _logger;

        public ProductosController(ApplicationDbContext context, ILogger<ProductosController>logger)
        {
            _logger = logger;
            _context = context;
        }

        [Authorize(Roles = "Administrador, Consultor")]
        // GET: Productoes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Producto.Include(p => p.Categoria).Include(p => p.Tipo).Include(p => p.Unidad);
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize(Roles = "Administrador, Consultor")]
        // GET: Productoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto
                .Include(p => p.Categoria)
                .Include(p => p.Tipo)
                .Include(p => p.Unidad)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        [Authorize(Roles = "Administrador")]
        // GET: Productoes/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nombre");
            ViewData["TipoId"] = new SelectList(_context.Set<TipoDeProducto>(), "Id", "Nombre");
            ViewData["UnidadId"] = new SelectList(_context.UM, "Id", "Unidad");
            return View();
        }

        // POST: Productoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codigo,Nombre,Descripcion,UnidadId,CategoriaId,TipoId,PrecioCompraMn,PrecioVentaMn,PrecioCompraMlc,PrecioVentaMlc")] Producto producto)
        {
            if (await _context.Producto.AnyAsync(p => p.Codigo == producto.Codigo))
            {
                ModelState.AddModelError("Codigo", "ya existe un Producto con este código");
            }
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                //creando un Submayor de este producto en cada Unidad
                var unidades = _context.UnidadesOrganizativas;
                foreach (var unidad in unidades)
                {
                    var submayor = new Submayor() { AlmacenId = unidad.Id, ProductoId = producto.Id, Cantidad = 0, UnidadId = producto.UnidadId };
                    _context.Add(submayor);                      
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nombre", producto.CategoriaId);
            ViewData["TipoId"] = new SelectList(_context.Set<TipoDeProducto>(), "Id", "Nombre", producto.TipoId);
            ViewData["UnidadId"] = new SelectList(_context.UM, "Id", "Unidad", producto.UnidadId);
            return View(producto);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nombre", producto.CategoriaId);
            ViewData["TipoId"] = new SelectList(_context.Set<TipoDeProducto>(), "Id", "Nombre", producto.TipoId);
            ViewData["UnidadId"] = new SelectList(_context.UM, "Id", "Unidad", producto.UnidadId);
            return View(producto);
        }

        // POST: Productoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,Nombre,Descripcion,UnidadId,CategoriaId,TipoId,PrecioCompraMn,PrecioVentaMn,PrecioCompraMlc,PrecioVentaMlc")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nombre", producto.CategoriaId);
            ViewData["TipoId"] = new SelectList(_context.Set<TipoDeProducto>(), "Id", "Nombre", producto.TipoId);
            ViewData["UnidadId"] = new SelectList(_context.UM, "Id", "Unidad", producto.UnidadId);
            return View(producto);
        }

        // GET: Productoes/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto
                .Include(p => p.Categoria)
                .Include(p => p.Tipo)
                .Include(p => p.Unidad)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productoes/Delete/5
        [Authorize(Roles = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Producto.FindAsync(id);
            _context.Producto.Remove(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public ActionResult um(int id)
        {
            if(id != 0)
            {
                var producto = _context.Producto.Include(p => p.Unidad).FirstOrDefault(p => p.Id == id);
                ViewData["Um"] = producto.Unidad.Unidad;
            }
            else
            {
                ViewData["Um"] = "";
            }

            return PartialView("_UMProductoPartial");
        }

        private bool ProductoExists(int id)
        {
            return _context.Producto.Any(e => e.Id == id);
        }
    }
}
