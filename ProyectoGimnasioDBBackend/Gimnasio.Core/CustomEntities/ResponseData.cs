using System.Net;
using System.Text.Json.Serialization;
using Gimnasio.Core.QueryFilters;

namespace Gimnasio.Core.CustomEntities
{
    public class ResponseData
    {
        public PagedList<object> Pagination { get; set; }
        public Message[] Messages { get; set; } 
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }

    }
}