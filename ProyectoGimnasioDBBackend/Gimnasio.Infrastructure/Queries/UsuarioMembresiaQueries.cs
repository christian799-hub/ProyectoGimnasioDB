namespace Gimnasio.Infrastructure.Queries
{
        public static class UsuarioMembresiaQueries
    {
        public static string UsuarioMembresiaQuerySqlServer = @"
            Select Id, UsuarioId, MembresiaId, FechaInicio, FechaFin, ClasesRestantes,Estado,PrecioPagado
            from Usuario_Membresias
            order by Id asc
            OFFSET 0 ROWS FETCH NEXT @Limit ROWS ONLY;
        ";

        public static string UsuarioMembresiaQueryMySql = @"
            Select Id, UsuarioId, MembresiaId, FechaInicio, FechaFin, ClasesRestantes,Estado,PrecioPagado
            from Usuario_Membresias
            order by Id asc
            LIMIT @Limit
        ";

        public static string UsuarioYMembresiaQuery = @"
            select u.Nombre, u.Id, m.Descripcion, um.ClasesRestantes
            from Usuario_Membresias um
            INNER JOIN Usuarios u
            on  um.UsuarioId = u.Id
            INNER JOIN Membresias m
            on m.Id = um.MembresiaId
            group by u.Nombre, u.Id, m.Descripcion, um.ClasesRestantes;
            ";
    }
}