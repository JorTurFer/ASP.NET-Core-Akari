using Akari_Net.Core.Areas.Pacientes.Models.Entities;
using Akari_Net.Core.Areas.Pacientes.Models.ViewModels.Pacientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Pacientes.Models.Services
{
    public interface IPacientesService
    {
        PacientesPageDataViewModel GetPacientesPageAsync(string text, int page, int pageSize, string sort, bool @ascending);
        List<Paciente> GetPacientes();
        Task<List<Paciente>> GetPacientesAsync();

        int Add(Paciente paciente);
        Task<int> AddAsync(Paciente paciente);

        int Update(Paciente paciente);
        Task<int> UpdateAsync(Paciente paciente);

        int Remove(Paciente paciente);
        Task<int> RemoveAsync(Paciente paciente);

        Paciente FindPacienteById(int id);
        Task<Paciente> FindPacienteByIdAsync(int id);

        bool PacienteExists(int id);

        int SaveChanges();
        Task<int> SaveChangesAsync();

        CitasPacienteViewModel GetCitasViewModel(int id);
    }
}
