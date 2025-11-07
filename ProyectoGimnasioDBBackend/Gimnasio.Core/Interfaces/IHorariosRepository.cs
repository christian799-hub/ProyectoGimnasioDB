using Gimnasio.Core.CustomEntities;
using Gimnasio.Core.Entities;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

namespace Gimnasio.Core.Interfaces
{
    public interface IHorariosRepository : IBaseRepository<Horario>
    {
        // CRUD
        new Task<IEnumerable<Horario>> GetAllAsync();
        Task<IEnumerable<Horario>> GetAllClaseDapperAsync(int limit = 10);
        new Task<Horario> GetByIdAsync(int id);

        // Metodo adicional
        Task<IEnumerable<Horario>> GetHorariosByClaseAsync(int claseId);
        Task<IEnumerable<HorarioDisponibilidadResponse>> GetHorariosDisponibilidadAsync();

    }
}