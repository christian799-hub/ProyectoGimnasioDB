namespace Gimnasio.Infrastructure.Queries
{
    public static class HorarioQueries
    {
        public static string HorarioQuerySqlServer = @"
            Select Id, ClaseId, DiaSemana, HoraInicio, HoraFin, Sala
            from Horarios
            order by Id asc
            OFFSET 0 ROWS FETCH NEXT @Limit ROWS ONLY;
        ";

        public static string HorarioQueryMySql = @"
            Select Id, ClaseId, DiaSemana, HoraInicio, HoraFin, Sala
            from Horarios
            order by Id asc
            LIMIT @Limit
        ";

        public static string HorariosClasesQuery = @"
            SELECT h.DiaSemana, CAST(h.HoraInicio AS CHAR) as HoraInicio, CAST(h.HoraFin AS CHAR) as HoraFin, h.Sala, c.Descripcion, c.Nivel
            FROM Horarios h
            INNER JOIN Clases c ON c.Id = h.ClaseId
            ORDER BY h.DiaSemana;
        ";
    }
}