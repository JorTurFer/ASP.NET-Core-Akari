using Akari_Net.Core.Areas.Pacientes.Models.Data;
using Akari_Net.Core.Areas.Pacientes.Models.ViewModels.Calendario;
using Akari_Net.Core.Areas.Pacientes.Models.ViewModels.Pacientes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Areas.Pacientes.Data;

namespace Akari_Net.Core.Areas.Pacientes.Models.Services
{
    public interface IPacientesService
    {
        Task<PacientesPageDataViewModel> GetPacientesPageAsync(string text, int page, int pageSize, string sort, bool @ascending);
        Task<HistorialPageDataViewModel> GetHistorialPageAsync(int id, string text, int page, int pageSize, string sort, bool @ascending);
        List<Paciente> GetPacientes();
        Task<List<Paciente>> GetPacientesAsync();

        int Add(Paciente paciente);
        Task<int> AddAsync(Paciente paciente);

        int Update(Paciente paciente);
        Task<int> UpdateAsync(Paciente paciente);

        int Remove(Paciente paciente);
        Task<int> RemoveAsync(Paciente paciente);

        int Remove(Historial historia);
        Task<int> RemoveAsync(Historial historia);

        Paciente FindPacienteById(int id);
        Task<Paciente> FindPacienteByIdAsync(int id);

        Historial FindHistoriaById(int id);
        Task<Historial> FindHistoriaByIdAsync(int id);

        PacienteDataViewModel GetPacienteDataViewModel(int id);
        Task<PacienteDataViewModel> GetPacienteDataViewModelAsync(int id);

        bool PacienteExists(int id);

        int SaveChanges();
        Task<int> SaveChangesAsync();

        CitasPacienteViewModel GetCitasViewModel(int id);
        Task<CitasPacienteViewModel> GetCitasViewModelAsync(int id);

        List<PacientesAutoCompleteViewModel> GetPatientNames(string Nombre);
        Task<List<PacientesAutoCompleteViewModel>> GetPatientNamesAsync(string Nombre);

        int CreateRegistry(int idPaciente, string registry);
        Task<int> CreateRegistryAsync(int idPaciente, string registry);
        Task<int> UpdateRegistryAsync(int id, string registry);
    }
}
