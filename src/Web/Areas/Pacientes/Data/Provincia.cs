using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Akari_Net.Core.Areas.Pacientes.Models.Data
{
    [Table("Provincias", Schema = "Patients")]
    public class Provincia
    {
        [Key]
        public int IdProvincia { get; set; }

        public string Nombre { get; set; }

        public virtual ICollection<Paciente> Pacientes { get; set; }
    }
}