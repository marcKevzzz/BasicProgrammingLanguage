using Microsoft.AspNetCore.Mvc;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;
namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Controllers;
public class DELMUNDOMARCKEVIN_241639_WishlistController(TypedCrudStore<DELMUNDOMARCKEVIN_241639_WishlistItem> store) : Controller
{
    public IActionResult Index(string? search, string? priority)
    {
        IEnumerable<DELMUNDOMARCKEVIN_241639_WishlistItem> items = store.GetAll();
        if (!string.IsNullOrWhiteSpace(search))
        { var query = search.Trim(); items = items.Where(item => item.Name.Contains(query, StringComparison.OrdinalIgnoreCase) || item.Category.Contains(query, StringComparison.OrdinalIgnoreCase)); }
        if (!string.IsNullOrWhiteSpace(priority) && priority != "All") items = items.Where(item => item.Priority.Equals(priority, StringComparison.OrdinalIgnoreCase));
        return View(new DELMUNDOMARCKEVIN_241639_WishlistIndexModel(items.ToArray(), search, priority));
    }
    public IActionResult Create() => View(new DELMUNDOMARCKEVIN_241639_WishlistInput { Price = 2500 });
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Create(DELMUNDOMARCKEVIN_241639_WishlistInput input)
    {
        if (!ModelState.IsValid) return View(input);
        var saved = store.Insert(input.ToRecord());
        TempData["Success"] = $"Record #{saved.Id} added successfully.";
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Edit(int id)
    {
        var item = store.GetById(id);
        if (item == null) return Missing(id);
        return View(new DELMUNDOMARCKEVIN_241639_WishlistEditModel(id, DELMUNDOMARCKEVIN_241639_WishlistInput.FromRecord(item)));
    }
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Edit(int id, DELMUNDOMARCKEVIN_241639_WishlistInput input)
    {
        if (store.GetById(id) == null) return Missing(id);
        if (!ModelState.IsValid) return View(new DELMUNDOMARCKEVIN_241639_WishlistEditModel(id, input));
        if (!store.Update(id, input.ToRecord(id))) return Missing(id);
        TempData["Success"] = $"Record #{id} updated successfully.";
        return RedirectToAction(nameof(Index));
    }
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        if (!store.Delete(id)) return Missing(id);
        TempData["Success"] = $"Record #{id} deleted.";
        return RedirectToAction(nameof(Index));
    }
    private IActionResult Missing(int id)
    { TempData["Error"] = $"Record #{id} was not found."; return RedirectToAction(nameof(Index)); }
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult TogglePurchase(int id)
    {
        if (!store.Change(id, item => item with { Purchased = !item.Purchased })) return Missing(id);
        TempData["Success"] = "Purchase status updated.";
        return RedirectToAction(nameof(Index));
    }
}
