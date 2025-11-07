namespace Gimnasio.Infrastructure.Queries
{
    public static class ClaseQueries
    {
                public static string ClaseQuerySqlServer = @"
                select Id, Descripcion, InstructorId, CapacidadMaxima, DuracionMinutos, Nivel
                from Clases
                order by Id asc
                OFFSET 0 ROWS FETCH NEXT @Limit ROWS ONLY;
                ";
        public static string ClaseQueryMySql = @"
                select Id, Descripcion, InstructorId, CapacidadMaxima, DuracionMinutos, Nivel
                from Clases
                order by Id asc
                LIMIT @Limit
                ";

        public static string ClaseInstructorQuery = @"
                SELECT i.Nombre, i.Especialidad, c.Descripcion, c.Nivel
                FROM Clases c
                INNER JOIN Instructores i
                ON c.InstructorId = i.Id
                ORDER BY c.Id;
                ";  
    }
}