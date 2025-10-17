    using Gimnasio.Core.Entities;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

namespace Gimnasio.Core.Interfaces
{
    public interface IHorariosRepository
    {
        // CRUD
        Task<IEnumerable<Horario>> GetAllHorariosAsync();
        Task<Horario> GetHorarioByIdAsync(int id);
        Task InsertarHorario(Horario horario);
        Task UpdateHorario(Horario horario);
        Task DeleteHorario(Horario horario);

        // Metodo adicional
        Task<IEnumerable<Horario>> GetHorariosByClaseAsync(int claseId);

    }
}