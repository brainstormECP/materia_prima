using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MateriasPrimaApp.Models
{
    public class DetalleDeVenta
    {
        public int Id { get; set; }
        [Required]
        public int VentaId { get; set; }
        public virtual Venta Venta { get; set; }
        [Required]
        public int ProductoId { get; set; }
        public virtual Producto Producto { get; set; }
        [Required]
        public int UnidadId { get; set; }
        public virtual UnidadDeMedida Unidad { get; set; }
        [Required]
        public decimal Cantidad { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrecioMn { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrecioMlc { get; set; }
    }
}