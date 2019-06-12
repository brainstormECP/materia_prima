using System.ComponentModel.DataAnnotations;

namespace MateriasPrimasApp.ViewModels
{
    public class ParametroVentasVM
    {
        public int Ueb { get; set; }
        [Required(ErrorMessage = "El campo Mes es obligatorio")]
        public int Mes { get; set; }
        [Required(ErrorMessage = "El campo Año es obligatorio")]
        public int Año { get; set; }
    }
}