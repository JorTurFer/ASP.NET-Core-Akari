using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Pacientes.Data
{
    [Table("Ciudades", Schema = "Facturas")]
    public class Ciudad
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string Localidad { get; set; }
        [Required]
        public string Comunidad { get; set; }
    }
}
