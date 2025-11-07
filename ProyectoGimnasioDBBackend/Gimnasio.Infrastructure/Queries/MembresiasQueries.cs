namespace Gimnasio.Infrastructure.Queries
{
    public class MembresiasQueries
    {
        public const string MembresiaQuerySqlServer = @"
            SELECT Id, Descripcion, Precio, DuracionDias, ClasesIncluidas
            FROM Membresias
            ORDER BY Id DESC;
            OFFSET 0 ROWS FETCH NEXT @Limit ROWS ONLY;
        ";

        public const string MembresiaQueryMySql = @"
            SELECT Id, Descripcion, Precio, DuracionDias, ClasesIncluidas
            FROM Membresias
            ORDER BY Id DESC
            LIMIT @Limit;
        ";

        public const string MembresiaOrdenPrecioQuery = @"
            SELECT Descripcion, Precio, DuracionDias, ClasesIncluidas
            FROM Membresias
            ORDER BY Precio DESC;
        ";
        
    }
}