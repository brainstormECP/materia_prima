using MateriasPrimaApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MateriasPrimasApp.Models
{
    public class EntradaMateriaPrimaViewModel
    {
        [Required]
        public DateTime Fecha { get; set; }
        [Required]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }
        [Required]
        [Display(Name = "Casa de Compra")]
        public int UnidadOrganizativaId { get; set; }
        public List<DetalleDeEntrada> DetallesDeEntradas { get; set; }
    }
}
