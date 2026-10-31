using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;

namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Controllers;

public class DELMUNDOMARCKEVIN_241639_ArraysController : Controller
{
    [HttpGet] public IActionResult Maze() => View();
    [HttpGet] public IActionResult TheaterSeating() => View();
    [HttpGet] public IActionResult TicTacToe() => View();
    [HttpGet] public IActionResult LargestSmallest() => RedirectToAction(nameof(TheaterSeating));
    [HttpGet] public IActionResult SumAverage() => View(new DELMUNDOMARCKEVIN_241639_ArrayInput { Input = string.Join(", ", Enumerable.Range(0, 3).Select(_ => System.Security.Cryptography.RandomNumberGenerator.GetInt32(60, 101))) });
    [HttpGet] public IActionResult ReverseArray() => RedirectToAction(nameof(TicTacToe));
    [HttpGet] public IActionResult BubbleSort() => View(new DELMUNDOMARCKEVIN_241639_ArrayInput { Input = "42, 7, 19, 3, 28" });
    [HttpGet] public IActionResult MatrixAddition() => RedirectToAction(nameof(Maze));

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult LargestSmallest(DELMUNDOMARCKEVIN_241639_ArrayInput model)
    {
        if (!ModelState.IsValid || !TryMatrix(model.Input, out var matrix)) return Invalid(model, "Enter a rectangular matrix, up to 10 × 10.");
        var transposed = new decimal[matrix.GetLength(1), matrix.GetLength(0)];
        for (int row = 0; row < matrix.GetLength(0); row++)
            for (int col = 0; col < matrix.GetLength(1); col++) transposed[col, row] = matrix[row, col];
        ViewBag.Result = FormatMatrix(transposed);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult SumAverage(DELMUNDOMARCKEVIN_241639_ArrayInput model)
    {
        if (!ModelState.IsValid || !TryNumbers(model.Input, out var values))
            return Invalid(model, "Enter 1–100 numbers between -1000000 and 1000000, separated by spaces or commas.");
        if (values.Any(value => value < 0 || value > 100)) return Invalid(model, "Grades must be between 0 and 100.");
        decimal total = 0;
        for (int i = 0; i < values.Length; i++) total += values[i];
        decimal average = total / values.Length;
        average = Math.Round(average, 2, MidpointRounding.AwayFromZero);
        var equivalent = average switch
        {
            >= 96.50m => (1.00m, "Excellent"),
            >= 92.50m => (1.25m, "Very Good"),
            >= 88.50m => (1.50m, "Very Good"),
            >= 84.50m => (1.75m, "Good"),
            >= 80.50m => (2.00m, "Good"),
            >= 76.50m => (2.25m, "Satisfactory"),
            >= 72.50m => (2.50m, "Satisfactory"),
            >= 68.50m => (2.75m, "Fair"),
            >= 64.50m => (3.00m, "Fair"),
            _ => (5.00m, "Poor / Failed")
        };
        string remarks = average >= 64.50m ? "Passed" : "Failed";
        ViewBag.GradePassed = average >= 64.50m;
        ViewBag.Result = FormattableString.Invariant($"General weighted average: {average:0.00}%\nNumerical grade: {equivalent.Item1:0.00}\nDescriptive rating: {equivalent.Item2}\nRemarks: {remarks}");
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult ReverseArray(DELMUNDOMARCKEVIN_241639_ArrayInput model)
    {
        if (!ModelState.IsValid || !TryMatrix(model.Input, out var matrix)) return Invalid(model, "Enter a rectangular matrix, up to 10 × 10.");
        var sums = new decimal[matrix.GetLength(0), 1];
        for (int row = 0; row < matrix.GetLength(0); row++)
            for (int col = 0; col < matrix.GetLength(1); col++) sums[row, 0] += matrix[row, col];
        ViewBag.Result = FormatMatrix(sums);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult BubbleSort(DELMUNDOMARCKEVIN_241639_ArrayInput model)
    {
        if (!ModelState.IsValid || !TryNumbers(model.Input, out var values))
            return Invalid(model, "Enter 1–100 numbers between -1000000 and 1000000, separated by spaces or commas.");
        if (model.Direction is not ("ascending" or "descending")) return Invalid(model, "Choose a sort direction.");
        for (int pass = 0; pass < values.Length - 1; pass++)
        {
            for (int i = 0; i < values.Length - 1 - pass; i++)
            {
                if (model.Direction == "ascending" ? values[i] > values[i + 1] : values[i] < values[i + 1])
                {
                    decimal temporary = values[i];
                    values[i] = values[i + 1];
                    values[i + 1] = temporary;
                }
            }
        }
        ViewBag.Result = (model.Direction == "descending" ? "Descending: " : "Ascending: ") + FormatArray(values);
        return View(model);
    }

    private static string FormatArray(decimal[] values) =>
        string.Join(", ", values.Select(value => value.ToString(CultureInfo.InvariantCulture)));

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult MatrixAddition(DELMUNDOMARCKEVIN_241639_ArrayInput model)
    {
        if (!ModelState.IsValid || !TryMatrix(model.Input, out var a) || !TryMatrix(model.SecondInput, out var b) ||
            a.GetLength(0) != b.GetLength(0) || a.GetLength(1) != b.GetLength(1))
            return Invalid(model, "Enter two rectangular matrices of the same size (up to 10 × 10). Separate rows with semicolons or newlines.");
        var result = new decimal[a.GetLength(0), a.GetLength(1)];
        for (int row = 0; row < a.GetLength(0); row++)
            for (int col = 0; col < a.GetLength(1); col++)
                result[row, col] = a[row, col] + b[row, col];
        ViewBag.Result = FormatMatrix(result);
        return View(model);
    }

    private IActionResult Invalid(DELMUNDOMARCKEVIN_241639_ArrayInput model, string message)
    {
        ViewBag.Error = message;
        return View(model);
    }

    private static bool TryNumbers(string? input, out decimal[] values)
    {
        values = [];
        if (string.IsNullOrWhiteSpace(input) || input.Length > 3000) return false;
        var tokens = input.Split(new[] { ',', ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Length is < 1 or > 100) return false;
        values = new decimal[tokens.Length];
        for (int i = 0; i < tokens.Length; i++)
            if (!decimal.TryParse(tokens[i], NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out values[i]) || values[i] < -1000000 || values[i] > 1000000) return false;
        return true;
    }

    private static bool TryMatrix(string? input, out decimal[,] matrix)
    {
        matrix = new decimal[0, 0];
        if (string.IsNullOrWhiteSpace(input) || input.Length > 3000) return false;
        var rows = input.Replace("\r\n", "\n").Trim().Split(new[] { ';', '\n' });
        if (rows.Length is < 1 or > 10) return false;
        for (int row = 0; row < rows.Length; row++)
        {
            if (!TryNumbers(rows[row], out var values) || values.Length > 10) return false;
            if (row == 0) matrix = new decimal[rows.Length, values.Length];
            if (values.Length != matrix.GetLength(1)) return false;
            for (int col = 0; col < values.Length; col++) matrix[row, col] = values[col];
        }
        return true;
    }

    private static string FormatMatrix(decimal[,] matrix)
    {
        var rows = new string[matrix.GetLength(0)];
        for (int row = 0; row < matrix.GetLength(0); row++)
        {
            var cells = new string[matrix.GetLength(1)];
            for (int col = 0; col < matrix.GetLength(1); col++) cells[col] = matrix[row, col].ToString(CultureInfo.InvariantCulture);
            rows[row] = string.Join("  ", cells);
        }
        return string.Join('\n', rows);
    }
}
