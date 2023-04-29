using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Demo.BookPublication.Domain.DTO
{
    public class BookDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string Genres { get; set; }
        public int TotalPages { get; set; }
        public string ISBN { get; set; }
        public int? PublishedYear { get; set; }
        public string Language { get; set; }
    }
    public class BookSearchResponse
    {
        public IEnumerable<BookDTO> Result { get; set; }
        public int TotalRows { get; set; }

    }
    public class BookImportResponse
    {
        public int InsertedRowsCount { get; set; }
        public int UpdatedRowsCount { get; set; }
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
