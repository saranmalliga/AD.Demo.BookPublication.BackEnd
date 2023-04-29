using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Demo.BookPublication.SQL.Infrastructure.Utility
{
    public static class SQLQueryBuilder
    {
        private static Dictionary<string, string> _booksSearchWhereClauseParams = new Dictionary<string, string> {
            { "title", "B.TITLE" },
            { "author", "B.AUTHOR" },
            { "publisher", "B.PUBLISHER" },
            { "genres", "B.GENRES" },
            { "isbn", "B.ISBN" }
        }; 
        public static string GetBookingListQuery(string whereClause = "", string orderBy = "B.TITLE", string orderDir = "ASC", int skip = 0, int take = 20) => @$"
                                                          SELECT @TotalRows = COUNT(1) FROM [BMG].[BOOK] B WHERE B.[IS_ACTIVE] = 1 {whereClause};
                                                          SELECT B.[ID] AS Id
                                                              ,B.[TITLE] AS Title
                                                              ,B.[AUTHOR] AS Author
                                                              ,B.[PUBLISHER] AS Publisher
                                                              ,B.[GENRES] AS Genres
                                                              ,B.[TOTAL_PAGES] AS TotalPages
                                                              ,B.[ISBN] AS ISBN
                                                              ,B.[PUBLISHED_YEAR] AS PublishedYear
                                                          FROM [BMG].[BOOK] B
                                                          WHERE B.[IS_ACTIVE] = 1
                                                          {whereClause}
                                                          ORDER BY {orderBy} {orderDir}
                                                          OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY 
                                                            ";

        public static string GetBookSearchWhereClauese(string filterBy)
        {
            string filterColumn = string.Empty;
            if (string.IsNullOrEmpty(filterBy) || filterBy?.ToLower() == "all")
            {
                return $" AND (B.TITLE LIKE '%'+@SearchValue+'%' OR B.AUTHOR LIKE '%'+@SearchValue+'%' OR B.PUBLISHER LIKE '%'+@SearchValue+'%'  OR B.GENRES LIKE '%'+@SearchValue+'%'  OR B.ISBN LIKE '%'+@SearchValue+'%')";
            }
            if (!string.IsNullOrEmpty(filterBy))
            {
                filterColumn = _booksSearchWhereClauseParams.Any(w => w.Key == filterBy.ToLower()) ? _booksSearchWhereClauseParams.FirstOrDefault(w => w.Key == filterBy.ToLower()).Value : string.Empty;
                if (!string.IsNullOrEmpty(filterColumn))
                {
                    return $" AND {filterColumn} LIKE '%'+@SearchValue+'%'";
                }
            }
            return string.Empty;
        }





    }
}
