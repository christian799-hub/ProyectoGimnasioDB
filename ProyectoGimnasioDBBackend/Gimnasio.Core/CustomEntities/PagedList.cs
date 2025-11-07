
using Gimnasio.Core.Entities;

namespace Gimnasio.Core.QueryFilters
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public int? NextPageNumber => HasNextPage ? CurrentPage + 1 : null;
        public int? PreviousPageNumber => HasPreviousPage ? CurrentPage - 1 : null;

        public PagedList( List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items); //Adicionar a la lista de manera masiva
        }    

        public static PagedList<T> Create(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(); //Skip divide registros y Take toma los registros
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}