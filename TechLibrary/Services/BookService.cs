using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TechLibrary.Data;
using TechLibrary.Domain;
using TechLibrary.Models;

namespace TechLibrary.Services
{
    public interface IBookService
    {
        Task<BookResponse> GetBooksAsync(int? pageNo, int pageSize, string filterColumnName = "", string filterValue = "", string sortField = "", string sortType = "");
        Task<Book> GetBookByIdAsync(int bookid);
        Task<bool> CreateBook(Book request);
        Task<bool> UpdateBook(BookData request);
    }

    public class BookService : IBookService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        public BookService(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<BookResponse> GetBooksAsync(int? pageNo, int pageSize, string filterColumnName = "", string filterValue = "", string sortField = "", string sortType = "")
        {

            var queryable = _dataContext.Books.AsQueryable();
            if (string.IsNullOrEmpty(filterColumnName) && !string.IsNullOrEmpty(filterValue))
            {
                queryable = queryable.Where(i => i.Title.Contains(filterValue)
                || i.ShortDescr.Contains(filterValue));
            }

            for (int index = 0; index < filterColumnName.Split(',').Length; index++)
            {
                string colName = filterColumnName.Split(',')[index].ToLower();
                string colValue = filterValue.Split(',')[index];

                switch (colName)
                {
                    case "title":
                        queryable = queryable.Where(i => i.Title.Contains(colValue));
                        break;
                    case "shortdescr":
                        queryable = queryable.Where(i => i.ShortDescr.Contains(colValue));
                        break;
                    case "longdescr":
                        queryable = queryable.Where(i => i.LongDescr.Contains(colValue));
                        break;
                    default:
                        // column not handled - nothing to filter
                        break;
                }
            }

            for (int index = 0; index < sortField.Split(',').Length; index++)
            {
                string colName = sortField.Split(',')[index].ToLower();
                string type = sortType.Split(',')[index].ToLower();

                switch (colName)
                {
                    case "title":
                        if (type == "asc")
                        {
                            queryable = queryable.OrderBy(x => x.Title);
                        }
                        else
                        {
                            queryable = queryable.OrderByDescending(x => x.Title);
                        }
                        break;
                    case "longdescr":
                        if (type == "asc")
                        {
                            queryable = queryable.OrderBy(x => x.LongDescr);
                        }
                        else
                        {
                            queryable = queryable.OrderByDescending(x => x.LongDescr);
                        }
                        break;
                    case "shortdescr":
                        if (type == "asc")
                        {
                            queryable = queryable.OrderBy(x => x.ShortDescr);
                        }
                        else
                        {
                            queryable = queryable.OrderByDescending(x => x.ShortDescr);
                        }
                        break;
                    default:
                        // column not handled - nothing to filter
                        break;
                }
            }
            var totalCount = queryable.Count();

            queryable = queryable.Skip(pageSize * (pageNo == null ? 0 : (pageNo.Value -1))).Take(pageSize);
           
            var response = new BookResponse
            {
                Data = _mapper.Map<List<BookData>>(await queryable.ToListAsync()),
                PageNo = (pageNo == null ? 0 : (pageNo.Value - 1)),
                PageSize = pageSize,
                TotalRecordCount = totalCount
            };
            return response;
        }

        public async Task<Book> GetBookByIdAsync(int bookid)
        {
            return await _dataContext.Books.SingleOrDefaultAsync(x => x.BookId == bookid);
        }

        public async Task<bool> CreateBook(Book request)
        {
            _dataContext.Books.Add(request);
            return await _dataContext.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateBook(BookData request)
        {
            var book = _dataContext.Books.FirstOrDefault(x => x.BookId == request.BookId);
            book.ISBN = request.ISBN;
            //book.LongDescr = request.Descr;
            book.ShortDescr = request.Descr;
            book.ThumbnailUrl = request.ThumbnailUrl;
            book.Title = request.Title;
            book.PublishedDate = request.PublishedDate;
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}

