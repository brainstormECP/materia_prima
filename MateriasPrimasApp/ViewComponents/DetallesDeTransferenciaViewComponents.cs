using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MateriasPrimasApp.Data;
using MateriasPrimasApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MateriasPrimasApp.ViewComponents
{
    [ViewComponent(Name = "DetallesDeTransferencia")]
    public class DetallesDeTransferencia:ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public DetallesDeTransferencia(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int TransferenciaId)
        {
            var items = await GetItemsAsync(TransferenciaId);
            decimal total = 0;
            foreach(var i in items)
            {
                total += i.PrecioMn;
            }
            ViewData["Total"] = total;
            return View(items);
        }
        private Task<List<DetalleDeTransferencia>> GetItemsAsync(int id)
        {
            return _context.DetallesDeTransferencia.Include(x=>x.Producto).ThenInclude(p=>p.Unidad).Where(x => x.TransferenciaId == id).ToListAsync();
        }
    }
}
