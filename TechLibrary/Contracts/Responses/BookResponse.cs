using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechLibrary.Models
{
    public class BookResponse
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int TotalRecordCount { get; set; }
        public List<BookData> Data { get; set; } = new List<BookData>();
    }
    public class BookData
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string PublishedDate { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Descr { get; set; }
    }
}
