using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MateriasPrimasApp.Models;
using MateriasPrimasApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MateriasPrimasApp.ViewComponents
{
    [ViewComponent(Name ="DetallesDeVenta")]
    public class DetallesDeVenta:ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public DetallesDeVenta(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int VentaId)
        {
            var items = await GetItemsAsync(VentaId);
            decimal total = 0;
            foreach(var i in items)
            {
                total += i.PrecioVentaMn;
            }
            ViewData["Total"] = total;
            return View(items);
        }
        private Task<List<DetalleDeVenta>> GetItemsAsync(int id)
        {
            return _context.DetallesDeVenta.Include(x=>x.Producto).Where(x => x.VentaId == id).ToListAsync();
        }
    }
}
