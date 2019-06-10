using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MateriasPrimasApp.Models
{
    public class UnidadDeMedida
    {
        public int Id { get; set; }
        [Required]
        public string Unidad { get; set; }
        [Display(Name ="Descripci�n")]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }        
    }
}