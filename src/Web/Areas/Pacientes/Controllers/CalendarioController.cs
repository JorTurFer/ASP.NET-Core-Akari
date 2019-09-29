using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Akari_Net.Core.Areas.Pacientes.Hubs;
using Akari_Net.Core.Areas.Pacientes.Models.Data;
using Akari_Net.Core.Areas.Pacientes.Models.Services;
using AspNetCore.Identity.ByPermissions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Akari_Net.Core.Areas.Pacientes.Controllers
{
    [Area("Pacientes")]
    [Permission("GetCalendarEvents",  "Visualización del Calendario")]
    [Route("[area]/[controller]/[action]")]
    public class CalendarioController : Controller
    {
        private readonly ICalendarioServices _calendarioServices;
        private readonly IHubContext<CalendarioHub> _calendarHubContext;
        public CalendarioController(ICalendarioServices calendarioServices, IHubContext<CalendarioHub> calendarHubContext)
        {
            _calendarioServices = calendarioServices;
            _calendarHubContext = calendarHubContext;
        }
        
        public async Task<IActionResult> Index()
        {
            var vm = await _calendarioServices.GetCalendarioViewModelAsync();
            return View(vm);
        }

        public async Task<JsonResult> GetEvents(DateTime Date , string Type)
        {
            List<CalendarEvent> events = await _calendarioServices.GetCalendarEventsAsync(Date,Type);           
            return new JsonResult(events);
        }

        [HttpPost]
        [Permission("SaveCalendarEvents", "Permitir registrar en calendario")]
        public async Task<JsonResult> SaveEvent(CalendarEvent e,string color)
        {
            e.IdTipoCita = (await _calendarioServices.FindTipoCitaByColorAsync(color)).IdTipoCita;
            var status = (await _calendarioServices.SaveEventAsync(e)) == 1;
            if (status)
                await _calendarHubContext.Clients.All.SendAsync("updateCalendar");
            return Json(status);
        }

        [HttpPost]
        [Permission("SaveCalendarEvents", "Permitir registrar en calendario")]
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
        [Permission( "DeleteCalendarEvents",  "Permitir borrar del calendario")]
        public async Task<JsonResult> DeleteEvent(int eventID)
        {
            var status = (await _calendarioServices.DeleteEventAsync(eventID)) == 1;
            if (status)
                await _calendarHubContext.Clients.All.SendAsync("updateCalendar");
            return Json(status);
        }
    }
}