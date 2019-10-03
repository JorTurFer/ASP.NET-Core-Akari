using Akari_Net.Core.Areas.Pacientes.Models.Data;
using Akari_Net.Core.Areas.Pacientes.Models.ViewModels.Calendario;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Pacientes.Models.Services
{
    public interface ICalendarioServices
    {
        List<CalendarEvent> GetCalendarEvents(DateTime Date, string Type);
        Task<List<CalendarEvent>> GetCalendarEventsAsync(DateTime Date, string Type);

        int SaveEvent(CalendarEvent e);
        Task<int> SaveEventAsync(CalendarEvent e);


        int DeleteEvent(int eventID);
        Task<int> DeleteEventAsync(int eventID);

        List<TipoCita> GetTipoCitas();
        Task<List<TipoCita>> GetTipoCitasAsync();

        TipoCita FindTipoCitaByColor(string Color);
        Task<TipoCita> FindTipoCitaByColorAsync(string Color);

        CalendarioViewModel GetCalendarioViewModel();
        Task<CalendarioViewModel> GetCalendarioViewModelAsync();
    }
}
