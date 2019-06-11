using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MateriasPrimasApp.Models
{
    public class Entrada
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "El campo Fecha es obligatorio")]
        public DateTime Fecha { get; set; }
        [Display(Name="Cliente")]
        [Required(ErrorMessage = "El campo Cliente es obligatorio")]
        public int ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; }
        [Display(Name="Unidad Organizativa")]
        public int UnidadOrganizativaId { get; set; }
        public virtual UnidadOrganizativa UnidadOrganizativa { get; set; }
        public bool Confirmada { get; set; }

        public virtual ICollection<DetalleDeEntrada> DetallesDeEntrada { get; set; }
        public override string ToString()
        {
            return $"Entrada Fecha:{Fecha.ToShortDateString()}, UnidadOrganizativaId:{UnidadOrganizativaId}, ClienteId: {ClienteId}, Detalles: " + String.Join(",", DetallesDeEntrada.Select(d => "ProductoId: " + d.ProductoId + " Cantidad: "+ d.Cantidad));
        }
    }
}