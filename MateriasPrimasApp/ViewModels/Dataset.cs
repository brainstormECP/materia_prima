using System.Collections.Generic;

namespace MateriasPrimasApp.ViewModels
{
    public class Dataset
    {
        public string Label { get; set; }
        public string BackgroundColor { get; set; }
        public string BorderColor { get; set; }
        public List<decimal> Data { get; set; }
        public bool Fill { get; set; }
        public Dataset()
        {
            Data = new List<decimal>();
        }

    }
}