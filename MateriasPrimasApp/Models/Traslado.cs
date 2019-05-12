using System;
using System.ComponentModel.DataAnnotations;

namespace MateriasPrimaApp.Models
{
    public class Traslado
    {
        public int Id { get; set; }
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }
        [Required]
        public int ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; }
        [Required]
        public int OrigenId { get; set; }
        public virtual UnidadOrganizativa Origen { get; set; }
        [Required]
        public int DestinoId { get; set; }
        public virtual UnidadOrganizativa Destino { get; set; }
        public bool Confirmada { get; set; }
    }
}