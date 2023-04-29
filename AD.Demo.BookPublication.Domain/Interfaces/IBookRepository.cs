using AD.Demo.BookPublication.Domain.DTO;
using AD.Demo.BookPublication.Domain.Entities;
using SD.BuildingBlocks.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AD.Demo.BookPublication.Domain.Interfaces
{
    public interface IBookRepository  : IRepository<Book>
    {
        Task<BookSearchResponse> GetAllBooks(string filterBy, string filterValue, string sortBy, string sortDir, int pageIndex, int pageSize);
        IQueryable<Book> GetBook(Expression<Func<Book, bool>> filter);
        Task<BookImportResponse> ImportSampleBookData(IList<Book> books);
    }
}
