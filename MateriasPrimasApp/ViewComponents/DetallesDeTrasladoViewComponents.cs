using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MateriasPrimaApp.Models;
using MateriasPrimasApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MateriasPrimasApp.ViewComponents
{
    [ViewComponent(Name = "DetallesDeTraslado")]
    public class DetallesDeTraslado:ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public DetallesDeTraslado(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int TrasladoId)
        {
            var items = await GetItemsAsync(TrasladoId);
            decimal total = 0;
            foreach(var i in items)
            {
                total += i.PrecioMn;
            }
            ViewData["Total"] = total;
            return View(items);
        }
        private Task<List<DetalleDeTraslado>> GetItemsAsync(int id)
        {
            return _context.DetallesDeTraslado.Include(x=>x.Producto).Where(x => x.TrasladoId == id).ToListAsync();
        }
    }
}
