using AD.Demo.BookPublication.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Demo.BookPublication.Domain.Interfaces
{
    public interface IBookRepositoryES
    {
        Task<BookSearchResponse> GetAllBooks(string filterBy, string filterValue, string sortBy, string sortDir, int pageIndex, int pageSize);
        Task<BookImportResponse> ImportSampleBookData(IList<BookDTO> books);
    }
}
