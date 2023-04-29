using AD.Demo.BookPublication.Domain.DTO;
using AD.Demo.BookPublication.Domain.Entities;
using AD.Demo.BookPublication.Domain.Interfaces;
using AD.Demo.BookPublication.SQL.Infrastructure.Data;
using AD.Demo.BookPublication.SQL.Infrastructure.Utility;
using Dapper;
using SD.BuildingBlocks.Infrastructure;
using SD.BuildingBlocks.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AD.Demo.BookPublication.SQL.Infrastructure.Repositories
{
    public class BookRepository : RepositoryEF<Book>, IBookRepository
    {
        private readonly BookManagementDbContext _dbContext;
        private readonly IDbConnection _dbConnection;
        private readonly IUnitOfWork _unitofWork;
        public BookRepository(BookManagementDbContext dbContext, 
            IDbConnection dbConnection,
            IUnitOfWork unitofWork
            ) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbConnection = dbConnection;
            _unitofWork = unitofWork;
        }

        public async Task<BookSearchResponse> GetAllBooks(string filterBy, string filterValue, string sortBy, string sortDir, int pageIndex, int pageSize)
        {
            var response = new BookSearchResponse();
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@SearchValue", filterValue ?? "" } };
            string whereClause = SQLQueryBuilder.GetBookSearchWhereClauese(filterBy);
            int skip = (pageIndex > 0) ? (pageIndex - 1) * pageSize : 0;
            sortBy = sortBy ?? "B.TITLE";
            sortDir = sortDir ?? "ASC";
            DynamicParameters dynamicParameters = null;
            if (parameters?.Any() == true)
            {
                dynamicParameters = new DynamicParameters();
                foreach (var item in parameters)
                {
                    dynamicParameters.Add(item.Key, item.Value);
                }
                dynamicParameters.Add("@TotalRows", 0, DbType.Int32, ParameterDirection.Output);
            }
            response.Result = await GetResultFromRawSQL<BookDTO>(SQLQueryBuilder.GetBookingListQuery(whereClause, sortBy, sortDir, skip, pageSize), dynamicParameters);
            response.TotalRows = dynamicParameters.Get<int>("@TotalRows");
            return await Task.Run(() => response);
        }
        public IQueryable<Book> GetBook(Expression<Func<Book, bool>> filter)
        {
            return _dbContext.Books.Where(filter).AsQueryable();
        }
        public async Task<BookImportResponse> ImportSampleBookData(IList<Book> books)
        {
            BookImportResponse response = new() { };
            foreach (var book in books)
            {
                if (string.IsNullOrWhiteSpace(book.ID))
                {
                    book.ID = Guid.NewGuid().ToString();
                    book.IS_ACTIVE = true;
                    SetCreatedBy(book);
                    _dbContext.Add(book);
                    response.InsertedRowsCount++;
                }
                else
                {
                    SetUpdatedBy(book);
                    _dbContext.Update(book);
                    response.UpdatedRowsCount++;
                }
            }
            int commitCount = _unitofWork.Commit();
            response.Success = true;
            return await Task.Run(() => response);
        }


        #region Private Methods
        private async Task<IEnumerable<P>> GetResultFromRawSQL<P>(string sqlQuery, DynamicParameters dynamicParameters = null)
        {
            var result = await _dbConnection.QueryAsync<P>(sqlQuery, dynamicParameters);
            return await Task.Run(() => result);
        }

        private void SetCreatedBy(BaseEntity entity)
        {
            entity.CreatedBy =  "system";
            entity.CreatedDate = DateTime.Now;
        }

        private void SetUpdatedBy(BaseEntity entity, bool isEasUser = false)
        {
            entity.UpdatedBy = "system";
            entity.UpdatedDate = DateTime.Now;
        }
        #endregion

    }
}
