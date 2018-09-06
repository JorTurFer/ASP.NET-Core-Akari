using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Pacientes.Models.Data
{
    [Table("Paises")]
    public class Pais
    {
        [Key]
        public int IdPais { get; set; }

        public string Nombre { get; set; }

        public ICollection<Paciente> Pacientes { get; set; }

    }
}
