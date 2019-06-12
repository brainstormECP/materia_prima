using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MateriasPrimasApp.Models
{
    public class Transferencia
    {
        public int Id { get; set; }
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

        public Transferencia()
        {
            DetallesDeTransferencia = new HashSet<DetalleDeTransferencia>();
        }

        public override string ToString()
        {
            return $"Transferencia Fecha:{Fecha.ToShortDateString()}, UnidadOrganizativaOrigenId:{OrigenId}, UnidadOrganizativaDestinoId: {DestinoId}, Detalles: " + String.Join(",", DetallesDeTransferencia.Select(d => "ProductoId: " + d.ProductoId + " Cantidad: " + d.Cantidad));
        }

    }
}