using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MateriasPrimasApp.Models;
using MateriasPrimasApp.Data;
using MateriasPrimasApp.HelperClass;
using Microsoft.AspNetCore.Identity;

namespace MateriasPrimasApp.Controllers
{
    public class VentasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ControlSubMayor controlSubMayor;

        public VentasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            controlSubMayor = new ControlSubMayor(context);
        }

        // GET: Ventas
        public async Task<IActionResult> Index()
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var ventas = _context.Venta.Where(v=>v.UnidadOrganizativaId == user.UnidadOrganizativaId).Include(v => v.Cliente).Include(v => v.UnidadOrganizativa).Include(v=>v.DetallesDeVenta).OrderByDescending(v=>v.Fecha);
            return View(await ventas.ToListAsync());
        }

        public async Task<IActionResult> TodasLasVentas()
        {
            var ventas = _context.Venta.Where(v => !v.Confirmada).Include(v => v.Cliente).Include(v => v.UnidadOrganizativa).Include(v => v.DetallesDeVenta);
            return View( await ventas.ToListAsync());
        }


        // GET: Ventas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Venta
                .Include(v => v.Cliente)
                .Include(v => v.UnidadOrganizativa)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (venta == null)
            {
                return NotFound();
            }
            ViewData["Detalles"] = _context.DetallesDeVenta.Where(d => d.VentaId == id)
                              .Include(p => p.Producto)
                              .ToList(); ;


            return View(venta);
        }

        // GET: Ventas/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Nombre");
            return View();
        }

        // POST: Ventas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fecha,ClienteId")] Venta venta)
        {
            var user = _context.Users.Find(_userManager.GetUserId(User));

            venta.UnidadOrganizativaId = (int)user.UnidadOrganizativaId;

            if (ModelState.IsValid)
            {
                _context.Add(venta);
                await _context.SaveChangesAsync();
                TempData["exito"] = "Venta creada satisfactoriamente";
                return RedirectToAction("DetallesDeVenta", new { id = venta.Id});
            }
            TempData["error"] = "Se ha producido un error al crear la venta";
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Nombre", venta.ClienteId);
            ViewData["UnidadOrganizativaId"] = new SelectList(_context.Set<UnidadOrganizativa>(), "Id", "Nombre", venta.UnidadOrganizativaId);
            return View(venta);
        }

        // GET: Ventas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Venta.FindAsync(id);
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (venta == null || venta.UnidadOrganizativaId != user.UnidadOrganizativaId)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Nombre", venta.ClienteId);
            ViewData["UnidadOrganizativaId"] = new SelectList(_context.Set<UnidadOrganizativa>(), "Id", "Nombre", venta.UnidadOrganizativaId);
            return View(venta);
        }

        // POST: Ventas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha,ClienteId,UnidadOrganizativaId")] Venta venta)
        {
            if (id != venta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venta);
                    await _context.SaveChangesAsync();
                    TempData["exito"] = "Venta editada satisfactoriamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentaExists(venta.Id))
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
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Nombre", venta.ClienteId);
            ViewData["UnidadOrganizativaId"] = new SelectList(_context.Set<UnidadOrganizativa>(), "Id", "Nombre", venta.UnidadOrganizativaId);
            return View(venta);
        }

        // GET: Ventas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Venta
                .Include(v => v.Cliente)
                .Include(v => v.UnidadOrganizativa)
                .FirstOrDefaultAsync(m => m.Id == id);
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (venta == null || venta.UnidadOrganizativaId != user.UnidadOrganizativaId)
            {
                return NotFound();
            }

            return View(venta);
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venta = await _context.Venta.FindAsync(id);
            _context.Venta.Remove(venta);
            await _context.SaveChangesAsync();
            TempData["exito"] = "Venta eliminada satisfactoriamente";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddDetalle(DetalleDeVenta detalle)
        {
            var producto = _context.Producto.Find(detalle.ProductoId);
            var venta = _context.Venta.Find(detalle.VentaId);
            //agregar precio total en base al precio de venta del producto seleccionado * la cantidad de producto
            detalle.PrecioVentaMn = detalle.Cantidad * producto.PrecioVentaMn;
            detalle.PrecioVentaMlc = detalle.Cantidad * producto.PrecioVentaMlc;

            //Controlando excepción en caso de producto duplicado
            if (_context.DetallesDeVenta.Any(d => d.ProductoId == detalle.ProductoId && d.VentaId == detalle.VentaId))
            {
                ModelState.AddModelError("ProductoId", "El producto selecionado ya se encuentra en la venta");
            }

            if (detalle.Cantidad == 0)
            {
                ModelState.AddModelError("Cantidad", "Debe especificar una cantidad para este Producto");
            }


            //Controlar excepción en caso de que la cantidad a vender sea mayor a la existente en el almacén
            var sub = await _context.Submayor.FirstOrDefaultAsync(s => s.AlmacenId == venta.UnidadOrganizativaId && s.ProductoId == detalle.ProductoId);


            if (detalle.Cantidad > sub.Cantidad)
            {
                ModelState.AddModelError("Cantidad", "No existe esa cantidad en el almacén de orígen");
            }

            if (ModelState.IsValid)
            {
                await _context.DetallesDeVenta.AddAsync(detalle);
                await _context.SaveChangesAsync();
                return ViewComponent("DetallesDeVenta", new { VentaId = detalle.VentaId });
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> EliminarDetalle(int? id)
        {
            if (id == null)
            {
                return BadRequest(ModelState);
            }
            DetalleDeVenta detalle = _context.DetallesDeVenta.FirstOrDefault(d => d.Id == id);

            if (detalle == null)
            {
                return BadRequest(ModelState);
            }

            _context.DetallesDeVenta.Remove(detalle);

            await _context.SaveChangesAsync();
            return ViewComponent("DetallesDeVenta", new { VentaId = detalle.VentaId });

        }

        public IActionResult DetallesDeVenta(int id)
        {
            var venta = _context.Venta.Include(e => e.UnidadOrganizativa).Include(e => e.Cliente).FirstOrDefault(e => e.Id == id);
            if (venta == null || venta.Confirmada)
            {
                return NotFound();
            }

            ViewData["Productos"] = new SelectList(_context.Submayor.Include(s=>s.Producto)
                .Include(s=>s.AlmacenId).Where(s=>s.AlmacenId == venta.UnidadOrganizativaId && s.Cantidad >0)
                .Select(s=>s.Producto).ToList(), "Id", "Nombre");
            return View(venta);
        }

        [HttpGet]
        public async Task<IActionResult> Confirmar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Venta
                .Include(e => e.Cliente)
                .Include(e => e.UnidadOrganizativa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venta == null || venta.Confirmada)
            {
                return NotFound();
            }
            var detallesdeventa = _context.DetallesDeVenta
                                                      .Where(d => d.VentaId == id)
                                                      .Include(p => p.Producto).ThenInclude(p=>p.Unidad)
                                                      .ToList();
            decimal total = 0;
            foreach (var i in detallesdeventa)
            {
                total += i.PrecioVentaMn;
            }
            ViewData["TotalMn"] = total;
            ViewData["Detalles"] = detallesdeventa;
            return View(venta);
        }

        [HttpPost, ActionName("Confirmar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarConfirmed(int id)
        {
            var venta = await _context.Venta.FindAsync(id);
            venta.Confirmada = true;
            _context.Venta.Update(venta);
            controlSubMayor.ProcesarVenta(venta);

            await _context.SaveChangesAsync();
            TempData["exito"] = "Venta confirmada satisfactoriamente";
            return RedirectToAction(nameof(Index));
        }




        private bool VentaExists(int id)
        {
            return _context.Venta.Any(e => e.Id == id);
        }
    }
}
