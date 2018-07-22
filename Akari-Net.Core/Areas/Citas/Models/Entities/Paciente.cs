using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Citas.Models.Entities
{
    public class Paciente
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public DateTime Nacimiento { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
    }
}
