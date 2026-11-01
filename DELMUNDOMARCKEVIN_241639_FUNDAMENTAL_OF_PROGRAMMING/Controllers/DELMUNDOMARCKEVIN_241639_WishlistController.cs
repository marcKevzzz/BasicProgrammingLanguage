using Microsoft.AspNetCore.Mvc;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;

namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Controllers
{
    public class DELMUNDOMARCKEVIN_241639_WishlistController : Controller
    {
        private readonly InMemoryCrudStore _store;

        public DELMUNDOMARCKEVIN_241639_WishlistController(InMemoryCrudStore store)
        {
            _store = store;
        }

        // GET: /Wishlist
        public IActionResult Index(string? search, string? priority)
        {
            var items = _store.GetAll("wishlist");

            if (!string.IsNullOrWhiteSpace(search))
            {
                string query = search.Trim().ToLowerInvariant();
                items = items.Where(x =>
                    (x.ContainsKey("name") && x["name"].ToString()!.ToLowerInvariant().Contains(query)) ||
                    (x.ContainsKey("category") && x["category"].ToString()!.ToLowerInvariant().Contains(query))
                ).ToList();
                ViewBag.Search = search;
            }

            if (!string.IsNullOrWhiteSpace(priority) && priority != "All")
            {
                items = items.Where(x =>
                    x.ContainsKey("priority") && x["priority"].ToString()!.Equals(priority, StringComparison.OrdinalIgnoreCase)
                ).ToList();
                ViewBag.PriorityFilter = priority;
            }

            ViewBag.Wishlist = items;
            return View();
        }

        // GET: /DELMUNDOMARCKEVIN_241639_Wishlist/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /DELMUNDOMARCKEVIN_241639_Wishlist/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string? name, string? category, double? price, string? priority, bool purchased)
        {
            string? validationError = !ModelState.IsValid ? "Check the entered values and try again." : CrudValidation.Wishlist(name, price, priority);
            if (validationError != null)
            {
                ViewBag.Error = validationError;
                ViewBag.NameInput = name;
                ViewBag.CategoryInput = category;
                ViewBag.PriceInput = price;
                ViewBag.PriorityInput = priority;
                return View();
            }

            var item = new Dictionary<string, object>
            {
                ["name"] = name!.Trim(),
                ["category"] = string.IsNullOrWhiteSpace(category) ? "General" : category.Trim(),
                ["price"] = price!.Value,
                ["priority"] = string.IsNullOrWhiteSpace(priority) ? "Medium" : priority.Trim(),
                ["purchased"] = purchased
            };

            int newId = _store.Insert("wishlist", item);
            TempData["Success"] = $"Wishlist item #{newId} '{item["name"]}' was added successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /DELMUNDOMARCKEVIN_241639_Wishlist/Edit/1
        public IActionResult Edit(int id)
        {
            var item = _store.GetById("wishlist", id);
            if (item == null)
            {
                TempData["Error"] = $"Wishlist item #{id} not found.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Item = item;
            return View();
        }

        // POST: /DELMUNDOMARCKEVIN_241639_Wishlist/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, string? name, string? category, double? price, string? priority, bool purchased)
        {
            var existing = _store.GetById("wishlist", id);
            if (existing == null)
            {
                TempData["Error"] = $"Wishlist item #{id} not found.";
                return RedirectToAction(nameof(Index));
            }

            string? validationError = !ModelState.IsValid ? "Check the entered values and try again." : CrudValidation.Wishlist(name, price, priority);
            if (validationError != null)
            {
                ViewBag.Error = validationError;
                ViewBag.Item = new Dictionary<string, object>
                {
                    ["id"] = id,
                    ["name"] = name ?? "",
                    ["category"] = category ?? "",
                    ["price"] = price ?? 0,
                    ["priority"] = priority ?? "Medium",
                    ["purchased"] = purchased
                };
                return View();
            }

            var updated = new Dictionary<string, object>
            {
                ["name"] = name!.Trim(),
                ["category"] = string.IsNullOrWhiteSpace(category) ? "General" : category.Trim(),
                ["price"] = price ?? 0,
                ["priority"] = string.IsNullOrWhiteSpace(priority) ? "Medium" : priority.Trim(),
                ["purchased"] = purchased
            };

            _store.Update("wishlist", id, updated);
            TempData["Success"] = $"Wishlist item #{id} '{updated["name"]}' updated.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /DELMUNDOMARCKEVIN_241639_Wishlist/TogglePurchase/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TogglePurchase(int id)
        {
            var item = _store.GetById("wishlist", id);
            if (item != null)
            {
                bool current = Convert.ToBoolean(item["purchased"]);
                _store.Update("wishlist", id, new Dictionary<string, object> { ["purchased"] = !current });
                TempData["Success"] = $"Status updated: '{item["name"]}' marked as {(current ? "Pending" : "Purchased")}.";
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: /DELMUNDOMARCKEVIN_241639_Wishlist/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            bool removed = _store.Delete("wishlist", id);
            if (removed)
            {
                TempData["Success"] = $"Wishlist item #{id} was deleted.";
            }
            else
            {
                TempData["Error"] = $"Wishlist item #{id} not found.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
