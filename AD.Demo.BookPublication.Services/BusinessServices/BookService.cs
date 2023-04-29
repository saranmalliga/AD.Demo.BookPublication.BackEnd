using AD.Demo.BookPublication.Domain.DTO;
using AD.Demo.BookPublication.Domain.Entities;
using AD.Demo.BookPublication.Domain.Interfaces;
using AD.Demo.BookPublication.Domain.Validators;
using AD.Demo.BookPublication.Interfaces.Interfaces;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Demo.BookPublication.Services.BusinessServices
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBookRepositoryES _bookRepositoryES;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public BookService(IBookRepository bookRepository, IMapper mapper, IConfiguration config, IBookRepositoryES bookRepositoryES)
        {
            _bookRepository = bookRepository;
            _bookRepositoryES = bookRepositoryES;
            _mapper = mapper;
            _config = config;
        }
        public async Task<BookSearchResponse> GetAllBooks(string filterBy, string filterValue, string sortBy, string sortDir, int? pageIndex, int? pageSize)
        {
            BookSearchResponse response = new ();
            try
            {
                if (IsElasticSearchEnabled())
                {
                    response = await _bookRepositoryES.GetAllBooks(filterBy, filterValue, sortBy, sortDir, pageIndex ?? 0, pageSize ?? 20);
                    if (response?.Result?.Count() > 0)
                    {
                        return await Task.Run(()=> response);
                    }
                }
            }
            catch (Exception ex)
            {

            }
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
                    book = _mapper.Map<BookDTO, Book>(item, book ?? new Book());
                    item.Id = book.ID = id;
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
            try
            {
                if (IsElasticSearchEnabled())
                {
                    foreach (var item in books)
                    {
                        await _bookRepositoryES.ImportSampleBookData(books);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return response;
        }

        private bool IsElasticSearchEnabled() => _config["AppSettings:IsElasticSearchEnabled"] == "true";

    }
}
