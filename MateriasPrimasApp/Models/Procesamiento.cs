using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MateriasPrimasApp.Models
{
    public class Procesamiento
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "El campo Fecha es obligatorio")]
        [Remote("CheckFecha", "Remote", AdditionalFields = "Id")]
        public DateTime Fecha { get; set; }

        [Display(Name = "Unidad Organizativa")]
        public int UnidadOrganizativaId { get; set; }
        public virtual UnidadOrganizativa UnidadOrganizativa { get; set; }

        [Display(Name = "Materia Prima")]
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }

        [Required(ErrorMessage = "El campo Cantidad es requerido")]
        public decimal Cantidad { get; set; }

        public decimal Merma { get; set; } = 0; 

        public bool Confirmado { get; set; }

        public virtual ICollection<DetalleDeProcesamiento> DetallesDeProcesamiento { get; set; }
    }
}
