using Gimnasio.Core.CustomEntities;
using Gimnasio.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gimnasio.Core.Interfaces{
    public interface IAsistenciaRepository : IBaseRepository<Asistencium>
    {
        // CRUD
        new Task<IEnumerable<Asistencium>> GetAllAsync(); //Todas las asistencias ( La palabra new deja mi implementacion y oculta el de base repository )
        Task<IEnumerable<Asistencium>> GetAllAsistenciasDapperAsync(int limit = 10);
        new Task<Asistencium> GetByIdAsync(int id); 
        Task RegistrarAsistenciaAsync(Asistencium asistencia);

        // Metodos adicionales
        Task<IEnumerable<Asistencium>> GetAsistenciaByClaseAsync(int claseId, DateOnly fecha);
        Task<bool> UsuarioYaRegistroAsistenciaAsync(int usuarioId, int horarioId, DateOnly fecha);

        Task<IEnumerable<Asistencium>> GetAsistenciaByIdUsuarioAsync(int usuarioId);//Lista de Asistencias de un usuario determinado
        Task<int> GetCantidadAsistenciasByHorarioAndFechaAsync(int horarioId, DateOnly fecha);
        Task<IEnumerable<AsistenciaCompletaResponse>> GetAsistenciaCompleta();
    }
}