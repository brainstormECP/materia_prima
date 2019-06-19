using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MateriasPrimasApp.Models
{
    public class DetalleDeVenta
    {
        public int Id { get; set; }
        [Required]
        public int VentaId { get; set; }
        public virtual Venta Venta { get; set; }
        [Required(ErrorMessage = "El campo Producto es requerido")]
        [Display(Name = "Producto")]
        public int ProductoId { get; set; }
        public virtual Producto Producto { get; set; }
        [Required(ErrorMessage = "El campo Cantidad es requerido")]
        public decimal Cantidad { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrecioVentaMn { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrecioVentaMlc { get; set; }
    }
}