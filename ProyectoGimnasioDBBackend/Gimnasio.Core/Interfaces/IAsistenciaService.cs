
using Gimnasio.Core.CustomEntities;
using Gimnasio.Core.Entities;
using Gimnasio.Core.QueryFilters;

namespace Gimnasio.Core.Interfaces
{
    public interface IAsistenciaService
    {
        Task<ResponseData> GetAllAsistenciasAsync(AsistenciaQueryFilter asistenciaQueryFilter);
        Task<IEnumerable<Asistencium>> GetAllAsistenciasDapperAsync();
        Task<Asistencium> GetAsistenciaByIdAsync(int id);
        Task InsertAsistencia(Asistencium asistencia);
        Task UpdateAsistenciaAsync(Asistencium asistencia);
        Task DeleteAsistenciaAsync(Asistencium asistencia);

         // Metodo adicional
        Task<IEnumerable<Asistencium>> GetAsistenciaByClaseAsync(int claseId,DateOnly fecha);
        Task<IEnumerable<Asistencium>> GetAsistenciaByIdUsuarioAsync(int usuarioId);
        Task<bool> UsuarioYaRegistroAsistenciaAsync(int usuarioId, int horarioId, DateOnly fecha);
        Task<IEnumerable<AsistenciaCompletaResponse>> GetAsistenciaCompleta();
    }
}