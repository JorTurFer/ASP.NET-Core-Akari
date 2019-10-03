using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Akari_Net.Core.Areas.Pacientes.Models.Data
{
    [Table("TipoCitas", Schema = "Patients")]
    public class TipoCita
    {
        [Key]
        public int IdTipoCita { get; set; }

        public string Tipo { get; set; }

        public string Color { get; set; }
    }
}