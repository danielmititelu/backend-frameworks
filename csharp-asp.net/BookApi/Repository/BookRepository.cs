using BookApi.Models;
using System;
using System.Collections.Generic;

namespace BookApi.Repository
{
    public class BookRepository
    {
        private Dictionary<Guid, Book> _books;

        public BookRepository()
        {
            _books = new Dictionary<Guid, Book>();
        }

        public IEnumerable<Book> Get()
        {
            return _books.Values;
        }

        public Book GetById(Guid id)
        {
            if (_books.TryGetValue(id, out var book))
                return book;

            return null;
        }

        public Book Add(Book book)
        {
            book.Id = Guid.NewGuid();
            _books.Add(book.Id, book);
            return book;
        }

        public Book Update(Guid id, Book book)
        {
            if (!_books.ContainsKey(id))
            {
                return null;
            }
            book.Id = id;
            _books[id] = book;
            return book;
        }

        public void Delete(Guid id)
        {
            _books.Remove(id);
        }
    }
}
