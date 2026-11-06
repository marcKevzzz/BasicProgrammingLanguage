using Microsoft.AspNetCore.Http;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Controllers;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;

void Check(bool condition, string message) { if (!condition) throw new Exception(message); }
var catalog = new ProgramCatalog();
Check(catalog.Categories.Count == 5 && catalog.Programs.Count == 25, "Expected five categories and 25 programs");
foreach (var category in catalog.Categories)
    Check((int)category.Count == 5, "Expected five exercises per category");

Check(string.Join(",", catalog.Programs.Where(p => (string)p.CategoryId == "repetition").Select(p => p.Action)) ==
    "SavingsChallenge,ExpenseTracker,CountdownTimer,NumberGuessing,PinVerification", "Loop order");
foreach (string action in new[] { "SumAverage", "BubbleSort" })
{
    var seeded = (Microsoft.AspNetCore.Mvc.ViewResult)typeof(DELMUNDOMARCKEVIN_241639_ArraysController).GetMethod(action, Type.EmptyTypes)!.Invoke(new DELMUNDOMARCKEVIN_241639_ArraysController(), null)!;
    var sample = (DELMUNDOMARCKEVIN_241639_ArrayInput)seeded.Model!;
    Check(!string.IsNullOrWhiteSpace(sample.Input), "Missing array sample");
    if (action == "MatrixAddition") Check(!string.IsNullOrWhiteSpace(sample.SecondInput), "Missing second matrix");
}

foreach (var example in new[] { ("add", 8m), ("subtract", 4m), ("multiply", 12m), ("divide", 3m) })
{
    var calculator = new DELMUNDOMARCKEVIN_241639_InputOutputController();
    calculator.TravelFare(6m, 2m, example.Item1);
    Check(((dynamic)calculator.ViewData.Model!).Success == true && (decimal)((dynamic)calculator.ViewData.Model!).Result == example.Item2, "Calculator " + example.Item1);
}
foreach (var example in new (decimal? First, decimal? Second, string Operation)[] { (1m, 0m, "divide"), (null, 2m, "add"), (1m, 2m, "unknown"), (decimal.MaxValue, 2m, "multiply") })
{
    var calculator = new DELMUNDOMARCKEVIN_241639_InputOutputController();
    calculator.TravelFare(example.First, example.Second, example.Operation);
    Check(((dynamic)calculator.ViewData.Model!).Error != null, "Calculator validation");
}


var gwa = new DELMUNDOMARCKEVIN_241639_ArraysController();
gwa.SumAverage(new() { Input = "90,80" });
Check(((string)((dynamic)gwa.ViewData.Model!).Result).Contains("85.00"), "Each grade must have equal weight");
foreach (var boundary in new[] { (96.50m, "1.00"), (92.50m, "1.25"), (88.50m, "1.50"), (84.50m, "1.75"), (80.50m, "2.00"), (76.50m, "2.25"), (72.50m, "2.50"), (68.50m, "2.75"), (64.50m, "3.00"), (64.49m, "5.00") })
{
    var grade = new DELMUNDOMARCKEVIN_241639_ArraysController();
    grade.SumAverage(new() { Input = boundary.Item1.ToString(System.Globalization.CultureInfo.InvariantCulture) });
    Check(((string)((dynamic)grade.ViewData.Model!).Result).Contains("Numerical grade: " + boundary.Item2), "School conversion boundary");
}
var descendingSort = new DELMUNDOMARCKEVIN_241639_ArraysController();
descendingSort.BubbleSort(new() { Input = "3,1,2", Direction = "descending" });
Check((string)((dynamic)descendingSort.ViewData.Model!).Result == "Descending: 3, 2, 1", "Descending bubble sort");
for (int sample = 0; sample < 10; sample++)
{
    var view = (Microsoft.AspNetCore.Mvc.ViewResult)new DELMUNDOMARCKEVIN_241639_ArraysController().SumAverage();
    var initial = (DELMUNDOMARCKEVIN_241639_ArrayInput)view.Model!;
    Check(initial.Input.Split(',').All(value => int.Parse(value.Trim()) is >= 60 and <= 100), "Initial grade range");
}
Check(string.Join(",", catalog.Programs.Where(p => ((string)p.Title).Contains("(2D)")).Select(p => p.Action)) == "Maze,TheaterSeating,TicTacToe", "2D program order");
void ArrayCheck(string action, string input, string? second, string expected)
{
    var controller = new DELMUNDOMARCKEVIN_241639_ArraysController();
    var model = new DELMUNDOMARCKEVIN_241639_ArrayInput { Input = input, SecondInput = second };
    typeof(DELMUNDOMARCKEVIN_241639_ArraysController).GetMethod(action, new[] { model.GetType() })!.Invoke(controller, new object[] { model });
    Check(((dynamic)controller.ViewData.Model!).Error == null && ((string)((dynamic)controller.ViewData.Model!).Result).Contains(expected), action);
}

ArrayCheck("SumAverage", "90, 80, 100", null, "General weighted average: 90");
ArrayCheck("SumAverage", "90, 80, 100", null, "Numerical grade: 1.50");
ArrayCheck("SumAverage", "60, 60, 60", null, "Numerical grade: 5.00");
var invalidGrade = new DELMUNDOMARCKEVIN_241639_ArraysController();
invalidGrade.SumAverage(new() { Input = "101" });
Check(((dynamic)invalidGrade.ViewData.Model!).Error != null, "Reject grades above 100");

ArrayCheck("BubbleSort", "3,-2,3,0,1.5", null, "-2, 0, 1.5, 3, 3");
ArrayCheck("BubbleSort", "1", null, "1");
foreach (var input in new[] { "", "NaN", "1,x", "1000001", "-79228162514264337593543950335", new string('1', 3001) })
{
    var controller = new DELMUNDOMARCKEVIN_241639_ArraysController();
    controller.SumAverage(new() { Input = input });
    Check(((dynamic)controller.ViewData.Model!).Error != null, "Invalid array accepted");
}
foreach (var alias in new[] { ("LargestSmallest", "TheaterSeating"), ("ReverseArray", "TicTacToe"), ("MatrixAddition", "Maze") })
{
    var controller = new DELMUNDOMARCKEVIN_241639_ArraysController();
    var get = (Microsoft.AspNetCore.Mvc.RedirectToActionResult)controller.GetType().GetMethod(alias.Item1, Type.EmptyTypes)!.Invoke(controller, null)!;
    var post = (Microsoft.AspNetCore.Mvc.RedirectToActionResult)controller.GetType().GetMethod(alias.Item1, new[] { typeof(DELMUNDOMARCKEVIN_241639_ArrayInput) })!.Invoke(controller, new object[] { new DELMUNDOMARCKEVIN_241639_ArrayInput() })!;
    Check(get.ActionName == alias.Item2 && post.ActionName == alias.Item2, "Consistent legacy routes");
}
DELMUNDOMARCKEVIN_241639_RepetitionController NewLoop() => new() {
    ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext { HttpContext = new DefaultHttpContext { Session = new TestSession() } }
};
var loop = NewLoop();
loop.HttpContext.Session.SetInt32("SecretNumber", 42);
loop.NumberGuessing("10,42,99", null);
Check((int)((dynamic)loop.ViewData.Model!).Attempts == 2 && ((dynamic)loop.ViewData.Model!).IsWon == true, "Guessing must stop on a correct guess");
loop = NewLoop();
loop.HttpContext.Session.SetInt32("SecretNumber", 42);
loop.NumberGuessing("10", null);
loop.NumberGuessing("80", null);
Check(((List<string>)((dynamic)loop.ViewData.Model!).History).Count == 2, "History must retain earlier guesses");
loop.NumberGuessing();
Check(((List<string>)((dynamic)loop.ViewData.Model!).History).Count == 2, "History must survive refresh");
loop.NumberGuessing("", "restart");
loop.NumberGuessing();
Check(((List<string>)((dynamic)loop.ViewData.Model!).History).Count == 0, "New game must clear history");
loop = NewLoop();
loop.HttpContext.Session.SetInt32("SecretNumber", 42);
loop.NumberGuessing("10,x", null);
Check(((dynamic)loop.ViewData.Model!).Error != null && loop.HttpContext.Session.GetInt32("GuessAttempts") == null, "Invalid batch must not consume guesses");
loop = NewLoop();
loop.HttpContext.Session.SetInt32("GuessAttempts", 5);
loop.NumberGuessing("x", null);
Check((int)((dynamic)loop.ViewData.Model!).Attempts == 5 && loop.HttpContext.Session.GetInt32("GuessAttempts") == 5, "Invalid input must preserve the displayed guess count");
loop = NewLoop();
loop.HttpContext.Session.SetString("DemoPin", "1234");
loop.PinVerification("0000,1234,9999", null);
Check(((dynamic)loop.ViewData.Model!).Success == true && ((List<string>)((dynamic)loop.ViewData.Model!).History).Count == 2, "PIN must stop when verified");
loop = NewLoop();
loop.HttpContext.Session.SetString("DemoPin", "1234");
loop.PinVerification("0000,1111,2222", null);
Check(((dynamic)loop.ViewData.Model!).IsLocked == true && (int)((dynamic)loop.ViewData.Model!).RemainingAttempts == 0, "Three wrong PINs must lock the demo");
loop.PinVerification("1234", null);
Check(((dynamic)loop.ViewData.Model!).IsLocked == true, "A locked PIN session must remain locked");
loop = NewLoop();
loop.HttpContext.Session.SetString("DemoPin", "1234");
loop.PinVerification("1234,bad", null);
Check(((dynamic)loop.ViewData.Model!).Error != null && (int)((dynamic)loop.ViewData.Model!).RemainingAttempts == 3, "Validate all input before consuming PIN attempts");
foreach (var (weight, expected) in new[] { (74.97, "Normal Weight"), (75.0, "Overweight"), (89.97, "Overweight"), (90.0, "Obese") })
{
    var conditional = new DELMUNDOMARCKEVIN_241639_ConditionalController();
    conditional.BmiChecker(weight, Math.Sqrt(3) * 100);
    Check((string)((dynamic)conditional.ViewData.Model!).Category == expected, "Nested BMI boundary");
}
foreach (var (grade, income, failing, expected) in new[] { (95.0, 10000.0, false, true), (89.0, 10000.0, false, false), (95.0, 30000.0, false, false), (95.0, 10000.0, true, false) })
{
    var conditional = new DELMUNDOMARCKEVIN_241639_ConditionalController();
    conditional.ScholarshipQualification(grade, income, failing);
    Check((bool)((dynamic)conditional.ViewData.Model!).IsQualified == expected, "Nested scholarship branch");
}
Console.WriteLine("Catalog, common arrays, validation, and do-while guessing/PIN checks passed.");

Check(string.Join(",", catalog.Programs.Where(p => (string)p.CategoryId == "conditional").Select(p => p.Action)) ==
    "OutfitRecommendation,ScholarshipQualification,BmiChecker,PasswordStrength,Payroll", "Payroll follows Password Checker");
var slip = PayrollCalculator.Calculate(30000m);
Check(slip.Sss == 1500m && slip.PhilHealth == 750m && slip.PagIbig == 200m, "Payroll employee contributions");
Check(slip.TaxableIncome == 27550m && slip.WithholdingTax == 1007.55m && slip.NetPay == 26542.45m, "Payroll sample payslip");
foreach (var example in new[] { (5249.99m, 250m), (5250m, 275m), (34749.99m, 1725m), (34750m, 1750m), (100000m, 1750m) })
    Check(PayrollCalculator.Calculate(example.Item1).Sss == example.Item2, "SSS schedule boundary");
Check(PayrollCalculator.Calculate(5000m).PhilHealth == 250m && PayrollCalculator.Calculate(150000m).PhilHealth == 2500m, "PhilHealth floor and cap");
Check(PayrollCalculator.Calculate(1500m).PagIbig == 15m && PayrollCalculator.Calculate(1500.01m).PagIbig == 30m && PayrollCalculator.Calculate(10000m).PagIbig == 200m, "Pag-IBIG rates and cap");
foreach (var example in new[] { (20833m, 0m), (20833.01m, 0m), (20834m, .15m), (33332.99m, 1875m), (33333m, 1875m), (66666.99m, 8541.80m), (66667m, 8541.80m), (166666.99m, 33541.80m), (166667m, 33541.80m), (666666.99m, 183541.80m), (666667m, 183541.80m), (1000000m, 300208.35m) })
    Check(PayrollCalculator.MonthlyTax(example.Item1) == example.Item2, "BIR monthly threshold: " + example.Item1);
foreach (string? invalid in new string?[] { null, "", "abc", "0", "-1", "1.001", "1e9", "9999999999999999999999999999999999999999" })
{
    var payroll = new DELMUNDOMARCKEVIN_241639_ConditionalController();
    payroll.Payroll(invalid);
    Check(((DELMUNDOMARCKEVIN_241639_PayrollModel)payroll.ViewData.Model!).Error != null, "Payroll rejects invalid salary: " + invalid);
}
var payrollController = new DELMUNDOMARCKEVIN_241639_ConditionalController();
var payrollView = (Microsoft.AspNetCore.Mvc.ViewResult)payrollController.Payroll("30000.00");
Check(((DELMUNDOMARCKEVIN_241639_PayrollModel)payrollView.Model!).Result!.NetPay == 26542.45m, "Payroll POST returns payslip");
Check(PayrollCalculator.Calculate(decimal.MaxValue).NetPay > 0, "Largest decimal salary cannot overflow");
Console.WriteLine("Payroll checks passed: validation, contribution schedules, all tax brackets, net pay, navigation order.");



var libraryStore = new LibraryStore();
var invalidBook = new DELMUNDOMARCKEVIN_241639_LibraryController(libraryStore);
var invalidInput = new DELMUNDOMARCKEVIN_241639_BookInput { Title = "Valid title", Author = "Valid author", Genre = "Test", Year = 1200 };
var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
Check(!System.ComponentModel.DataAnnotations.Validator.TryValidateObject(invalidInput,
    new System.ComponentModel.DataAnnotations.ValidationContext(invalidInput), validationResults, true), "DELMUNDOMARCKEVIN_241639_Book model rejects invalid years");
foreach (var error in validationResults) invalidBook.ModelState.AddModelError("", error.ErrorMessage!);
var invalidEdit = (Microsoft.AspNetCore.Mvc.ViewResult)invalidBook.Edit(1, invalidInput);
Check(!invalidBook.ModelState.IsValid && libraryStore.GetById(1)!.Year != 1200 &&
    ((DELMUNDOMARCKEVIN_241639_BookEditModel)invalidEdit.Model!).Input == invalidInput, "DELMUNDOMARCKEVIN_241639_Book edit preserves invalid input without changing data");
var bindingBook = new DELMUNDOMARCKEVIN_241639_LibraryController(libraryStore);
bindingBook.ModelState.AddModelError("Year", "Not an integer");
Check(bindingBook.Create(new DELMUNDOMARCKEVIN_241639_BookInput()) is Microsoft.AspNetCore.Mvc.ViewResult && libraryStore.Search(null).Count == 4,
    "DELMUNDOMARCKEVIN_241639_Book create rejects binding errors");
Check(libraryStore.Search("  cLeAn CoDe ").Single().Id == 2 && libraryStore.Search("andrew").Single().Id == 1 &&
    libraryStore.Search("architecture").Single().Id == 3, "DELMUNDOMARCKEVIN_241639_Book search trims and matches title, author, and genre ignoring case");
var bookInput = new DELMUNDOMARCKEVIN_241639_BookInput { Title = " New book ", Author = " Author ", Genre = " ", Year = 2026 };
var insertedBook = libraryStore.Insert(bookInput);
Check(insertedBook.Id == 5 && insertedBook.Title == "New book" && insertedBook.Author == "Author" && insertedBook.Genre == "General",
    "DELMUNDOMARCKEVIN_241639_Book creation normalizes input and assigns ID");
var snapshot = libraryStore.Search(null);
bookInput.Title = "Updated";
Check(libraryStore.Update(5, bookInput) && libraryStore.GetById(5)!.Title == "Updated" && snapshot.Single(b => b.Id == 5).Title == "New book",
    "DELMUNDOMARCKEVIN_241639_Book update replaces records without changing previous snapshots");
Check(!libraryStore.Update(999, bookInput) && !libraryStore.Delete(999) && libraryStore.GetById(999) == null, "Missing book operations");
Check(libraryStore.Delete(5) && libraryStore.GetById(5) == null, "DELMUNDOMARCKEVIN_241639_Book deletion");
try { libraryStore.Insert(invalidInput); throw new Exception("Store accepted an invalid book"); }
catch (ArgumentException) { Check(libraryStore.Search(null).Count == 4, "Invalid book cannot enter storage"); }
Parallel.For(0, 50, _ => libraryStore.Insert(bookInput));
Check(libraryStore.Search(null).Count == 54 && libraryStore.Search(null).Select(b => b.Id).Distinct().Count() == 54,
    "Concurrent book inserts use unique IDs");


void ValidateInput(Microsoft.AspNetCore.Mvc.Controller controller, object input)
{
    var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
    System.ComponentModel.DataAnnotations.Validator.TryValidateObject(input,
        new System.ComponentModel.DataAnnotations.ValidationContext(input), results, true);
    foreach (var result in results) controller.ModelState.AddModelError("", result.ErrorMessage!);
}
var wishStore = new TypedCrudStore<DELMUNDOMARCKEVIN_241639_WishlistItem>(new[] { new DELMUNDOMARCKEVIN_241639_WishlistItem(1, "Item", "Test", 10, "High", false) });
var invalidWish = new DELMUNDOMARCKEVIN_241639_WishlistController(wishStore);
var wishInput = new DELMUNDOMARCKEVIN_241639_WishlistInput { Name = "Item", Price = -1 };
ValidateInput(invalidWish, wishInput);
Check(invalidWish.Edit(1, wishInput) is Microsoft.AspNetCore.Mvc.ViewResult && !invalidWish.ModelState.IsValid && wishStore.GetById(1)!.Price == 10, "Wishlist rejects invalid prices without changing data");
var gameStore = new TypedCrudStore<DELMUNDOMARCKEVIN_241639_Game>(new[] { new DELMUNDOMARCKEVIN_241639_Game(1, "DELMUNDOMARCKEVIN_241639_Game", "RPG", "PC", "Backlog", 5) });
var invalidGame = new DELMUNDOMARCKEVIN_241639_GameCollectionController(gameStore);
var gameInput = new DELMUNDOMARCKEVIN_241639_GameCollectionInput { Title = "DELMUNDOMARCKEVIN_241639_Game", Rating = 6 };
ValidateInput(invalidGame, gameInput);
Check(invalidGame.Edit(1, gameInput) is Microsoft.AspNetCore.Mvc.ViewResult && !invalidGame.ModelState.IsValid && gameStore.GetById(1)!.Rating == 5, "DELMUNDOMARCKEVIN_241639_Game rejects invalid rating");
var attendanceStore = new TypedCrudStore<DELMUNDOMARCKEVIN_241639_AttendanceRecord>(Array.Empty<DELMUNDOMARCKEVIN_241639_AttendanceRecord>());
var invalidAttendance = new DELMUNDOMARCKEVIN_241639_AttendanceController(attendanceStore);
var attendanceInput = new DELMUNDOMARCKEVIN_241639_AttendanceInput { Name = "Student", StudentNumber = "123", Date = "2026-02-30" };
ValidateInput(invalidAttendance, attendanceInput);
Check(invalidAttendance.Create(attendanceInput) is Microsoft.AspNetCore.Mvc.ViewResult && !invalidAttendance.ModelState.IsValid && attendanceStore.GetAll().Count == 0, "Attendance rejects impossible dates");
var reportStore = new TypedCrudStore<DELMUNDOMARCKEVIN_241639_LostFoundItem>(Array.Empty<DELMUNDOMARCKEVIN_241639_LostFoundItem>());
var invalidReport = new DELMUNDOMARCKEVIN_241639_LostFoundController(reportStore);
var reportInput = new DELMUNDOMARCKEVIN_241639_LostFoundInput { Name = "Item", Location = "Office", Status = "Unknown" };
ValidateInput(invalidReport, reportInput);
Check(invalidReport.Create(reportInput) is Microsoft.AspNetCore.Mvc.ViewResult && !invalidReport.ModelState.IsValid, "Lost and found rejects unknown statuses");
Check(CrudValidation.Wishlist("Item", double.NaN, "Medium") != null && CrudValidation.Wishlist("Item", double.PositiveInfinity, "Medium") != null, "Non-finite prices rejected");
var bindingFailure = new DELMUNDOMARCKEVIN_241639_GameCollectionController(gameStore);
bindingFailure.ModelState.AddModelError("Rating", "Not an integer");
Check(bindingFailure.Create(new DELMUNDOMARCKEVIN_241639_GameCollectionInput()) is Microsoft.AspNetCore.Mvc.ViewResult && gameStore.GetAll().Count == 1, "Binding failure cannot insert a game");
var wishSnapshot = wishStore.GetAll();
Parallel.For(0, 100, _ => wishStore.Change(1, item => item with { Purchased = !item.Purchased }));
Check(!wishStore.GetById(1)!.Purchased && !wishSnapshot[0].Purchased, "Purchase toggling is atomic and snapshots are immutable");
Parallel.For(0, 50, _ => wishStore.Insert(new DELMUNDOMARCKEVIN_241639_WishlistItem(0, "New", "General", 0, "Medium", false)));
Check(wishStore.GetAll().Select(item => item.Id).Distinct().Count() == 51, "Concurrent CRUD IDs are unique");
Check(!wishStore.Update(999, wishSnapshot[0]) && !wishStore.Delete(999), "Missing records are not silently created");
var gamePage = (Microsoft.AspNetCore.Mvc.ViewResult)new DELMUNDOMARCKEVIN_241639_GameCollectionController(gameStore).Index(" GAME ", "Backlog");
Check(((DELMUNDOMARCKEVIN_241639_GameCollectionIndexModel)gamePage.Model!).Items.Count == 1, "Typed game search and status filter");
Console.WriteLine("CRUD validation regressions passed: years, prices, ratings, dates, statuses, and binding errors.");


for (int layout = 0; layout < 5; layout++)
{
    char[,] map = GridExercises.MazeMap(layout);
    Check(map[1, 0] == 'S' && map[13, 24] == 'E', "Maze openings");
    var visited = new HashSet<(int, int)> { (1, 0) };
    var queue = new Queue<(int, int)>(); queue.Enqueue((1, 0));
    while (queue.Count > 0)
    {
        var cell = queue.Dequeue();
        foreach (string direction in new[] { "up", "down", "left", "right" })
        {
            int r = cell.Item1, c = cell.Item2;
            if (GridExercises.TryMove(map, ref r, ref c, direction) && visited.Add((r, c))) queue.Enqueue((r, c));
        }
    }
    Check(visited.Contains((13, 24)), "Every fixed C# maze is solvable");
    int row = 1, column = 0;
    Check(!GridExercises.TryMove(map, ref row, ref column, "left") && column == 0, "Maze bounds blocked");
    Check(!GridExercises.TryMove(map, ref row, ref column, "up"), "Maze wall blocked");
}
bool[,] seats = GridExercises.SelectedSeats(new string('0', 40));
decimal[,] seatPrices = GridExercises.SeatPrices();
seats[0, 0] = true; seats[2, 3] = true; seats[4, 7] = true;
Check(GridExercises.SeatTotal(seats, seatPrices) == 650m, "C# 2D seat prices and total");
foreach (int[] line in new[] { new[] {0,1,2}, new[] {3,4,5}, new[] {6,7,8}, new[] {0,3,6}, new[] {1,4,7}, new[] {2,5,8}, new[] {0,4,8}, new[] {2,4,6} })
{
    char[,] board = GridExercises.GameBoard(".........");
    foreach (int index in line) board[index / 3, index % 3] = 'X';
    Check(GridExercises.WinningCells(board).SequenceEqual(line), "All eight C# winning lines");
    Check(GridExercises.SaveBoard(board).Length == 9, "C# board serialization");
}
Check(GridExercises.WinningCells(GridExercises.GameBoard("XOXXOOOXX")).Length == 0, "Draw has no winning line");
var gridController = new DELMUNDOMARCKEVIN_241639_ArraysController {
    ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext { HttpContext = new DefaultHttpContext { Session = new TestSession() } }
};
gridController.Maze("move", null, "right");
Check((int)((dynamic)gridController.ViewData.Model!).Column == 1 && (int)((dynamic)gridController.ViewData.Model!).Moves == 1, "Maze moves processed by C#");
gridController.Maze("restart", null, null);
Check((int)((dynamic)gridController.ViewData.Model!).Column == 0 && (int)((dynamic)gridController.ViewData.Model!).Moves == 0, "Maze restart");
gridController.TheaterSeating("toggle", 2, 3);
Check((decimal)((dynamic)gridController.ViewData.Model!).Total == 300m, "Seat toggle processed by C#");
gridController.TheaterSeating("toggle", 2, 3);
Check((decimal)((dynamic)gridController.ViewData.Model!).Total == 0m, "Seat release processed by C#");
gridController.TheaterSeating("toggle", 99, -1);
Check((decimal)((dynamic)gridController.ViewData.Model!).Total == 0m, "Invalid seat cannot alter state");
foreach (var cell in new[] { (0,0), (1,0), (0,1), (1,1), (0,2) }) gridController.TicTacToe("move", cell.Item1, cell.Item2);
Check((bool)((dynamic)gridController.ViewData.Model!).Finished && ((int[])((dynamic)gridController.ViewData.Model!).Winning).SequenceEqual(new[] {0,1,2}), "DELMUNDOMARCKEVIN_241639_Game win processed by C#");
gridController.TicTacToe("move", 2, 2);
Check(((char[,])((dynamic)gridController.ViewData.Model!).Board)[2,2] == '.', "No moves after win");
gridController.TicTacToe("reset", null, null);
Check(!(bool)((dynamic)gridController.ViewData.Model!).Finished && (string)((dynamic)gridController.ViewData.Model!).Turn == "X", "DELMUNDOMARCKEVIN_241639_Game reset");
Console.WriteLine("C# grid checks passed: all fixed maze paths, movement, seating, eight wins, draws, session state, validation, and resets.");

var feedbackLoop = NewLoop();
feedbackLoop.HttpContext.Session.SetInt32("SecretNumber", 35);
feedbackLoop.NumberGuessing("50,20", null);
Check((string)((dynamic)feedbackLoop.ViewData.Model!).Message == "20 is too low. Try a higher number." && (string)((dynamic)feedbackLoop.ViewData.Model!).MessageType == "higher", "Latest guess gives the correct directional feedback");
feedbackLoop.NumberGuessing("40", null);
Check((string)((dynamic)feedbackLoop.ViewData.Model!).Message == "40 is too high. Try a lower number." && (string)((dynamic)feedbackLoop.ViewData.Model!).MessageType == "lower", "High guess points down");

var expenseLoop = NewLoop();
expenseLoop.HttpContext.Session.SetString("DailyExpenses", "[{\"name\":\"Legacy expense\",\"amount\":125.5,\"date\":\"Nov 6\"}]");
expenseLoop.ExpenseTracker();
var expenseModel = (DELMUNDOMARCKEVIN_241639_ExpenseTrackerModel)expenseLoop.ViewData.Model!;
Check(expenseModel.TotalExpenses == 125.5 && expenseModel.Expenses.Single().Name == "Legacy expense", "Typed expenses preserve existing session JSON");
expenseLoop.CountdownTimer();
var countdownModel = (DELMUNDOMARCKEVIN_241639_CountdownTimerModel)expenseLoop.ViewData.Model!;
Check(countdownModel.Sequence.Count == 11 && countdownModel.Sequence.Last().Second == 0, "Typed countdown ticks");
Check(new DELMUNDOMARCKEVIN_241639_WishlistInput { Name = "Item" }.Validate(new System.ComponentModel.DataAnnotations.ValidationContext(new object())).Any(), "Missing wishlist price is rejected");

class TestSession : ISession {
    readonly Dictionary<string, byte[]> values = new();
    public bool IsAvailable => true;
    public string Id => "test";
    public IEnumerable<string> Keys => values.Keys;
    public void Clear() => values.Clear();
    public void Remove(string key) => values.Remove(key);
    public void Set(string key, byte[] value) => values[key] = value;
    public bool TryGetValue(string key, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out byte[]? value) => values.TryGetValue(key, out value);
    public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
}
