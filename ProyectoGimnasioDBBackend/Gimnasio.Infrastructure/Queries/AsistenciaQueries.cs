
namespace Gimnasio.Infrastructure.Queries
{
    public static class AsistenciaQueries
    {
        public static string AsistenciaQuerySqlServer = @"
                select Id, UsuarioId, HorarioId, FechaAsistencia
                from Asistencia
                order by Id asc
                OFFSET 0 ROWS FETCH NEXT @Limit ROWS ONLY;
                ";
        public static string AsistenciaQueryMySql = @"
                select Id, UsuarioId, HorarioId, FechaAsistencia
                from Asistencia
                order by Id asc
                LIMIT @Limit
                ";

        public static string AsistenciaCompletaQuery =@"
                SELECT u.Nombre, h.DiaSemana, CAST(h.HoraInicio AS CHAR) as HoraInicio, CAST(h.HoraFin AS CHAR) as HoraFin, h.Sala, c.Descripcion, CAST(a.FechaAsistencia AS CHAR) as FechaAsistencia, a.Estado
                FROM Asistencia a
                INNER JOIN Horarios h ON a.HorarioId = h.Id
                INNER JOIN Usuarios u ON a.UsuarioId = u.Id
                INNER JOIN Clases c ON c.Id = h.ClaseId
                order by u.Nombre asc;
                ";
    }
}