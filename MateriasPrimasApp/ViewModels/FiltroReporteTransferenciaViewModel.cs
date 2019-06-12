using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MateriasPrimasApp.ViewModels
{
    public class FiltroReporteTransferenciaViewModel
    {
        [DataType(DataType.Date)]
        public DateTime? FechaInicio { get; set; }
        [DataType(DataType.Date)]
        public DateTime? FechaFin { get; set; }
        [Display(Name ="Producto")]
        public int? ProductoId { get; set; }
        [Display(Name = "Origen")]
        public int? OrigenId { get; set; }
        [Display(Name = "Destino")]
        public int? DestinoId { get; set; }
    }
}
