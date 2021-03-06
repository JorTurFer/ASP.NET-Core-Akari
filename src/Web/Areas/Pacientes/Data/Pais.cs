﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Akari_Net.Core.Areas.Pacientes.Models.Data
{
    [Table("Paises", Schema = "Patients")]
    public class Pais
    {
        [Key]
        public int IdPais { get; set; }

        public string Nombre { get; set; }

        public virtual ICollection<Paciente> Pacientes { get; set; }
    }
}