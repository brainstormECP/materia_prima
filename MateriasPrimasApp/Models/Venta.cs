using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MateriasPrimasApp.Models
{
    public class Venta
    {
        public int Id { get; set; }
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "El campo Fecha es obligatorio")]
        public DateTime Fecha { get; set; }
        [Required]
        public int ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; }
        [Required]
        public int UnidadOrganizativaId { get; set; }
        public virtual UnidadOrganizativa UnidadOrganizativa { get; set; }
        public bool Confirmada { get; set; }

        public virtual ICollection<DetalleDeVenta> DetallesDeVenta { get; set; }
    }
}