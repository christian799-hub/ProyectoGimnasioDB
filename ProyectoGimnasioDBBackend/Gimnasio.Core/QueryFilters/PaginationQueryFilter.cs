
using Swashbuckle.AspNetCore.Annotations;

namespace Gimnasio.Core.QueryFilters
{
    public abstract class PaginationQueryFilter
    {
        [SwaggerSchema("Cantidad de registros por pagina")]
        public int PageSize  { get; set; } // CANTIDAD REGISTROS POR PAGINA
        
        [SwaggerSchema("Numero de pagina a mostrar")]
        public int PageNumber { get; set; } //Numero de paginas mostrar

        
    }
}