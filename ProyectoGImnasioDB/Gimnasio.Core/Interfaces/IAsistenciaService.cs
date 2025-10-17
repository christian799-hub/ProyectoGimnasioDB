
using Gimnasio.Core.Entities;

namespace Gimnasio.Core.Interfaces
{
    public interface IAsistenciaService
    {
        Task<IEnumerable<Asistencium>> GetAllAsistenciasAsync();
        Task<Asistencium> GetAsistenciaByIdAsync(int id);
        Task InsertAsistencia(Asistencium asistencia);
        Task UpdateAsistenciaAsync(Asistencium asistencia);
        Task DeleteAsistenciaAsync(Asistencium asistencia);
        Task<IEnumerable<Asistencium>> GetAsistenciaByClaseAsync(int claseId,DateOnly fecha);
        Task<IEnumerable<Asistencium>> GetAsistenciaByIdUsuarioAsync(int usuarioId);
        Task<bool> UsuarioYaRegistroAsistenciaAsync(int usuarioId, int horarioId, DateOnly fecha);
    }
}