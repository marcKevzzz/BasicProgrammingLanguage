using Microsoft.AspNetCore.Mvc;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;

namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Controllers;

public class DELMUNDOMARCKEVIN_241639_ArraysController : Controller
{
    [HttpGet] public IActionResult Maze() => View();
    [HttpGet] public IActionResult TheaterSeating() => View();
    [HttpGet] public IActionResult TicTacToe() => View();
    [HttpGet] public IActionResult LargestSmallest() => RedirectToAction(nameof(TheaterSeating));
    [HttpGet] public IActionResult ReverseArray() => RedirectToAction(nameof(TicTacToe));
    [HttpGet] public IActionResult MatrixAddition() => RedirectToAction(nameof(Maze));

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult LargestSmallest(DELMUNDOMARCKEVIN_241639_ArrayInput model) => RedirectToAction(nameof(TheaterSeating));
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult ReverseArray(DELMUNDOMARCKEVIN_241639_ArrayInput model) => RedirectToAction(nameof(TicTacToe));
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult MatrixAddition(DELMUNDOMARCKEVIN_241639_ArrayInput model) => RedirectToAction(nameof(Maze));

    [HttpGet]
    public IActionResult SumAverage() => View(new DELMUNDOMARCKEVIN_241639_ArrayInput {
        Input = string.Join(", ", Enumerable.Range(0, 3).Select(_ => System.Security.Cryptography.RandomNumberGenerator.GetInt32(60, 101)))
    });

    [HttpGet]
    public IActionResult BubbleSort() => View(new DELMUNDOMARCKEVIN_241639_ArrayInput { Input = "42, 7, 19, 3, 28" });

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult SumAverage(DELMUNDOMARCKEVIN_241639_ArrayInput model)
    {
        if (!ModelState.IsValid || !ArrayCalculations.TryNumbers(model.Input, out var values))
            return Invalid(model, "Enter 1–100 grades separated by spaces or commas.");
        if (values.Any(value => value < 0 || value > 100)) return Invalid(model, "Grades must be between 0 and 100.");
        var result = ArrayCalculations.GradeAverage(values);
        ViewBag.GradePassed = result.Passed;
        ViewBag.Result = result.Result;
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult BubbleSort(DELMUNDOMARCKEVIN_241639_ArrayInput model)
    {
        if (!ModelState.IsValid || !ArrayCalculations.TryNumbers(model.Input, out var values))
            return Invalid(model, "Enter 1–100 numbers between -1000000 and 1000000, separated by spaces or commas.");
        if (model.Direction is not ("ascending" or "descending")) return Invalid(model, "Choose a sort direction.");
        ViewBag.Result = ArrayCalculations.BubbleSort(values, model.Direction);
        return View(model);
    }

    private IActionResult Invalid(DELMUNDOMARCKEVIN_241639_ArrayInput model, string message)
    {
        model.Input ??= string.Empty;
        ViewBag.Error = message;
        return View(model);
    }
}
