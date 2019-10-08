namespace Web.Areas.Pacientes.Models.ViewModels.Facturas
{
    public class FacturacionItem
    {
        public string Mes { get; set; }
        public int Year { get; set; }
        public double TotalBruto { get; set; }
        public double TotalNeto { get; set; }
        public double TotalIrpf { get; set; }
    }
}
