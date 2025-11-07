using Gimnasio.Core.CustomEntities;
using Gimnasio.Core.Entities;
using Gimnasio.Core.Enum;
using Gimnasio.Core.Interfaces;
using Gimnasio.Core.Repositories;
using Gimnasio.Infrastructure.Data;
using Gimnasio.Infrastructure.Queries;

namespace Gimnasio.Infrastructure.Repositories
{
    public class MembresiaRepository : BaseRepository<Membresia>, IMembresiasRepository
    {
        private readonly IDapperContext _dapper;
        public MembresiaRepository(GimnasioContext context, IDapperContext dapper) : base(context)
        {
            _dapper = dapper;
        }
    
        public async Task<IEnumerable<Membresia>> GetAllClaseDapperAsync(int limit = 10) 
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DatabaseProvider.SqlServer => MembresiasQueries.MembresiaQuerySqlServer,
                    DatabaseProvider.MySql => MembresiasQueries.MembresiaQueryMySql,
                    _ => throw new NotSupportedException("Provider no sportado")
                };

                return await _dapper.QueryAsync<Membresia>(sql,new { Limit = limit }); 
            }
            catch(Exception err )
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<IEnumerable<MembresiaOrdenPrecioResponse>> GetMembresiaOrdenPrecio()
        {
            try
                {
                    var sql = MembresiasQueries.MembresiaOrdenPrecioQuery;

                    return await _dapper.QueryAsync<MembresiaOrdenPrecioResponse>(sql);
                }
                catch (Exception err)
                {
                    throw new Exception(err.Message);
                }
        }


    }
}