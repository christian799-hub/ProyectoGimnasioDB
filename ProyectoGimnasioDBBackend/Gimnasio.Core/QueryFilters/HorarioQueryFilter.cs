
namespace Gimnasio.Core.QueryFilters
{
    public class HorarioQueryFilter : PaginationQueryFilter
    {
        public int? ClaseId { get; set; }
        public string? DiaSemana { get; set; }
        public string? Sala { get; set; }
    }
}