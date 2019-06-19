using MateriasPrimasApp.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace MateriasPrimasApp.Controllers
{
    public class RemoteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RemoteController(ApplicationDbContext context)
        {
            _context = context;
        }

        public JsonResult CheckFecha(string fecha, int? Id)
        {
            try
            {
                if(Id == 0 || Id == null)
                {
                    DateTime dateTime = DateTime.Parse(fecha);

                    if (dateTime < DateTime.Now.AddDays(-4))
                    {
                        return Json(data: $"Solo se permite una fecha con 4 días de antelación");
                    }
                    if (dateTime > DateTime.Now)
                    {
                        return Json(data: $"No se permite una fecha mayor al día actual");
                    }
                    return Json(true);
                }
                return Json(true);

            }
            catch (Exception)
            {

                return Json(true);
            }
        }
    }
}