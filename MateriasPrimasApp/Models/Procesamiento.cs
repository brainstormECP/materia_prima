using System.ComponentModel.DataAnnotations;

namespace MateriasPrimaApp.Models
{
    public class Procesamiento
    {
        public int Id { get; set; }
        [Required]
        public int ProductoOrigenId { get; set; }
        public virtual Producto ProductoOrigen { get; set; }
        [Required]
        public decimal CantidadOrigen { get; set; }
        [Required]
        public int ProductoSalidaId { get; set; }

        public virtual Producto ProductoSalida { get; set; }
        [Required]
        public decimal CantidadSalida { get; set; }

        public bool  Confirmada { get; set; }
    }
}