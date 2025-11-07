
namespace Gimnasio.Core.QueryFilters
{
    public class UsuarioMembresiaQueryFilter : PaginationQueryFilter
    {
        public int? UsuarioId { get; set; }

        public int? MembresiaId { get; set; }

        public int? ClasesRestantes { get; set; }
    }
}