using AD.Demo.BookPublication.Domain.DTO;
using AD.Demo.BookPublication.Domain.Entities;
using AD.Demo.BookPublication.Domain.Interfaces;
using Nest;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Demo.BookPublication.ElasticSearch.Infrastructure.Repositories
{
    public class BookRepositoryES : IBookRepositoryES
    {
        private readonly IElasticClient _elasticClient;
        public BookRepositoryES(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<BookSearchResponse> GetAllBooks(string filterBy, string filterValue, string sortBy, string sortDir, int pageIndex, int pageSize)
        {
            var response = new BookSearchResponse();
            filterValue = filterValue ?? string.Empty;
            response.TotalRows = _elasticClient.Count<BookDTO>(s => s.Query(q => q.QueryString(d => d.Query("*" + filterValue + "*")))).Count;
            response.Result = _elasticClient.SearchAsync<BookDTO>(s => s.Query(q => q.QueryString(d => d.Query("*" + filterValue + "*"))).Skip(pageIndex > 0 ? (pageIndex - 1) * pageSize : 0).Size(pageSize)).Result.Documents.ToList();
            return await Task.Run(() => response);
        }

        public async Task<BookImportResponse> ImportSampleBookData(IList<BookDTO> books)
        {
            BookImportResponse response = new() { };
            foreach (var item in books)
            {
                await _elasticClient.IndexDocumentAsync(item);
                response.InsertedRowsCount++;
            }
            response.Success = true;
            return await Task.Run(() => response);
        }

    }
}
