using Microsoft.AspNetCore.Mvc;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;
namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Controllers;
public class DELMUNDOMARCKEVIN_241639_AttendanceController(TypedCrudStore<DELMUNDOMARCKEVIN_241639_AttendanceRecord> store) : Controller
{
    public IActionResult Index(string? search, string? status)
    {
        IEnumerable<DELMUNDOMARCKEVIN_241639_AttendanceRecord> items = store.GetAll();
        if (!string.IsNullOrWhiteSpace(search))
        { var query = search.Trim(); items = items.Where(item => item.Name.Contains(query, StringComparison.OrdinalIgnoreCase) || item.StudentNumber.Contains(query, StringComparison.OrdinalIgnoreCase) || item.Course.Contains(query, StringComparison.OrdinalIgnoreCase) || item.Section.Contains(query, StringComparison.OrdinalIgnoreCase)); }
        if (!string.IsNullOrWhiteSpace(status) && status != "All") items = items.Where(item => item.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
        return View(new DELMUNDOMARCKEVIN_241639_AttendanceIndexModel(items.ToArray(), search, status));
    }
    public IActionResult Create() => View(new DELMUNDOMARCKEVIN_241639_AttendanceInput());
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Create(DELMUNDOMARCKEVIN_241639_AttendanceInput input)
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
        return View(new DELMUNDOMARCKEVIN_241639_AttendanceEditModel(id, DELMUNDOMARCKEVIN_241639_AttendanceInput.FromRecord(item)));
    }
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Edit(int id, DELMUNDOMARCKEVIN_241639_AttendanceInput input)
    {
        if (store.GetById(id) == null) return Missing(id);
        if (!ModelState.IsValid) return View(new DELMUNDOMARCKEVIN_241639_AttendanceEditModel(id, input));
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

}
