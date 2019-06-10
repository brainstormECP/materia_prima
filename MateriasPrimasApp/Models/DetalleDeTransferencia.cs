using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MateriasPrimasApp.Models
{
    public class DetalleDeTransferencia
    {
        public int Id { get; set; }
        [Required]
        public int TransferenciaId { get; set; }
        public virtual Transferencia Transferencia { get; set; }
        [Required]
        public int ProductoId { get; set; }
        public virtual Producto Producto { get; set; }
        [Required(ErrorMessage ="El campo Cantidad es requerido")]
        public decimal Cantidad { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrecioMn { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrecioMlc { get; set; }
    }
}