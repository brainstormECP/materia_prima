using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MateriasPrimasApp.Models
{
    public class DetalleDeProcesamiento
    {
        public int Id { get; set; }
        [Required]
        public int ProcesamientoId { get; set; }
        public int DerivadoId { get; set; }
        [Required(ErrorMessage = "Debe ingresar una Cantidad")]
        public decimal Cantidad { get; set; }
        public Procesamiento Procesamiento { get; set; }
        public Derivado Derivado { get; set; }
    }
}
