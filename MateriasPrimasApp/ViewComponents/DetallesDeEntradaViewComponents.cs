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
    [ViewComponent(Name ="Detalles")]
    public class DetallesDeEntrada:ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public DetallesDeEntrada(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int EntradaId)
        {
            var items = await GetItemsAsync(EntradaId);
            decimal total = 0;
            foreach(var i in items)
            {
                total += i.PrecioMn;
            }
            ViewData["Total"] = total;
            return View(items);
        }
        private Task<List<DetalleDeEntrada>> GetItemsAsync(int id)
        {
            return _context.DetallesDeEntradas.Include(x=>x.Producto).Where(x => x.EntradaId == id).ToListAsync();
        }
    }
}
