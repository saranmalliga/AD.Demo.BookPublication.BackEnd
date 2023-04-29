using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Demo.BookPublication.Domain.Entities
{
    [Table("BOOK", Schema = "BMG")]
    public class Book : SD.BuildingBlocks.Infrastructure.BaseEntity
    {
        public string TITLE { get; set; }
        public string AUTHOR { get; set; }
        public string PUBLISHER { get; set; }
        public string GENRES { get; set; }
        public int TOTAL_PAGES { get; set; }
        public string ISBN { get; set; }
        public int? PUBLISHED_YEAR { get; set; }
        public string BOOK_LANGUAGE { get; set; }
        public bool IS_ACTIVE { get; set; }
    }
}
