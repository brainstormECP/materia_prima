using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MateriasPrimasApp.Models
{
    public class DetalleDeEntrada
    {
        public int Id { get; set; }
        [Required]
        public int EntradaId { get; set; }
        public virtual Entrada Entrada { get; set; }
        [Required]
        public int ProductoId { get; set; }
        public virtual Producto Producto { get; set; }
        [Required(ErrorMessage = "Debe ingresar una Cantidad")]
        public decimal Cantidad { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrecioMn { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrecioMlc { get; set; }
    }
}