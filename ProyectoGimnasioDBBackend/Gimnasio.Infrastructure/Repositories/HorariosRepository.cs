using Gimnasio.Core.CustomEntities;
using Gimnasio.Core.Entities;
using Gimnasio.Core.Enum;
using Gimnasio.Core.Interfaces;
using Gimnasio.Core.Repositories;
using Gimnasio.Infrastructure.Data;
using Gimnasio.Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;

namespace Gimnasio.Infrastructure.Repositories{
    public class HorariosRepository : BaseRepository<Horario>, IHorariosRepository
{
    private readonly IDapperContext _dapper;

        public HorariosRepository(GimnasioContext context,IDapperContext dapper) : base(context)
    {
        _dapper = dapper;
    }
    public async Task<IEnumerable<Horario>> GetAllAsync()
    {
        var horarios = await _entities
            .Include(h => h.Clase)
            .ThenInclude(c => c.Instructor)  
            .ToListAsync();
        return horarios;
    }

    public async Task<IEnumerable<Horario>> GetAllClaseDapperAsync(int limit = 10) 
    {
        try
        {
            var sql = _dapper.Provider switch
            {
                DatabaseProvider.SqlServer => HorarioQueries.HorarioQuerySqlServer,
                DatabaseProvider.MySql => HorarioQueries.HorarioQueryMySql,
                _ => throw new NotSupportedException("Provider no sportado")
            };

            return await _dapper.QueryAsync<Horario>(sql,new { Limit = limit }); 
        }
        catch(Exception err )
        {
            throw new Exception(err.Message);
        }
    }

    public async Task<Horario> GetHorarioByIdAsync(int id){
        var horario = await _entities
            .Include(h => h.Clase)
            .ThenInclude(c => c.Instructor)
            .FirstOrDefaultAsync(h => h.Id == id); 
        return horario;
    }


    // Metodo Extra
    public async Task<IEnumerable<Horario>> GetHorariosByClaseAsync(int claseId){
        var horarios = await _entities
            .Where(h => h.ClaseId == claseId)
            .Include(h => h.Clase)
            .ToListAsync();
        return horarios;
    }

    public async Task<IEnumerable<HorarioDisponibilidadResponse>> GetHorariosDisponibilidadAsync()
    {
        try
            {
                var sql = HorarioQueries.HorariosClasesQuery;

                return await _dapper.QueryAsync<HorarioDisponibilidadResponse>(sql);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
    }

}
}
