using Microsoft.AspNetCore.Mvc;
using _24_1639BasicProgrammingLanguage.Services;

namespace _24_1639BasicProgrammingLanguage.Controllers
{
    public class LostFoundController : Controller
    {
        private readonly InMemoryCrudStore _store;

        public LostFoundController(InMemoryCrudStore store)
        {
            _store = store;
        }

        // GET: /LostFound
        public IActionResult Index(string? search, string? status)
        {
            var items = _store.GetAll("lostfound");

            if (!string.IsNullOrWhiteSpace(search))
            {
                string query = search.Trim().ToLowerInvariant();
                items = items.Where(x =>
                    (x.ContainsKey("name") && x["name"].ToString()!.ToLowerInvariant().Contains(query)) ||
                    (x.ContainsKey("description") && x["description"].ToString()!.ToLowerInvariant().Contains(query)) ||
                    (x.ContainsKey("location") && x["location"].ToString()!.ToLowerInvariant().Contains(query))
                ).ToList();
                ViewBag.Search = search;
            }

            if (!string.IsNullOrWhiteSpace(status) && status != "All")
            {
                items = items.Where(x =>
                    x.ContainsKey("status") && x["status"].ToString()!.Equals(status, StringComparison.OrdinalIgnoreCase)
                ).ToList();
                ViewBag.StatusFilter = status;
            }

            ViewBag.Reports = items;
            return View();
        }

        // GET: /LostFound/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /LostFound/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string? name, string? description, string? location, string? date, string? status)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ViewBag.Error = "Item name is required.";
                ViewBag.NameInput = name;
                ViewBag.DescriptionInput = description;
                ViewBag.LocationInput = location;
                ViewBag.DateInput = date;
                ViewBag.StatusInput = status;
                return View();
            }

            if (string.IsNullOrWhiteSpace(location))
            {
                ViewBag.Error = "Location is required.";
                ViewBag.NameInput = name;
                ViewBag.DescriptionInput = description;
                ViewBag.LocationInput = location;
                ViewBag.DateInput = date;
                ViewBag.StatusInput = status;
                return View();
            }

            var report = new Dictionary<string, object>
            {
                ["name"] = name.Trim(),
                ["description"] = description?.Trim() ?? "No additional description provided.",
                ["location"] = location.Trim(),
                ["date"] = string.IsNullOrWhiteSpace(date) ? DateTime.Now.ToString("yyyy-MM-dd") : date.Trim(),
                ["status"] = string.IsNullOrWhiteSpace(status) ? "Found" : status.Trim()
            };

            int newId = _store.Insert("lostfound", report);
            TempData["Success"] = $"Report #{newId} '{report["name"]}' was registered successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /LostFound/Edit/1
        public IActionResult Edit(int id)
        {
            var report = _store.GetById("lostfound", id);
            if (report == null)
            {
                TempData["Error"] = $"Report #{id} was not found.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Report = report;
            return View();
        }

        // POST: /LostFound/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, string? name, string? description, string? location, string? date, string? status)
        {
            var existing = _store.GetById("lostfound", id);
            if (existing == null)
            {
                TempData["Error"] = $"Report #{id} was not found.";
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(location))
            {
                ViewBag.Error = "Item name and Location are required.";
                ViewBag.Report = new Dictionary<string, object>
                {
                    ["id"] = id,
                    ["name"] = name ?? "",
                    ["description"] = description ?? "",
                    ["location"] = location ?? "",
                    ["date"] = date ?? "",
                    ["status"] = status ?? "Found"
                };
                return View();
            }

            var updated = new Dictionary<string, object>
            {
                ["name"] = name.Trim(),
                ["description"] = description?.Trim() ?? "",
                ["location"] = location.Trim(),
                ["date"] = string.IsNullOrWhiteSpace(date) ? DateTime.Now.ToString("yyyy-MM-dd") : date.Trim(),
                ["status"] = string.IsNullOrWhiteSpace(status) ? "Found" : status.Trim()
            };

            _store.Update("lostfound", id, updated);
            TempData["Success"] = $"Report #{id} '{updated["name"]}' was updated.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /LostFound/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            bool removed = _store.Delete("lostfound", id);
            if (removed)
            {
                TempData["Success"] = $"Report #{id} was removed.";
            }
            else
            {
                TempData["Error"] = $"Report #{id} could not be deleted.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
