using MateriasPrimasApp.Data;
using MateriasPrimasApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MateriasPrimasApp.ViewComponents
{
    [ViewComponent(Name = "DetallesProcesamiento")]
    public class DetallesProcesamiento: ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public DetallesProcesamiento(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int ProcesamientoId)
        {
            var items = await GetItemsAsync(ProcesamientoId);
            var p = await _context.Procesamientos.FindAsync(ProcesamientoId);
            ViewBag.Merma = p.Merma;
            return View(items);
        }
        private Task<List<DetalleDeProcesamiento>> GetItemsAsync(int id)
        {
            return _context.DetallesDeProcesamiento.Include(x => x.Derivado).Include(x=>x.Procesamiento).Where(x => x.ProcesamientoId == id).ToListAsync();
        }
    }
}
