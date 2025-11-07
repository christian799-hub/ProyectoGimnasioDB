
namespace Gimnasio.Infrastructure.Queries
{
    public static class UsuarioQueries
    {
        public static string UsuarioQuerySqlServer = @"
                select Id, Nombre, Edad, Telefono, FechaRegistro, IsActive
                from usuarios
                order by Id asc
                OFFSET 0 ROWS FETCH NEXT @Limit ROWS ONLY;
                ";
        public static string UsuarioQueryMySql = @"
                select Id, Nombre, Edad, Telefono, FechaRegistro, IsActive
                from usuarios
                order by Id asc
                LIMIT @Limit
                ";
        
        public static string AsistenciasTotalesUsuarios = @"
                select u.Nombre, u.Id, COUNT(a.UsuarioId) as CantidadAsistencias
                from Usuarios u
                INNER JOIN Asistencia a 
                on  a.UsuarioId = u.Id
                group by u.Nombre, u.Id;    
                ";
        public static string UsuarioMembresia = @"
                select u.Nombre, u.Id, COUNT(a.UsuarioId) as CantidadAsistencias
                from Usuarios u
                INNER JOIN Asistencia a 
                on  a.UsuarioId = u.Id
                group by u.Nombre, u.Id;
                ";
    }
}