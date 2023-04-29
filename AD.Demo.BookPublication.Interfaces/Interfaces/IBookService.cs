using AD.Demo.BookPublication.Domain.DTO;
using AD.Demo.BookPublication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Demo.BookPublication.Interfaces.Interfaces
{
    public interface IBookService
    {
        Task<BookSearchResponse> GetAllBooks(string filterBy, string filterValue, string sortBy, string sortDir, int? pageIndex, int? pageSize);
        Task<BookImportResponse> ImportSampleBookData(IList<BookDTO> books);

    }
}
