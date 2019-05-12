using System;
using System.ComponentModel.DataAnnotations;

namespace MateriasPrimaApp.Models
{
    public class Entrada
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public DateTime Fecha { get; set; }
        [Required]
        public int ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; }
        public int UnidadOrganizativaId { get; set; }
        public virtual UnidadOrganizativa UnidadOrganizativa { get; set; }
        public bool Confirmada { get; set; }
    }
}