using BookApi.Models;
using BookApi.Repository;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BooksRepository _booksRepository;

        public BooksController(BooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var books = _booksRepository.Get();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var book = _booksRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Book book)
        {
            var addedBook = _booksRepository.Add(book);
            return Created(new Uri(Request.GetEncodedUrl() + "/" + addedBook.Id), addedBook);
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] Guid id,[FromBody] Book book)
        {
            var updatedBook = _booksRepository.Update(id, book);
            if(updatedBook == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            _booksRepository.Delete(id);
            return NoContent();
        }
    }
}
