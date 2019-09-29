using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Facturas.Data
{
    public class Referencia
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        public string Identificador { get; set; }
        [Required]
        public string Concepto { get; set; }
        [Required]
        public double Precio { get; set; }
    }
}
