using System.ComponentModel.DataAnnotations;

namespace MateriasPrimaApp.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        [Required]
        public string Codigo { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string Organismo { get; set; }
    }
}