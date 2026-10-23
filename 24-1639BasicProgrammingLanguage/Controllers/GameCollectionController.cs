using Microsoft.AspNetCore.Mvc;
using _24_1639BasicProgrammingLanguage.Services;

namespace _24_1639BasicProgrammingLanguage.Controllers
{
    public class GameCollectionController : Controller
    {
        private readonly InMemoryCrudStore _store;

        public GameCollectionController(InMemoryCrudStore store)
        {
            _store = store;
        }

        // GET: /GameCollection
        public IActionResult Index(string? search, string? status)
        {
            var games = _store.GetAll("games");

            if (!string.IsNullOrWhiteSpace(search))
            {
                string query = search.Trim().ToLowerInvariant();
                games = games.Where(x =>
                    (x.ContainsKey("title") && x["title"].ToString()!.ToLowerInvariant().Contains(query)) ||
                    (x.ContainsKey("genre") && x["genre"].ToString()!.ToLowerInvariant().Contains(query)) ||
                    (x.ContainsKey("platform") && x["platform"].ToString()!.ToLowerInvariant().Contains(query))
                ).ToList();
                ViewBag.Search = search;
            }

            if (!string.IsNullOrWhiteSpace(status) && status != "All")
            {
                games = games.Where(x =>
                    x.ContainsKey("status") && x["status"].ToString()!.Equals(status, StringComparison.OrdinalIgnoreCase)
                ).ToList();
                ViewBag.StatusFilter = status;
            }

            ViewBag.Games = games;
            return View();
        }

        // GET: /GameCollection/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /GameCollection/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string? title, string? genre, string? platform, string? status, int? rating)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                ViewBag.Error = "Game title is required.";
                ViewBag.TitleInput = title;
                ViewBag.GenreInput = genre;
                ViewBag.PlatformInput = platform;
                ViewBag.StatusInput = status;
                ViewBag.RatingInput = rating;
                return View();
            }

            if (!rating.HasValue || rating.Value < 1 || rating.Value > 5)
            {
                ViewBag.Error = "Rating must be between 1 and 5 stars.";
                ViewBag.TitleInput = title;
                ViewBag.GenreInput = genre;
                ViewBag.PlatformInput = platform;
                ViewBag.StatusInput = status;
                ViewBag.RatingInput = rating;
                return View();
            }

            var game = new Dictionary<string, object>
            {
                ["title"] = title.Trim(),
                ["genre"] = string.IsNullOrWhiteSpace(genre) ? "General" : genre.Trim(),
                ["platform"] = string.IsNullOrWhiteSpace(platform) ? "PC" : platform.Trim(),
                ["status"] = string.IsNullOrWhiteSpace(status) ? "Backlog" : status.Trim(),
                ["rating"] = rating.Value
            };

            int newId = _store.Insert("games", game);
            TempData["Success"] = $"Game #{newId} '{game["title"]}' was added to your library.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /GameCollection/Edit/1
        public IActionResult Edit(int id)
        {
            var game = _store.GetById("games", id);
            if (game == null)
            {
                TempData["Error"] = $"Game #{id} was not found.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Game = game;
            return View();
        }

        // POST: /GameCollection/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, string? title, string? genre, string? platform, string? status, int? rating)
        {
            var existing = _store.GetById("games", id);
            if (existing == null)
            {
                TempData["Error"] = $"Game #{id} not found.";
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                ViewBag.Error = "Game title cannot be blank.";
                ViewBag.Game = new Dictionary<string, object>
                {
                    ["id"] = id,
                    ["title"] = title ?? "",
                    ["genre"] = genre ?? "",
                    ["platform"] = platform ?? "",
                    ["status"] = status ?? "Backlog",
                    ["rating"] = rating ?? 5
                };
                return View();
            }

            var updated = new Dictionary<string, object>
            {
                ["title"] = title.Trim(),
                ["genre"] = string.IsNullOrWhiteSpace(genre) ? "General" : genre.Trim(),
                ["platform"] = string.IsNullOrWhiteSpace(platform) ? "PC" : platform.Trim(),
                ["status"] = string.IsNullOrWhiteSpace(status) ? "Backlog" : status.Trim(),
                ["rating"] = rating.HasValue && rating.Value >= 1 && rating.Value <= 5 ? rating.Value : 5
            };

            _store.Update("games", id, updated);
            TempData["Success"] = $"Game #{id} '{updated["title"]}' updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /GameCollection/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            bool removed = _store.Delete("games", id);
            if (removed)
            {
                TempData["Success"] = $"Game #{id} was deleted from collection.";
            }
            else
            {
                TempData["Error"] = $"Game #{id} could not be deleted.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
