using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookTracker.Domain;

public interface IBookRepository
{
    Task<IList<Book>> GetBooksAsync();
    Task<Book?> GetBookAsync(Guid id);
    
    Task AddBookAsync(Book book);
    Task UpdateBookAsync(Book book);
    Task DeleteBookAsync(Guid bookId);
    
    event Action<IList<Book>>? BooksChanged;
}