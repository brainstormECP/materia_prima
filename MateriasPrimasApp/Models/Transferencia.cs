using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MateriasPrimaApp.Models
{
    public class Transferencia
    {
        public int Id { get; set; }
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "El campo Fecha es obligatorio")]
        public DateTime Fecha { get; set; }
        public int OrigenId { get; set; }
        public virtual UnidadOrganizativa Origen { get; set; }
        [Required]
        public int DestinoId { get; set; }
        public virtual UnidadOrganizativa Destino { get; set; }
        public bool Confirmada { get; set; }

        public virtual ICollection<DetalleDeTransferencia> DetallesDeTransferencia { get; set; }
    }
}