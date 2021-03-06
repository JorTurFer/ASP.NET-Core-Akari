﻿using Akari_Net.Core.Areas.Pacientes.Models.Data;
using Akari_Net.Core.Areas.Pacientes.Models.ViewModels.Calendario;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Pacientes.Models.Services
{
    public class CalendarioServices : ICalendarioServices
    {
        private readonly PatientsDbContext _context;

        public CalendarioServices(PatientsDbContext context)
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

        public TipoCita FindTipoCitaByColor(string Color)
        {
            return _context.TipoCitas.Where(x => x.Color == Color).First();
        }

        public async Task<TipoCita> FindTipoCitaByColorAsync(string Color)
        {
            return await _context.TipoCitas.Where(x => x.Color == Color).FirstAsync();
        }

        public List<CalendarEvent> GetCalendarEvents(DateTime Date, string Type)
        {
            List<CalendarEvent> events;
            if (Type.ToLower().Contains("month"))
            {
                events = _context.CalendarEvents.Where(x => x.Start.Date > Date.Date.AddMonths(-1) && x.Start.Date < Date.Date.AddMonths(1)).Include(x => x.TipoCita).ToList();
            }
            else if (Type.ToLower().Contains("week"))
            {
                events = _context.CalendarEvents.Where(x => x.Start.Date > Date.Date.AddDays(-7) && x.Start.Date < Date.Date.AddMonths(7)).Include(x => x.TipoCita).ToList();
            }
            else
            {
                events = _context.CalendarEvents.Where(x => x.Start.Date == Date.Date).Include(x => x.TipoCita).ToList();
            }
            return events;
        }

        public async Task<List<CalendarEvent>> GetCalendarEventsAsync(DateTime Date, string Type)
        {
            List<CalendarEvent> events;
            if (Type.ToLower().Contains("month"))
            {
                events = await _context.CalendarEvents.Where(x => x.Start.Date > Date.Date.AddMonths(-1) && x.Start.Date < Date.Date.AddMonths(1)).Include(x => x.TipoCita).ToListAsync();
            }
            else if (Type.ToLower().Contains("week"))
            {
                events = await _context.CalendarEvents.Where(x => x.Start.Date > Date.Date.AddDays(-7) && x.Start.Date < Date.Date.AddMonths(7)).Include(x => x.TipoCita).ToListAsync();
            }
            else
            {
                events = await _context.CalendarEvents.Where(x => x.Start.Date == Date.Date).Include(x => x.TipoCita).ToListAsync();
            }
            return events;
        }

        public CalendarioViewModel GetCalendarioViewModel()
        {
            return new CalendarioViewModel { TipoCitas = _context.TipoCitas.Select(x => new SelectListItem { Value = x.Color.ToString(), Text = x.Tipo }).ToList() };
        }

        public async Task<CalendarioViewModel> GetCalendarioViewModelAsync()
        {
            var tipocitas = await _context.TipoCitas.Select(x => new SelectListItem { Value = x.Color.ToString(), Text = x.Tipo }).ToListAsync();
            return new CalendarioViewModel { TipoCitas = tipocitas };
        }

        public List<TipoCita> GetTipoCitas()
        {
            return _context.TipoCitas.ToList();
        }

        public Task<List<TipoCita>> GetTipoCitasAsync()
        {
            return _context.TipoCitas.ToListAsync();
        }

        public int SaveEvent(CalendarEvent e)
        {
            e.IdPaciente = (_context.Pacientes.Where(x => x.Nombre.ToLower() == e.Subject.ToLower()).FirstOrDefault())?.IdPaciente;
            if (e.EventID > 0)
            {
                //Update the event
                var v = _context.CalendarEvents.Where(a => a.EventID == e.EventID).FirstOrDefault();
                FetchEventData(v, e);
            }
            else
            {
                _context.CalendarEvents.Add(e);
            }
            return _context.SaveChanges();
        }

        public async Task<int> SaveEventAsync(CalendarEvent e)
        {
            e.IdPaciente = (await _context.Pacientes.Where(x => x.Nombre.ToLower() == e.Subject.ToLower()).FirstOrDefaultAsync())?.IdPaciente;
            if (e.EventID > 0)
            {
                //Update the event
                var v = await _context.CalendarEvents.Where(a => a.EventID == e.EventID).FirstOrDefaultAsync();
                FetchEventData(v, e);
            }
            else
            {
                await _context.CalendarEvents.AddAsync(e);
            }
            return await _context.SaveChangesAsync();
        }

        private void FetchEventData(CalendarEvent PreviousEvent, CalendarEvent NewEvent)
        {
            if (PreviousEvent != null)
            {
                PreviousEvent.Subject = NewEvent.Subject;
                PreviousEvent.Start = NewEvent.Start;
                PreviousEvent.End = NewEvent.End;
                PreviousEvent.Description = NewEvent.Description;
                PreviousEvent.IsFullDay = NewEvent.IsFullDay;
                PreviousEvent.IdTipoCita = NewEvent.IdTipoCita;
                PreviousEvent.IdPaciente = NewEvent.IdPaciente;
                PreviousEvent.IsSMSSended = false;
            }
        }
    }
}
