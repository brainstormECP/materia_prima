using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MateriasPrimaApp.Models
{
    public class Producto
    {       
        public int Id { get; set; }
        [Required]
        public string Codigo { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Display(Name = "Descripción")]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }
        [Required]
        public int UnidadId { get; set; }
        public virtual UnidadDeMedida Unidad { get; set; }
        [Required]
        public int CategoriaId { get; set; }
        public virtual Categoria Categoria { get; set; }
        [Required]
        public int TipoId { get; set; }
        public virtual TipoDeProducto Tipo { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrecioVentaMn { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrecioVentaMlc { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrecioCompraMn { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrecioCompraMlc { get; set; }
    }
}