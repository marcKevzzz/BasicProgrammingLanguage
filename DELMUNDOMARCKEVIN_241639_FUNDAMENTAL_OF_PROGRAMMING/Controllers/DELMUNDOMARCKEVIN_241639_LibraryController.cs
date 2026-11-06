using Microsoft.AspNetCore.Mvc;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;

namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Controllers;

public class DELMUNDOMARCKEVIN_241639_LibraryController(LibraryStore store) : Controller
{
    public IActionResult Index(string? search) => View(new DELMUNDOMARCKEVIN_241639_LibraryIndexModel(store.Search(search), search));

    public IActionResult Create() => View(new DELMUNDOMARCKEVIN_241639_BookInput { Year = DateTime.Now.Year });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(DELMUNDOMARCKEVIN_241639_BookInput input)
    {
        if (!ModelState.IsValid) return View(input);
        var book = store.Insert(input);
        TempData["Success"] = $"Book #{book.Id} '{book.Title}' was successfully added to the catalog.";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {
        var book = store.GetById(id);
        if (book == null) return MissingBook(id);
        return View(new DELMUNDOMARCKEVIN_241639_BookEditModel(id, new DELMUNDOMARCKEVIN_241639_BookInput
        {
            Title = book.Title, Author = book.Author, Genre = book.Genre, Year = book.Year
        }));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, DELMUNDOMARCKEVIN_241639_BookInput input)
    {
        if (store.GetById(id) == null) return MissingBook(id);
        if (!ModelState.IsValid) return View(new DELMUNDOMARCKEVIN_241639_BookEditModel(id, input));
        if (!store.Update(id, input)) return MissingBook(id);
        TempData["Success"] = $"Book #{id} '{input.Title!.Trim()}' was successfully updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        if (!store.Delete(id)) return MissingBook(id);
        TempData["Success"] = $"Book record #{id} was successfully deleted.";
        return RedirectToAction(nameof(Index));
    }

    private IActionResult MissingBook(int id)
    {
        TempData["Error"] = $"Book #{id} was not found.";
        return RedirectToAction(nameof(Index));
    }
}
