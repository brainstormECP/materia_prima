using System.ComponentModel.DataAnnotations;

namespace MateriasPrimaApp.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Código es obligatorio")]
        public string Codigo { get; set; }
        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Nombre { get; set; }
        public string Organismo { get; set; }
    }
}