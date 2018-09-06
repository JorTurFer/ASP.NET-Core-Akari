using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akari_Net.Core.Areas.Pacientes.Models.Data;
using Akari_Net.Core.Areas.Pacientes.Models.Entities;
using Akari_Net.Core.Areas.Pacientes.Models.ViewModels.Calendario;
using Microsoft.EntityFrameworkCore;

namespace Akari_Net.Core.Areas.Pacientes.Models.Services
{
    public class CalendarioServices : ICalendarioServices
    {
        private readonly PacientesDbContext _context;

        public CalendarioServices(PacientesDbContext context)
        {
            _context = context;
        }

        public int DeleteEvent(int eventID)
        {
            var v = _context.CalendarEvents.Where(a => a.EventID == eventID).FirstOrDefault();
            if (v != null)
            {
                _context.CalendarEvents.Remove(v);
                return _context.SaveChanges();
            }
            return 0;
        }

        public Task<int> DeleteEventAsync(int eventID)
        {
            var v = _context.CalendarEvents.Where(a => a.EventID == eventID).FirstOrDefault();
            if (v != null)
            {
                _context.CalendarEvents.Remove(v);
                return _context.SaveChangesAsync();
            }
            return new Task<int>(() => { return 0; });
        }

        public List<CalendarEvent> GetCalendarEvents(DateTime Date, string Type)
        {
            List<CalendarEvent> events;
            if (Type.ToLower().Contains("month"))
            {
                events = _context.CalendarEvents.Where(x => x.Start.Date > Date.Date.AddMonths(-1) && x.Start.Date < Date.Date.AddMonths(1)).ToList();
            }
            else if (Type.ToLower().Contains("week"))
            {
                events = _context.CalendarEvents.Where(x => x.Start.Date > Date.Date.AddDays(-7) && x.Start.Date < Date.Date.AddMonths(7)).ToList();
            }
            else
            {
                events = _context.CalendarEvents.Where(x => x.Start.Date == Date.Date).ToList();
            }
            return events;
        }

        public Task<List<CalendarEvent>> GetCalendarEventsAsync(DateTime Date, string Type)
        {
            Task<List<CalendarEvent>> events;
            if (Type.ToLower().Contains("month"))
            {
                events = _context.CalendarEvents.Where(x => x.Start.Date > Date.Date.AddMonths(-1) && x.Start.Date < Date.Date.AddMonths(1)).ToListAsync();
            }
            else if (Type.ToLower().Contains("week"))
            {
                events = _context.CalendarEvents.Where(x => x.Start.Date > Date.Date.AddDays(-7) && x.Start.Date < Date.Date.AddMonths(7)).ToListAsync();
            }
            else
            {
                events = _context.CalendarEvents.Where(x => x.Start.Date == Date.Date).ToListAsync();
            }
            return events;
        }

        public List<PacientesAutoCompleteViewModel> GetPatientNames(string Nombre)
        {
            return _context.Pacientes.Where(x => x.Nombre.ToLower().Contains(Nombre.ToLower())).Select(x => new PacientesAutoCompleteViewModel { Nombre = x.Nombre, Id = x.IdPaciente }).ToList();
        }

        public Task<List<PacientesAutoCompleteViewModel>> GetPatientNamesAsync(string Nombre)
        {
            return _context.Pacientes.Where(x => x.Nombre.ToLower().Contains(Nombre.ToLower())).Select(x => new PacientesAutoCompleteViewModel { Nombre = x.Nombre, Id = x.IdPaciente }).ToListAsync();
        }

        public int SaveEvent(CalendarEvent e)
        {
            e.IdPaciente = (_context.Pacientes.Where(x => x.Nombre.ToLower() == e.Subject.ToLower()).FirstOrDefault())?.IdPaciente;
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
            return _context.SaveChanges();
        }

        public Task<int> SaveEventAsync(CalendarEvent e)
        {
            e.IdPaciente = (_context.Pacientes.Where(x => x.Nombre.ToLower() == e.Subject.ToLower()).FirstOrDefault())?.IdPaciente;
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
            return _context.SaveChangesAsync();
        }
    }
}
