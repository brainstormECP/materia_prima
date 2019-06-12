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
    public class ProcesamientosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ControlSubMayor controlSubMayor;

        public ProcesamientosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            controlSubMayor = new ControlSubMayor(context);

        }

        // GET: Procesamientos
        public async Task<IActionResult> Index()
        {
            var user = _context.Users.Find(_userManager.GetUserId(User));
            var applicationDbContext = _context.Procesamientos.Where(p => p.UnidadOrganizativaId == user.UnidadOrganizativaId).Include(p => p.UnidadOrganizativa).Include(p => p.Producto).ThenInclude(p => p.Unidad).Include(p => p.DetallesDeProcesamiento).OrderByDescending(p=>p.Fecha);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Procesamientos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var procesamiento = await _context.Procesamientos
     .Include(p => p.Producto).ThenInclude(p => p.Unidad)
     .Include(p => p.UnidadOrganizativa)
     .FirstOrDefaultAsync(p => p.Id == id);
           
            var detallesdeprocesamiento = _context.DetallesDeProcesamiento
                                                      .Where(d => d.ProcesamientoId == id)
                                                      .Include(p => p.Derivado).ThenInclude(p => p.Unidad)
                                                      .ToList();

            ViewData["Detalles"] = detallesdeprocesamiento;


            return View(procesamiento);
        }

        // GET: Procesamientos/Create
        public async Task<IActionResult> Create()
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            var productos = await _context.Submayor.Include(s => s.Producto)
                .Where(s => s.AlmacenId == user.UnidadOrganizativaId)
                .Select(s => s.Producto).Where(p => !(p is Derivado)).Include(p => p.Derivados)
                .Where(p => p.Derivados.Count > 0).ToListAsync();

            ViewData["Productos"] = new SelectList(productos, "Id", "Nombre");
            return View();
        }

        // POST: Procesamientos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductoId, Cantidad")] Procesamiento procesamiento)
        {
            var user = _context.Users.Find(_userManager.GetUserId(User));
            procesamiento.Fecha = DateTime.Now;
            procesamiento.UnidadOrganizativaId = (int)user.UnidadOrganizativaId;

            var existencia = _context.Submayor.FirstOrDefault(s => s.ProductoId == procesamiento.ProductoId && s.AlmacenId == procesamiento.UnidadOrganizativaId);
            if (procesamiento.Cantidad == 0)
            {
                ModelState.AddModelError("Cantidad", "Debe especificar una cantidad");
            }
            else if (existencia.Cantidad < procesamiento.Cantidad)
            {
                ModelState.AddModelError("Cantidad", "No se dispone de esta cantidad para procesar");
            }


            if (ModelState.IsValid)
            {
                _context.Add(procesamiento);
                await _context.SaveChangesAsync();
                return RedirectToAction("DetallesDeProcesamiento", new { id = procesamiento.Id });
            }
            var productos = await _context.Submayor.Include(s => s.Producto)
                .Include(s => s.AlmacenId).Where(s => s.AlmacenId == user.UnidadOrganizativaId)
                .Select(s => s.Producto).Where(p => !(p is Derivado)).Include(p => p.Derivados)
                .Where(p => p.Derivados.Count > 0).ToListAsync();

            ViewData["Productos"] = new SelectList(productos, "Id", "Nombre", procesamiento.ProductoId);
            return View(procesamiento);
        }

        [HttpGet]
        public async Task<IActionResult> DetallesDeProcesamiento(int id)
        {
            var procesamiento = await _context.Procesamientos.Include(p => p.Producto).ThenInclude(p => p.Unidad).FirstOrDefaultAsync(p => p.Id == id);
            var derivados = await _context.Derivados.Where(d => d.ProductoOrigenId == procesamiento.ProductoId).ToListAsync();

            ViewData["Derivados"] = new SelectList(await _context.Derivados.Where(d => d.ProductoOrigenId == procesamiento.ProductoId).ToListAsync(), "Id", "Nombre");
            ViewData["UnidadesDeMedidas"] = new SelectList(_context.UM.ToList(), "Id", "Unidad");

            return View(procesamiento);
        }

        // GET: Procesamientos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var procesamiento = await _context.Procesamientos.FindAsync(id);
            if (procesamiento == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            var productos = await _context.Submayor.Include(s => s.Producto)
                .Include(s => s.AlmacenId).Where(s => s.AlmacenId == user.UnidadOrganizativaId)
                .Select(s => s.Producto).Where(p => !(p is Derivado)).Include(p => p.Derivados)
                .Where(p => p.Derivados.Count > 0).ToListAsync();

            ViewData["Productos"] = new SelectList(productos, "Id", "Nombre", procesamiento.ProductoId);

            return View(procesamiento);
        }

        // POST: Procesamientos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductoId, Cantidad")] Procesamiento procesamiento)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            var proc = await _context.Procesamientos.Include(p => p.DetallesDeProcesamiento).FirstOrDefaultAsync(p => p.Id == id);
            if (procesamiento.ProductoId != proc.ProductoId && proc.DetallesDeProcesamiento.Count() > 0)
            {
                ModelState.AddModelError("ProductoId", "Este procesamiento contiene detalles que pertenecen al producto anterior. Para poder cambiar el producto debe eliminar los detalles");
            }

            if (id != procesamiento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(procesamiento);
                TempData["exito"] = "Procesamiento editado satisfactoriamente";

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var productos = await _context.Submayor.Include(s => s.Producto)
                .Include(s => s.AlmacenId).Where(s => s.AlmacenId == user.UnidadOrganizativaId)
                .Select(s => s.Producto).Where(p => !(p is Derivado)).Include(p => p.Derivados)
                .Where(p => p.Derivados.Count > 0).ToListAsync();

            ViewData["Productos"] = new SelectList(productos, "Id", "Nombre");
            TempData["error"] = "Error al editar este procesamiento";

            return View(procesamiento);
        }

        // GET: Procesamientos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var procesamiento = await _context.Procesamientos
                .Include(p => p.UnidadOrganizativa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (procesamiento == null)
            {
                return NotFound();
            }

            return View(procesamiento);
        }

        // POST: Procesamientos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var procesamiento = await _context.Procesamientos.FindAsync(id);
            _context.Procesamientos.Remove(procesamiento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Comercial")]
        [HttpPost]
        public async Task<IActionResult> AddDetalle(DetalleDeProcesamiento detalle)
        {
            var procesamiento = await _context.Procesamientos.Include(p => p.DetallesDeProcesamiento).FirstOrDefaultAsync(p => p.Id == detalle.ProcesamientoId);
            decimal sumatoria = 0;
            //Comprobando si ya existe un detalle
            if (await _context.DetallesDeProcesamiento.AnyAsync(d => d.DerivadoId == detalle.DerivadoId && d.ProcesamientoId == detalle.ProcesamientoId))
            {
                ModelState.AddModelError("ProductoId", "El producto selecionado ya se encuentra en la entrada");
            }
            if (detalle.Cantidad == 0)
            {
                ModelState.AddModelError("Cantidad", "Debe especificar una cantidad para este Producto");
            }

            foreach (var d in procesamiento.DetallesDeProcesamiento)
            {
                sumatoria += d.Cantidad;
            }
            if (sumatoria + detalle.Cantidad > procesamiento.Cantidad)
            {
                ModelState.AddModelError("Cantidad", "La cantidad indicada es mayor que la cantidad disponible para procesar");
            }
            else
            {
                procesamiento.Merma = procesamiento.Cantidad - (sumatoria + detalle.Cantidad);
                if (ModelState.IsValid)
                {
                    await _context.DetallesDeProcesamiento.AddAsync(detalle);
                    _context.Update(procesamiento);
                    await _context.SaveChangesAsync();
                    return ViewComponent("DetallesProcesamiento", new { ProcesamientoId = detalle.ProcesamientoId });
                }

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
            DetalleDeProcesamiento detalle = await _context.DetallesDeProcesamiento.FirstOrDefaultAsync(d => d.Id == id);
            Procesamiento procesamiento = await _context.Procesamientos.FindAsync(detalle.ProcesamientoId);
            procesamiento.Merma += detalle.Cantidad;
            _context.Update(procesamiento);

            if (detalle == null)
            {
                return BadRequest(ModelState);
            }

            _context.DetallesDeProcesamiento.Remove(detalle);

            await _context.SaveChangesAsync();
            return ViewComponent("DetallesProcesamiento", new { ProcesamientoId = detalle.ProcesamientoId });


        }

        [HttpGet]
        public async Task<IActionResult> Confirmar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var procesamiento = await _context.Procesamientos
                .Include(p => p.Producto).ThenInclude(p => p.Unidad)
                .Include(p => p.UnidadOrganizativa)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (procesamiento == null || procesamiento.Confirmado)
            {
                return NotFound();
            }
            var detallesdeprocesamiento = _context.DetallesDeProcesamiento
                                                      .Where(d => d.ProcesamientoId == id)
                                                      .Include(p => p.Derivado).ThenInclude(p => p.Unidad)
                                                      .ToList();

            ViewData["Detalles"] = detallesdeprocesamiento;
            return View(procesamiento);
        }

        [HttpPost, ActionName("Confirmar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarConfirmed(int id)
        {
            var procesamiento = await _context.Procesamientos.Include(p => p.DetallesDeProcesamiento).FirstOrDefaultAsync(p => p.Id == id);

            procesamiento.Confirmado = true;
            controlSubMayor.ProcesarProcesamiento(procesamiento);

            _context.Procesamientos.Update(procesamiento);

            await _context.SaveChangesAsync();
            TempData["exito"] = "Procesamiento confirmado satisfactoriamente. Se han actualizado los submayores";
            return RedirectToAction(nameof(Index));
        }


        private bool ProcesamientoExists(int id)
        {
            return _context.Procesamientos.Any(e => e.Id == id);
        }
    }
}
