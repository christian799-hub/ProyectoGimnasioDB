using Gimnasio.Core.CustomEntities;
using Gimnasio.Core.Entities;
using Gimnasio.Core.QueryFilters;
using System.Collections.Generic;
    using System.Threading.Tasks;
namespace Gimnasio.Core.Interfaces
{
    public interface IHorarioService
    {
        Task<ResponseData> GetAllHorariosAsync(HorarioQueryFilter horarioQueryFilter);
        Task<IEnumerable<Horario>> GetAllClaseDapperAsync(); 
        Task<Horario> GetHorarioByIdAsync(int id);
        Task InsertHorario(Horario horario);
        Task UpdateHorarioAsync(Horario horario);
        Task DeleteHorarioAsync(Horario horario);
        
         // Metodo adicional
        Task<IEnumerable<Horario>> GetHorariosByClaseAsync(int usuarioId);
        Task<IEnumerable<HorarioDisponibilidadResponse>> GetHorariosDisponibilidadAsync();
    }
}