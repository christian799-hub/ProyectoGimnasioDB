   namespace Gimnasio.Core.QueryFilters
{
   public class UsuarioQueryFilter : PaginationQueryFilter
   {
        public string? Nombre { get; set; }
    
        public int? Edad { get; set; }
        public string? Telefono { get; set; }
   }
}

   
