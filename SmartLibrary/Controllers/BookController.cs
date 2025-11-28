using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Interfaces;
using SmartLibrary.Models;

namespace SmartLibrary.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _books;

        public BookController(IBookService books)
        {
            _books = books;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _books.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var book = await _books.GetAsync(id);
            return book == null ? NotFound() : Ok(book);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            return Ok(await _books.SearchAsync(keyword));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Book book)
        {
            await _books.AddAsync(book);
            return Ok(book);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Book updated)
        {
            updated.Id = id;
            await _books.UpdateAsync(updated);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _books.DeleteAsync(id);
            return Ok(new { message = "Book deleted" });
        }
    }
}
