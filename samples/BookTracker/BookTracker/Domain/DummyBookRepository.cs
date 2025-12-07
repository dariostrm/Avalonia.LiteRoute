using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookTracker.Domain;

public class DummyBookRepository : IBookRepository
{
    private readonly List<Book> _books = [];
    
    public event Action<IList<Book>>? BooksChanged;
    
    public void SetDummyData(IList<Book> books)
    {
        _books.Clear();
        _books.AddRange(books);
        BooksChanged?.Invoke(_books);
    }
    
    public Task<IList<Book>> GetBooksAsync()
    {
        return Task.FromResult<IList<Book>>(_books);
    }

    public async Task<Book?> GetBookAsync(Guid id)
    {
        // Simulate async operation
        await Task.Delay(100);
        
        return _books.Find(book => book.Id == id);
    }

    public async Task AddBookAsync(Book book)
    {
        // Simulate async operation
        await Task.Delay(100);
        
        _books.Add(book);
        BooksChanged?.Invoke(_books);
    }

    public async Task UpdateBookAsync(Book book)
    {
        // Simulate async operation
        await Task.Delay(100);
        
        var index = _books.FindIndex(b => b.Id == book.Id);
        if (index != -1)
        {
            _books[index] = book;
            BooksChanged?.Invoke(_books);
        }
    }

    public async Task DeleteBookAsync(Guid bookId)
    {
        // Simulate async operation
        await Task.Delay(100);
        
        var book = _books.Find(b => b.Id == bookId);
        if (book != null)
        {
            _books.Remove(book);
            BooksChanged?.Invoke(_books);
        }
    }

}