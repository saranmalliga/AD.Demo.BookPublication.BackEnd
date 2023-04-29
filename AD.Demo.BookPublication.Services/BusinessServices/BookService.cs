using AD.Demo.BookPublication.Domain.DTO;
using AD.Demo.BookPublication.Domain.Entities;
using AD.Demo.BookPublication.Domain.Interfaces;
using AD.Demo.BookPublication.Domain.Validators;
using AD.Demo.BookPublication.Interfaces.Interfaces;
using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Demo.BookPublication.Services.BusinessServices
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;

        }
        public async Task<BookSearchResponse> GetAllBooks(string filterBy, string filterValue, string sortBy, string sortDir, int? pageIndex, int? pageSize)
        {
            return await _bookRepository.GetAllBooks(filterBy, filterValue, sortBy, sortDir, pageIndex ?? 0, pageSize ?? 20);
        }
        public async Task<BookImportResponse> ImportSampleBookData(IList<BookDTO> books)
        {
            BookImportResponse response = new() { };
            IList<Book> bookData = new List<Book>();
            foreach (var item in books)
            {
                var result = await new ImportBookValidator().ValidateAsync(item);
                if (result.IsValid)
                {
                    var book = _bookRepository.GetBook(w => w.IS_ACTIVE && ((w.ISBN != null && w.ISBN == item.ISBN) || (w.TITLE == item.Title && w.AUTHOR == item.Author))).FirstOrDefault();
                    string id = book?.ID;
                    book =  _mapper.Map<BookDTO, Book>(item, book ?? new Book());
                    book.ID = id;
                    bookData.Add(book);
                }
                else
                {
                    response.Success = false;
                    response.Errors = result.Errors.Select(s => s.ErrorMessage).ToList();
                    return response;
                }
            }
            response = await _bookRepository.ImportSampleBookData(bookData);
            return response;
        }
    }
}
