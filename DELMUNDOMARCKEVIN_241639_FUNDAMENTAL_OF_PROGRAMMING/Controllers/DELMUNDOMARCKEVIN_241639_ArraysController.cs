using Microsoft.AspNetCore.Mvc;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;

namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Controllers;

public class DELMUNDOMARCKEVIN_241639_ArraysController : Controller
{
    [HttpGet]
    public IActionResult Maze() => MazeView();

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Maze(string? command, int? layout, string? direction)
    {
        if (!ModelState.IsValid || command is not ("move" or "restart" or "layout" or "new"))
            return MazeView("Choose a valid maze action.");
        if (command == "layout" && layout is not (>= 0 and < 5)) return MazeView("Choose a maze from 1 to 5.");
        int current = HttpContext.Session.GetInt32("MazeLayout") ?? 0;
        if (command != "move")
        {
            if (command == "layout") current = layout!.Value;
            else if (command == "new") current = (current + 1) % 5;
            HttpContext.Session.SetInt32("MazeLayout", current);
            HttpContext.Session.SetInt32("MazeRow", 1);
            HttpContext.Session.SetInt32("MazeColumn", 0);
            HttpContext.Session.SetInt32("MazeMoves", 0);
            return MazeView();
        }
        char[,] map = GridExercises.MazeMap(current);
        int row = HttpContext.Session.GetInt32("MazeRow") ?? 1;
        int column = HttpContext.Session.GetInt32("MazeColumn") ?? 0;
        if (map[row, column] == 'E') return MazeView("Exit reached. Restart or choose another maze.");
        if (!GridExercises.TryMove(map, ref row, ref column, direction ?? "")) return MazeView("Blocked. Try another direction.");
        HttpContext.Session.SetInt32("MazeRow", row);
        HttpContext.Session.SetInt32("MazeColumn", column);
        HttpContext.Session.SetInt32("MazeMoves", (HttpContext.Session.GetInt32("MazeMoves") ?? 0) + 1);
        return MazeView();
    }

    private IActionResult MazeView(string? message = null)
    {
        int layout = HttpContext.Session.GetInt32("MazeLayout") ?? 0;
        return View("Maze", new DELMUNDOMARCKEVIN_241639_MazeModel(GridExercises.MazeMap(layout), layout, HttpContext.Session.GetInt32("MazeRow") ?? 1, HttpContext.Session.GetInt32("MazeColumn") ?? 0, HttpContext.Session.GetInt32("MazeMoves") ?? 0, message));
    }

    [HttpGet]
    public IActionResult TheaterSeating() => SeatingView();

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult TheaterSeating(string? command, int? row, int? column)
    {
        if (!ModelState.IsValid || command is not ("toggle" or "clear")) return SeatingView("Choose a valid seat action.");
        bool[,] selected = GridExercises.SelectedSeats(HttpContext.Session.GetString("SelectedSeats") ?? new string('0', 40));
        if (command == "clear") selected = new bool[5, 8];
        else
        {
            if (row is not (>= 0 and < 5) || column is not (>= 0 and < 8)) return SeatingView("Choose a seat within the chart.");
            selected[row.Value, column.Value] = !selected[row.Value, column.Value];
        }
        string state = "";
        for (int r = 0; r < 5; r++)
            for (int c = 0; c < 8; c++) state += selected[r, c] ? '1' : '0';
        HttpContext.Session.SetString("SelectedSeats", state);
        string message = command == "clear" ? "Selection cleared." : $"Seat {(char)('A' + row!.Value)}{column!.Value + 1} {(selected[row.Value, column.Value] ? "selected" : "released")}.";
        return SeatingView(message);
    }

    private IActionResult SeatingView(string? message = null)
    {
        bool[,] selected = GridExercises.SelectedSeats(HttpContext.Session.GetString("SelectedSeats") ?? new string('0', 40));
        decimal[,] prices = GridExercises.SeatPrices();
        return View("TheaterSeating", new DELMUNDOMARCKEVIN_241639_TheaterSeatingModel(selected, prices, GridExercises.SeatTotal(selected, prices), message));
    }

    [HttpGet]
    public IActionResult TicTacToe() => GameView();

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult TicTacToe(string? command, int? row, int? column)
    {
        if (!ModelState.IsValid || command is not ("move" or "reset")) return GameView("Choose a valid game action.");
        if (command == "reset")
        {
            HttpContext.Session.SetString("TicBoard", ".........");
            HttpContext.Session.SetString("TicTurn", "X");
            return GameView();
        }
        char[,] board = GridExercises.GameBoard(HttpContext.Session.GetString("TicBoard") ?? ".........");
        if (row is not (>= 0 and < 3) || column is not (>= 0 and < 3)) return GameView("Choose a cell within the board.");
        if (GridExercises.WinningCells(board).Length > 0 || board[row.Value, column.Value] != '.') return GameView("Choose an empty cell or start a new game.");
        char turn = (HttpContext.Session.GetString("TicTurn") ?? "X")[0];
        board[row.Value, column.Value] = turn;
        HttpContext.Session.SetString("TicBoard", GridExercises.SaveBoard(board));
        HttpContext.Session.SetString("TicTurn", turn == 'X' ? "O" : "X");
        return GameView();
    }

    private IActionResult GameView(string? message = null)
    {
        char[,] board = GridExercises.GameBoard(HttpContext.Session.GetString("TicBoard") ?? ".........");
        int[] winning = GridExercises.WinningCells(board);
        return View("TicTacToe", new DELMUNDOMARCKEVIN_241639_TicTacToeModel(board, winning, HttpContext.Session.GetString("TicTurn") ?? "X", winning.Length > 0 || !GridExercises.SaveBoard(board).Contains('.'), message));
    }
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
        model.GradePassed = result.Passed;
        model.Result = result.Result;
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult BubbleSort(DELMUNDOMARCKEVIN_241639_ArrayInput model)
    {
        if (!ModelState.IsValid || !ArrayCalculations.TryNumbers(model.Input, out var values))
            return Invalid(model, "Enter 1–100 numbers between -1000000 and 1000000, separated by spaces or commas.");
        if (model.Direction is not ("ascending" or "descending")) return Invalid(model, "Choose a sort direction.");
        model.Result = ArrayCalculations.BubbleSort(values, model.Direction);
        return View(model);
    }

    private IActionResult Invalid(DELMUNDOMARCKEVIN_241639_ArrayInput model, string message)
    {
        model.Input ??= string.Empty;
        model.Error = message;
        return View(model);
    }
}
