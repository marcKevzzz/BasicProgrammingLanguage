using Microsoft.AspNetCore.Mvc;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;

namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Controllers
{
    public class DELMUNDOMARCKEVIN_241639_LibraryController : Controller
    {
        private readonly InMemoryCrudStore _store;

        public DELMUNDOMARCKEVIN_241639_LibraryController(InMemoryCrudStore store)
        {
            _store = store;
        }

        // GET: /Library or /DELMUNDOMARCKEVIN_241639_Library/Index?search=clean
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

        // GET: /DELMUNDOMARCKEVIN_241639_Library/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /DELMUNDOMARCKEVIN_241639_Library/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string? title, string? author, string? genre, int? year)
        {
            string? validationError = !ModelState.IsValid ? "Check the entered values and try again." : CrudValidation.Book(title, author, year);
            if (validationError != null)
            {
                ViewBag.Error = validationError;
                ViewBag.TitleInput = title;
                ViewBag.AuthorInput = author;
                ViewBag.GenreInput = genre;
                ViewBag.YearInput = year;
                return View();
            }

            var book = new Dictionary<string, object>
            {
                ["title"] = title!.Trim(),
                ["author"] = author!.Trim(),
                ["genre"] = string.IsNullOrWhiteSpace(genre) ? "General" : genre.Trim(),
                ["year"] = year!.Value
            };

            int newId = _store.Insert("library", book);
            TempData["Success"] = $"Book #{newId} '{book["title"]}' was successfully added to the catalog.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /DELMUNDOMARCKEVIN_241639_Library/Edit/1
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

        // POST: /DELMUNDOMARCKEVIN_241639_Library/Edit/1
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

            string? validationError = !ModelState.IsValid ? "Check the entered values and try again." : CrudValidation.Book(title, author, year);
            if (validationError != null)
            {
                ViewBag.Error = validationError;
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
                ["title"] = title!.Trim(),
                ["author"] = author!.Trim(),
                ["genre"] = string.IsNullOrWhiteSpace(genre) ? "General" : genre.Trim(),
                ["year"] = year ?? 2000
            };

            _store.Update("library", id, updated);
            TempData["Success"] = $"Book #{id} '{updated["title"]}' was successfully updated.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /DELMUNDOMARCKEVIN_241639_Library/Delete/1
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
