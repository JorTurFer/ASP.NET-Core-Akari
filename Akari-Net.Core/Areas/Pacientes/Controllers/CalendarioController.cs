using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akari_Net.Core.Areas.Pacientes.Models.Entities;
using Akari_Net.Core.Areas.Pacientes.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Akari_Net.Core.Areas.Pacientes.Controllers
{
    [Area("Pacientes")]
    [Authorize(Policy = "CalendarManager")]
    [Route("[area]/[controller]/[action]")]
    public class CalendarioController : Controller
    {
        private readonly PacientesDbContext _context;

        public CalendarioController(PacientesDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetEvents()
        {
            var events = _context.CalendarEvents.ToList();
            var test = new CalendarEvent
            {
                EventID = 1,
                IdPaciente = null,
                Start =DateTime.Now,
                End = DateTime.Now.AddHours(2),
                IsFullDay = false,
                Subject = "Cabecera",
            };
            events.Add(test);
            return new JsonResult(events);
        }
    }
}