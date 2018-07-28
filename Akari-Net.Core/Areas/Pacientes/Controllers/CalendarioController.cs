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
            return new JsonResult(events);
        }

        [HttpPost]
        public JsonResult SaveEvent(CalendarEvent e)
        {
            var status = false;
            if (e.EventID > 0)
            {
                //Update the event
                var v = _context.CalendarEvents.Where(a => a.EventID == e.EventID).FirstOrDefault();
                if (v != null)
                {
                    v.Subject = e.Subject;
                    v.Start = e.Start;
                    v.End = e.End;
                    v.Description = e.Description;
                    v.IsFullDay = e.IsFullDay;
                    v.ThemeColor = e.ThemeColor;
                    v.IdPaciente = e.IdPaciente;
                }
            }
            else
            {
                _context.CalendarEvents.Add(e);
            }
            _context.SaveChanges();
            status = true;
            return Json(status);
        }

        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;

            var v = _context.CalendarEvents.Where(a => a.EventID == eventID).FirstOrDefault();
            if (v != null)
            {
                _context.CalendarEvents.Remove(v);
                _context.SaveChanges();
                status = true;
            }

            return Json(status);
        }
    }
}