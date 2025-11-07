using Gimnasio.Core.CustomEntities;
using Gimnasio.Core.Entities;
using Gimnasio.Core.Enum;
using Gimnasio.Core.Interfaces;
using Gimnasio.Core.Repositories;
using Gimnasio.Infrastructure.Data;
using Gimnasio.Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;

namespace Gimnasio.Infrastructure.Repositories{
    public class AsistenciaRepository : BaseRepository<Asistencium>, IAsistenciaRepository
{
    private readonly IDapperContext _dapper;

    public AsistenciaRepository(GimnasioContext context,IDapperContext dapper) : base(context)
    {
        _dapper = dapper;
    }


    public async Task<IEnumerable<Asistencium>> GetAllAsync()
    {
        var asistencias = await _entities
        .Include(a => a.Usuario)      
        .Include(a => a.Horario)      
        .ThenInclude(h => h.Clase)
        .ThenInclude(c => c.Instructor)  
        .ToListAsync();
        return asistencias;
    }

    public async Task<IEnumerable<Asistencium>> GetAllAsistenciasDapperAsync(int limit = 10) 
    {
        try
        {
            var sql = _dapper.Provider switch
            {
                DatabaseProvider.SqlServer => AsistenciaQueries.AsistenciaQuerySqlServer,
                DatabaseProvider.MySql => AsistenciaQueries.AsistenciaQueryMySql,
                _ => throw new NotSupportedException("Provider no sportado")
            };

            return await _dapper.QueryAsync<Asistencium>(sql,new { Limit = limit }); 
        }
        catch(Exception err )
        {
            throw new Exception(err.Message);
        }
    }

   public async Task<Asistencium?> GetByIdAsync(int id)
    {
        var asistencia = await _entities
            .Include(a => a.Usuario)      
            .Include(a => a.Horario)      
            .ThenInclude(h => h.Clase)
            .ThenInclude(c => c.Instructor)    
            .FirstOrDefaultAsync(a => a.Id == id);
        return asistencia;
    }

    public async Task RegistrarAsistenciaAsync(Asistencium asistencia)
    {
    var existeRegistro = await UsuarioYaRegistroAsistenciaAsync(
        asistencia.UsuarioId, 
        asistencia.HorarioId, 
        asistencia.FechaAsistencia  
    );
    }

    public async Task<IEnumerable<Asistencium>> GetAsistenciaByClaseAsync(int claseId, DateOnly fecha)
    {
        return await _entities
            .Include(a => a.Usuario)
            .Include(a => a.Horario)
            .Where(a => a.Horario.ClaseId == claseId &&
                       a.FechaAsistencia.Year == fecha.Year &&
                       a.FechaAsistencia.Month == fecha.Month &&
                       a.FechaAsistencia.Day == fecha.Day)
            .ToListAsync();
    }

    public async Task<bool> UsuarioYaRegistroAsistenciaAsync(int usuarioId, int horarioId, DateOnly fecha)
    {
    return await _entities
        .AnyAsync(a => a.UsuarioId == usuarioId && 
                      a.HorarioId == horarioId && 
                      a.FechaAsistencia.Year == fecha.Year &&
                      a.FechaAsistencia.Month == fecha.Month &&
                      a.FechaAsistencia.Day == fecha.Day);

    }
        public async Task<IEnumerable<Asistencium>> GetAsistenciaByIdUsuarioAsync(int usuarioId)
    {
        var asistenciaUsuario = await _entities
        .Where(x => x.UsuarioId == usuarioId).Include(a => a.Horario).ThenInclude(h => h.Clase)
        .OrderByDescending(a => a.FechaAsistencia)
        .ToListAsync();
        return asistenciaUsuario;
    }

    public async Task<int> GetCantidadAsistenciasByHorarioAndFechaAsync(int horarioId, DateOnly fecha)
    {
        return await _entities
            .CountAsync(a => a.HorarioId == horarioId && 
                             a.FechaAsistencia.Year == fecha.Year &&
                             a.FechaAsistencia.Month == fecha.Month &&
                             a.FechaAsistencia.Day == fecha.Day);
    
    }

    public async Task<IEnumerable<AsistenciaCompletaResponse>> GetAsistenciaCompleta()
    {
        try
            {
                var sql = AsistenciaQueries.AsistenciaCompletaQuery;

                return await _dapper.QueryAsync<AsistenciaCompletaResponse>(sql);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
    }

    
}
}
