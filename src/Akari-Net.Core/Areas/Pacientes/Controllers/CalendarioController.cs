using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akari_Net.Core.Areas.Pacientes.Models.Entities;
using Akari_Net.Core.Areas.Pacientes.Models.Services;
using Akari_Net.Core.Areas.Pacientes.Models.ViewModels.Calendario;
using Akari_Net.Core.Areas.Usuarios.Models.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Akari_Net.Core.Areas.Pacientes.Controllers
{
    [Area("Pacientes")]
    [AuthorizePolicy(Policy = "GetCalendarEvents", Description = "Visualización del Calendario")]
    [Route("[area]/[controller]/[action]")]
    public class CalendarioController : Controller
    {
        private readonly ICalendarioServices _calendarioServices;

        public CalendarioController(ICalendarioServices calendarioServices)
        {
            _calendarioServices = calendarioServices;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetEvents(DateTime Date , string Type)
        {
            List<CalendarEvent> events = await _calendarioServices.GetCalendarEventsAsync(Date,Type);           
            return new JsonResult(events);
        }

        [HttpPost]
        [AuthorizePolicy(Policy = "SaveCalendarEvents", Description = "Permitir registrar en calendario")]
        public async Task<JsonResult> SaveEvent(CalendarEvent e)
        {
            var status = (await _calendarioServices.SaveEventAsync(e)) == 1;
            return Json(status);
        }

        [HttpPost]
        [AuthorizePolicy(Policy = "SaveCalendarEvents", Description = "Permitir registrar en calendario")]
        public async Task<JsonResult> GetPatientNames(string Nombre)
        {
            //Para evitar sobrecarga, solo busco si se han escrito 4 o mas letras
            if (Nombre.Length > 2)
            {
                var pacientes = await _calendarioServices.GetPatientNamesAsync(Nombre);
                return Json(pacientes);
            }
            else
                return Json("");
        }

        [HttpPost]
        [AuthorizePolicy(Policy = "DeleteCalendarEvents", Description = "Permitir borrar del calendario")]
        public async Task<JsonResult> DeleteEvent(int eventID)
        {
            var status = (await _calendarioServices.DeleteEventAsync(eventID)) == 1;
            return Json(status);
        }
    }
}