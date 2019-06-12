namespace MateriasPrimasApp.ViewModels
{
    public class ConciliacionVentasVM
    {
        public string Producto { get; set; }
        public string Um { get; set; }
        public decimal VentaMn { get; set; }
        public decimal VentaCuc { get; set; }
        public decimal Mp { get { return VentaMn + VentaCuc; } }
        public decimal AcumuladoMn { get; set; }
        public decimal AcumuladoCuc { get; set; }
        public decimal AcumuladoMp { get { return AcumuladoMn + AcumuladoCuc; } }
    }
}