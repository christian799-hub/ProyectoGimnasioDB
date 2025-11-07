using Gimnasio.Core.CustomEntities;
using Gimnasio.Core.Entities;
using Gimnasio.Core.Enum;
using Gimnasio.Core.Interfaces;
using Gimnasio.Core.Repositories;
using Gimnasio.Infrastructure.Data;
using Gimnasio.Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;

namespace Gimnasio.Infrastructure.Repositories{
    public class ClasesRepository : BaseRepository<Clase>, IClasesRepository
{
    private readonly IDapperContext _dapper;
    public ClasesRepository(GimnasioContext context,IDapperContext dapper) : base(context)
    {
        _dapper = dapper;
    }

    public async Task<IEnumerable<Clase>> GetAllClasesAsync()
    {
        var clases = await _entities
        .Include(c => c.Instructor)    
        .Include(c => c.Horarios)
        .ToListAsync();
        return clases;
    }

    public async Task<IEnumerable<Clase>> GetAllClaseDapperAsync(int limit = 10) 
    {
        try
        {
            var sql = _dapper.Provider switch
            {
                DatabaseProvider.SqlServer => ClaseQueries.ClaseQuerySqlServer,
                DatabaseProvider.MySql => ClaseQueries.ClaseQueryMySql,
                _ => throw new NotSupportedException("Provider no sportado")
            };

            return await _dapper.QueryAsync<Clase>(sql,new { Limit = limit }); 
        }
        catch(Exception err )
        {
            throw new Exception(err.Message);
        }
    }

    public async Task<Clase> GetClaseByIdAsync(int id)
    {
        var clase = await _entities
        .Include(c => c.Instructor)    
        .Include(c => c.Horarios)
        .FirstOrDefaultAsync(x => x.Id == id);
        return clase;
    }

    public async Task<IEnumerable<Clase>> GetClaseByInstructorAsync(int instructorId)
    {
        var clases = await _entities
        .Where(x => x.InstructorId == instructorId)
        .Include(c => c.Instructor)
        .Include(c => c.Horarios)
        .ToListAsync();
        return clases;
    }
    public async Task<int> GetCapacidadMaximaAsync(int claseId)
    {
        var clase = await _entities
        .FirstOrDefaultAsync(c => c.Id == claseId);
        if (clase == null)
        {
            throw new Exception("Clase no encontrada");
        }
        return clase.CapacidadMaxima;
    }

    public async Task<IEnumerable<ClaseInstructorResponse>> GetClaseInstructor()
    {
        try
            {
                var sql = ClaseQueries.ClaseInstructorQuery;

                return await _dapper.QueryAsync<ClaseInstructorResponse>(sql);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
    }
    
}
}
