using Microsoft.AspNetCore.Mvc;
using _24_1639BasicProgrammingLanguage.Services;

namespace _24_1639BasicProgrammingLanguage.Controllers
{
    public class LibraryController : Controller
    {
        private readonly InMemoryCrudStore _store;

        public LibraryController(InMemoryCrudStore store)
        {
            _store = store;
        }

        // GET: /Library or /Library/Index?search=clean
        public IActionResult Index(string? search)
        {
            var books = _store.GetAll("library");

            if (!string.IsNullOrWhiteSpace(search))
            {
                string query = search.Trim().ToLowerInvariant();
                books = books.Where(b =>
                    (b.ContainsKey("title") && b["title"].ToString()!.ToLowerInvariant().Contains(query)) ||
                    (b.ContainsKey("author") && b["author"].ToString()!.ToLowerInvariant().Contains(query)) ||
                    (b.ContainsKey("genre") && b["genre"].ToString()!.ToLowerInvariant().Contains(query))
                ).ToList();
                ViewBag.Search = search;
            }

            ViewBag.Books = books;
            return View();
        }

        // GET: /Library/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Library/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string? title, string? author, string? genre, int? year)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                ViewBag.Error = "Book title is required.";
                ViewBag.TitleInput = title;
                ViewBag.AuthorInput = author;
                ViewBag.GenreInput = genre;
                ViewBag.YearInput = year;
                return View();
            }

            if (string.IsNullOrWhiteSpace(author))
            {
                ViewBag.Error = "Author name is required.";
                ViewBag.TitleInput = title;
                ViewBag.AuthorInput = author;
                ViewBag.GenreInput = genre;
                ViewBag.YearInput = year;
                return View();
            }

            int currentYear = DateTime.Now.Year;
            if (!year.HasValue || year.Value < 1400 || year.Value > currentYear + 1)
            {
                ViewBag.Error = $"Publication year must be between 1400 and {currentYear + 1}.";
                ViewBag.TitleInput = title;
                ViewBag.AuthorInput = author;
                ViewBag.GenreInput = genre;
                ViewBag.YearInput = year;
                return View();
            }

            var book = new Dictionary<string, object>
            {
                ["title"] = title.Trim(),
                ["author"] = author.Trim(),
                ["genre"] = string.IsNullOrWhiteSpace(genre) ? "General" : genre.Trim(),
                ["year"] = year.Value
            };

            int newId = _store.Insert("library", book);
            TempData["Success"] = $"Book #{newId} '{book["title"]}' was successfully added to the catalog.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Library/Edit/1
        public IActionResult Edit(int id)
        {
            var book = _store.GetById("library", id);
            if (book == null)
            {
                TempData["Error"] = $"Book with ID #{id} was not found.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Book = book;
            return View();
        }

        // POST: /Library/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, string? title, string? author, string? genre, int? year)
        {
            var existing = _store.GetById("library", id);
            if (existing == null)
            {
                TempData["Error"] = $"Book #{id} could not be found to update.";
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author))
            {
                ViewBag.Error = "Title and Author are both mandatory fields.";
                ViewBag.Book = new Dictionary<string, object>
                {
                    ["id"] = id,
                    ["title"] = title ?? "",
                    ["author"] = author ?? "",
                    ["genre"] = genre ?? "",
                    ["year"] = year ?? 2000
                };
                return View();
            }

            var updated = new Dictionary<string, object>
            {
                ["title"] = title.Trim(),
                ["author"] = author.Trim(),
                ["genre"] = string.IsNullOrWhiteSpace(genre) ? "General" : genre.Trim(),
                ["year"] = year ?? 2000
            };

            _store.Update("library", id, updated);
            TempData["Success"] = $"Book #{id} '{updated["title"]}' was successfully updated.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /Library/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            bool removed = _store.Delete("library", id);
            if (removed)
            {
                TempData["Success"] = $"Book record #{id} was successfully deleted.";
            }
            else
            {
                TempData["Error"] = $"Could not delete Book #{id}; record not found.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
