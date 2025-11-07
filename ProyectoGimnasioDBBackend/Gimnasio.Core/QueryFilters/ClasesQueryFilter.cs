
using Swashbuckle.AspNetCore.Annotations;

namespace Gimnasio.Core.QueryFilters
{
    /// <summary>
    /// Filtra los parametros de Clases
    /// </summary>
    public class ClasesQueryFilter : PaginationQueryFilter
    {
        /// <summary>
        /// Descripcion de la clase
        /// </summary>
        [SwaggerSchema("Descripcion de la clase")]
        public string? Descripcion { get; set; }
        /// <summary>
        /// Id del instructor
        /// </summary>
        [SwaggerSchema("Id del instructor")]
        public int? InstructorId { get; set; }
        /// <summary>
        /// Capacidad maxima de la clase
        /// </summary>
        [SwaggerSchema("Capacidad maxima de la clase")]
        public int? CapacidadMaxima { get; set; }
    }
}