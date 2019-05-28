using System.ComponentModel.DataAnnotations;

namespace MateriasPrimaApp.Models
{
    public class Submayor
    {
        [Required]
        public int AlmacenId { get; set; }
        public virtual UnidadOrganizativa Almacen { get; set; }
        [Required]
        public int ProductoId { get; set; }
        public virtual Producto Producto { get; set; }
        [Required]        
        public int UnidadId { get; set; }
        public virtual UnidadDeMedida Unidad { get; set; }
        [Required]
        public decimal Cantidad { get; set; }
    }
}