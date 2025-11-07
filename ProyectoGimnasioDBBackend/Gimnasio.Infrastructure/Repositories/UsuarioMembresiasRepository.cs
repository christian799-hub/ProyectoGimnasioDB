
using Gimnasio.Core.CustomEntities;
using Gimnasio.Core.Entities;
using Gimnasio.Core.Enum;
using Gimnasio.Core.Interfaces;
using Gimnasio.Core.Repositories;
using Gimnasio.Infrastructure.Data;
using Gimnasio.Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;

namespace Gimnasio.Infrastructure.Repositories{
    public class UsuarioMembresiasRepository : BaseRepository<UsuarioMembresia>, IUsuarioMembresiasRepository
{
    private readonly IDapperContext _dapper;

    public UsuarioMembresiasRepository(GimnasioContext context, IDapperContext dapper) : base(context)
    {
        _dapper = dapper;
    }

    public async Task<IEnumerable<UsuarioMembresia>> GetAllUsuarioMembresiasAsync()
    {
        var usuarioMembresias = await _entities
        .Include(um => um.Usuario)
        .Include(um => um.Membresia)
        .ToListAsync();
        return usuarioMembresias;
    }

    public async Task<IEnumerable<UsuarioMembresia>> GetAllUsuariosDapperAsync(int limit = 10) // Si no manda parametro el limit es 10
    {
        try
        {
            var sql = _dapper.Provider switch
            {
                DatabaseProvider.SqlServer => UsuarioMembresiaQueries.UsuarioMembresiaQuerySqlServer,
                DatabaseProvider.MySql => UsuarioMembresiaQueries.UsuarioMembresiaQueryMySql,
                _ => throw new NotSupportedException("Provider no sportado")
            };

            return await _dapper.QueryAsync<UsuarioMembresia>(sql,new { Limit = limit }); 
        }
        catch(Exception err )
        {
            throw new Exception(err.Message);
        }
    }

    public async Task<UsuarioMembresia> GetUsuarioMembresiaByIdAsync(int id)
    {
        var usuarioMembresia = await _entities
        .Include(um => um.Usuario)
        .Include(um => um.Membresia)
        .FirstOrDefaultAsync( x=> x.Id == id);
        return usuarioMembresia;
    }

        public async Task<bool> UsuarioTieneMembresiaActivaAsync(int usuarioId) // Verifica si un usuario tiene una membresia activa
    {
        return await _entities
            .AnyAsync(um => um.UsuarioId == usuarioId && 
                           um.Estado == "Activa" && 
                           um.FechaFin >= DateOnly.FromDateTime(DateTime.Today));
    }

    public async Task<UsuarioMembresia> GetMembresiaActivaByUsuarioAsync(int usuarioId) // Obtiene la membresía activa de un usuario 
    {
        return await _entities
            .Include(um => um.Membresia)
            .Include(um => um.Usuario)
            .FirstOrDefaultAsync(um => um.UsuarioId == usuarioId && 
                                      um.Estado == "Activa" && 
                                      um.FechaFin >= DateOnly.FromDateTime(DateTime.Today));
    }

    public async Task<UsuarioMembresia> GetMembresiaActivaByUsuarioAndFechaAsync(int usuarioId, DateOnly fecha) // Obitnene la membresia y ayuda al service a comparar la fecha de asistencia y su fecha de vencimiento de membresia
    {
        return await _entities
            .Include(um => um.Membresia)
            .Include(um => um.Usuario)
            .FirstOrDefaultAsync(um => um.UsuarioId == usuarioId && 
                                    um.Estado == "Activa" && 
                                    um.FechaInicio <= fecha &&
                                    um.FechaFin >= fecha); 
    }

    public async Task<IEnumerable<UsuarioYMembresiaResponse>> GetUsuarioYMembresia()
    {
        try
            {
                var sql = UsuarioMembresiaQueries.UsuarioYMembresiaQuery;

                return await _dapper.QueryAsync<UsuarioYMembresiaResponse>(sql);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
    }
}
}
