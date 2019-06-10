namespace MateriasPrimasApp.ViewModels
{
    public class ExistenciaVM
    {
        public string Producto { get; set; }
        public decimal EnCasaDeCompra { get; set; }
        public decimal EnUEB { get; set; }
        public decimal EnProceso { get; set; }
        public decimal ListoParaLaVenta { get { return Total - EnProceso; } }
        public decimal Total { get { return EnCasaDeCompra + EnUEB; } }
    }
}