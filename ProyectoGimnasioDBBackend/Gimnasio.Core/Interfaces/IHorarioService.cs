using Gimnasio.Core.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;
namespace Gimnasio.Core.Interfaces
{
    public interface IHorarioService
    {
        Task<IEnumerable<Horario>> GetAllHorariosAsync();
        Task<Horario> GetHorarioByIdAsync(int id);
        Task InsertHorario(Horario horario);
        Task UpdateHorarioAsync(Horario horario);
        Task DeleteHorarioAsync(Horario horario);
        
         // Metodo adicional
        Task<IEnumerable<Horario>> GetHorariosByClaseAsync(int usuarioId);
    }
}