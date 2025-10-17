using Gimnasio.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gimnasio.Core.Interfaces{
    public interface IAsistenciaRepository
    {
        // CRUD
        Task<IEnumerable<Asistencium>> GetAllAsistenciasAsync(); //Todas las asistencias
        Task<Asistencium> GetAsistenciaByIdAsync(int id); 
        Task InsertarAsistencia(Asistencium asistencia); 
        Task UpdateAsistencia(Asistencium asistencia);
        Task DeleteAsistencia(Asistencium asistencia);
        Task RegistrarAsistenciaAsync(Asistencium asistencia);

        // Metodos adicionales
        Task<IEnumerable<Asistencium>> GetAsistenciaByClaseAsync(int claseId, DateOnly fecha);
        Task<bool> UsuarioYaRegistroAsistenciaAsync(int usuarioId, int horarioId, DateOnly fecha);

        Task<IEnumerable<Asistencium>> GetAsistenciaByIdUsuarioAsync(int usuarioId);//Lista de Asistencias de un usuario determinado
        Task<int> GetCantidadAsistenciasByHorarioAndFechaAsync(int horarioId, DateOnly fecha);
    }
}