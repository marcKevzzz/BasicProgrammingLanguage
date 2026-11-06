using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;

namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;

// Classroom data is kept in memory and resets when the application restarts.
public sealed class LibraryStore
{
    private readonly object _lock = new();
    private readonly Dictionary<int, DELMUNDOMARCKEVIN_241639_Book> _books = new()
    {
        [1] = new(1, "The Pragmatic Programmer", "Andrew Hunt & David Thomas", "Computer Science", 1999),
        [2] = new(2, "Clean Code", "Robert C. Martin", "Software Engineering", 2008),
        [3] = new(3, "Design Patterns: Elements of Reusable Object-Oriented Software", "Erich Gamma et al.", "Architecture", 1994),
        [4] = new(4, "Code Complete", "Steve McConnell", "Software Engineering", 2004)
    };
    private int _nextId = 4;

    public IReadOnlyList<DELMUNDOMARCKEVIN_241639_Book> Search(string? search)
    {
        DELMUNDOMARCKEVIN_241639_Book[] snapshot;
        lock (_lock) snapshot = _books.Values.ToArray();
        if (string.IsNullOrWhiteSpace(search)) return snapshot;
        var query = search.Trim();
        return snapshot.Where(book =>
            book.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            book.Author.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            book.Genre.Contains(query, StringComparison.OrdinalIgnoreCase)).ToArray();
    }

    public DELMUNDOMARCKEVIN_241639_Book? GetById(int id)
    {
        lock (_lock) return _books.GetValueOrDefault(id);
    }

    public DELMUNDOMARCKEVIN_241639_Book Insert(DELMUNDOMARCKEVIN_241639_BookInput input)
    {
        var book = CreateBook(0, input);
        lock (_lock)
        {
            book = book with { Id = ++_nextId };
            _books.Add(book.Id, book);
            return book;
        }
    }

    public bool Update(int id, DELMUNDOMARCKEVIN_241639_BookInput input)
    {
        var book = CreateBook(id, input);
        lock (_lock)
        {
            if (!_books.ContainsKey(id)) return false;
            _books[id] = book;
            return true;
        }
    }

    public bool Delete(int id)
    {
        lock (_lock) return _books.Remove(id);
    }

    private static DELMUNDOMARCKEVIN_241639_Book CreateBook(int id, DELMUNDOMARCKEVIN_241639_BookInput input)
    {
        var error = CrudValidation.DELMUNDOMARCKEVIN_241639_Book(input.Title, input.Author, input.Year);
        if (error != null) throw new ArgumentException(error, nameof(input));
        return new(id, input.Title!.Trim(), input.Author!.Trim(),
            string.IsNullOrWhiteSpace(input.Genre) ? "General" : input.Genre.Trim(), input.Year!.Value);
    }
}
