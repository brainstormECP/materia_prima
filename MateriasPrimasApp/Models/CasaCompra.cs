namespace MateriasPrimasApp.Models
{
    public class CasaCompra:UnidadOrganizativa
    {
        public int UebId { get; set; }
        public virtual UEB Ueb { get; set; }
    }
}