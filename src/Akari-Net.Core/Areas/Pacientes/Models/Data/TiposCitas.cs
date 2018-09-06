using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Pacientes.Models.Data
{
    public class TipoCita
    {
        [Key]
        public int IdTipoCita { get; set; }

        public string Tipo { get; set; }

        public string Color { get; set; }

        ICollection<CalendarEvent> CalendarEvents { get; set; }
    }
}
