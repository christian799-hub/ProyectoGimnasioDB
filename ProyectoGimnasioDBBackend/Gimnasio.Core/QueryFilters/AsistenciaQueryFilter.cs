
namespace Gimnasio.Core.QueryFilters
{
    public class AsistenciaQueryFilter : PaginationQueryFilter
    {
        public int? UsuarioId { get; set; }

        public int? HorarioId { get; set; }

        public DateOnly FechaAsistencia { get; set; }
    }
}