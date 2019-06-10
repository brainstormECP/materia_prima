using System.ComponentModel.DataAnnotations;

namespace MateriasPrimasApp.Models
{
    public class UnidadOrganizativa
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string Telefono { get; set; }
    }
}