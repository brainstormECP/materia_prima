using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MateriasPrimaApp.Models
{
    public class DetalleDeTraslado
    {
        public int Id { get; set; }
        [Required]
        public int TrasladoId { get; set; }
        public virtual Traslado Traslado { get; set; }
        [Required]
        public int ProductoId { get; set; }
        public virtual Producto Producto { get; set; }
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