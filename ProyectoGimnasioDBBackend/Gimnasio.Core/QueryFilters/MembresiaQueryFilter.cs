
namespace Gimnasio.Core.QueryFilters
{
    public class MembresiaQueryFilter : PaginationQueryFilter
    {
        public string? Descripcion { get; set; }

        public decimal? Precio { get; set; }

        public int? DuracionDias { get; set; }
    }
}