using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TechLibrary.Domain;
using TechLibrary.Models;
using TechLibrary.Services;
using System.Linq;

namespace TechLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public BooksController(ILogger<BooksController> logger, IBookService bookService, IMapper mapper)
        {
            _logger = logger;
            _bookService = bookService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int? pageNo, int pageSize = 10, string filterColumnName = "", string filterValue = "", string sortField = "", string sortType = "")
        {
            _logger.LogInformation("Get all books");

            var bookResponse = await _bookService.GetBooksAsync(pageNo, pageSize, filterColumnName, filterValue, sortField, sortType);

            //var bookResponse = _mapper.Map<List<BookResponse>>(books);

            return Ok(bookResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation($"Get book by id {id}");

            var book = await _bookService.GetBookByIdAsync(id);

            var bookResponse = _mapper.Map<BookResponse>(book);

            return Ok(bookResponse);
        }
        [HttpPost]
        public async Task<IActionResult> SaveBook(Book request)
        {
            _logger.LogInformation($"Saving new book");
            var status = await _bookService.CreateBook(request);
            return Ok(true);
        }
        [HttpPut]
        public async Task<IActionResult> Update(Book request)
        {
            //if (request.BookId == 0 || _bookService.GetBookByIdAsync(request.BookId).Result==null)
            //{
            //    return NotFound("Book not found");
            //}
            _logger.LogInformation($"Updating book {request.BookId}");
            var status = await _bookService.UpdateBook(request);
            return Ok(true);
        }
    }
}