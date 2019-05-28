using MateriasPrimaApp.Models;
using MateriasPrimasApp.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MateriasPrimasApp.ViewComponents
{
    [ViewComponent(Name = "Footer")]
    public class FooterViewComponents:ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public FooterViewComponents(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if(user.UnidadOrganizativaId != null)
            {
                var unidad = _context.Set<UnidadOrganizativa>().Find(user.UnidadOrganizativaId);
                ViewData["Unidad"] = unidad.Nombre;
            }
            return View();
        }

    }
}
