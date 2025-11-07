using Gimnasio.Core.CustomEntities;
using Gimnasio.Core.Entities;
using Gimnasio.Core.Enum;
using Gimnasio.Core.Interfaces;
using Gimnasio.Core.Repositories;
using Gimnasio.Infrastructure.Data;
using Gimnasio.Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;

namespace Gimnasio.Infrastructure.Repositories{
    public class UsuariosRepository : BaseRepository<Usuario>, IUsuariosRepository
{
    private readonly IDapperContext _dapper; 

    public UsuariosRepository(GimnasioContext context,IDapperContext dapper ) : base(context)
    {
        _dapper = dapper;
    }

    // Metodo Extra
    public async Task<IEnumerable<Usuario>> GetAllUsuariosActivosAsync(){
        var usuarios = _entities.Where(u => u.IsActive == 1);
        return usuarios;
    }
   
    public async Task<IEnumerable<Usuario>> GetAllUsuariosDapperAsync(int limit = 10) // Si no manda parametro el limit es 10
    {
        try
        {
            var sql = _dapper.Provider switch
            {
                DatabaseProvider.SqlServer => UsuarioQueries.UsuarioQuerySqlServer,
                DatabaseProvider.MySql => UsuarioQueries.UsuarioQueryMySql,
                _ => throw new NotSupportedException("Provider no sportado")
            };

            return await _dapper.QueryAsync<Usuario>(sql,new { Limit = limit }); 
        }
        catch(Exception err )
        {
            throw new Exception(err.Message);
        }
    }

    public async Task<IEnumerable<UsuarioAsistenciaResponse>> GetAsistenciasUsuarios()
    {
        try
            {
                var sql = UsuarioQueries.AsistenciasTotalesUsuarios;

                return await _dapper.QueryAsync<UsuarioAsistenciaResponse>(sql);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
    }
}
}
