using Microsoft.AspNetCore.Http;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Controllers;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;

void Check(bool condition, string message) { if (!condition) throw new Exception(message); }
var catalog = new ProgramCatalog();
Check(catalog.Categories.Count == 5 && catalog.Programs.Count == 25, "Expected five categories and 25 programs");
foreach (var category in catalog.Categories)
    Check((int)category["count"] == 5, "Expected five exercises per category");

Check(string.Join(",", catalog.Programs.Where(p => (string)p["categoryId"] == "repetition").Select(p => p["action"])) ==
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
    Check(calculator.ViewBag.Success == true && (decimal)calculator.ViewBag.Result == example.Item2, "Calculator " + example.Item1);
}
foreach (var example in new (decimal? First, decimal? Second, string Operation)[] { (1m, 0m, "divide"), (null, 2m, "add"), (1m, 2m, "unknown"), (decimal.MaxValue, 2m, "multiply") })
{
    var calculator = new DELMUNDOMARCKEVIN_241639_InputOutputController();
    calculator.TravelFare(example.First, example.Second, example.Operation);
    Check(calculator.ViewBag.Error != null, "Calculator validation");
}


var gwa = new DELMUNDOMARCKEVIN_241639_ArraysController();
gwa.SumAverage(new() { Input = "90,80" });
Check(((string)gwa.ViewBag.Result).Contains("85.00"), "Each grade must have equal weight");
foreach (var boundary in new[] { (96.50m, "1.00"), (92.50m, "1.25"), (88.50m, "1.50"), (84.50m, "1.75"), (80.50m, "2.00"), (76.50m, "2.25"), (72.50m, "2.50"), (68.50m, "2.75"), (64.50m, "3.00"), (64.49m, "5.00") })
{
    var grade = new DELMUNDOMARCKEVIN_241639_ArraysController();
    grade.SumAverage(new() { Input = boundary.Item1.ToString(System.Globalization.CultureInfo.InvariantCulture) });
    Check(((string)grade.ViewBag.Result).Contains("Numerical grade: " + boundary.Item2), "School conversion boundary");
}
var descendingSort = new DELMUNDOMARCKEVIN_241639_ArraysController();
descendingSort.BubbleSort(new() { Input = "3,1,2", Direction = "descending" });
Check((string)descendingSort.ViewBag.Result == "Descending: 3, 2, 1", "Descending bubble sort");
for (int sample = 0; sample < 10; sample++)
{
    var view = (Microsoft.AspNetCore.Mvc.ViewResult)new DELMUNDOMARCKEVIN_241639_ArraysController().SumAverage();
    var initial = (DELMUNDOMARCKEVIN_241639_ArrayInput)view.Model!;
    Check(initial.Input.Split(',').All(value => int.Parse(value.Trim()) is >= 60 and <= 100), "Initial grade range");
}
Check(string.Join(",", catalog.Programs.Where(p => ((string)p["title"]).Contains("(2D)")).Select(p => p["action"])) == "Maze,TheaterSeating,TicTacToe", "2D program order");
void ArrayCheck(string action, string input, string? second, string expected)
{
    var controller = new DELMUNDOMARCKEVIN_241639_ArraysController();
    var model = new DELMUNDOMARCKEVIN_241639_ArrayInput { Input = input, SecondInput = second };
    typeof(DELMUNDOMARCKEVIN_241639_ArraysController).GetMethod(action, new[] { model.GetType() })!.Invoke(controller, new object[] { model });
    Check(controller.ViewBag.Error == null && ((string)controller.ViewBag.Result).Contains(expected), action);
}

ArrayCheck("SumAverage", "90, 80, 100", null, "General weighted average: 90");
ArrayCheck("SumAverage", "90, 80, 100", null, "Numerical grade: 1.50");
ArrayCheck("SumAverage", "60, 60, 60", null, "Numerical grade: 5.00");
var invalidGrade = new DELMUNDOMARCKEVIN_241639_ArraysController();
invalidGrade.SumAverage(new() { Input = "101" });
Check(invalidGrade.ViewBag.Error != null, "Reject grades above 100");

ArrayCheck("BubbleSort", "3,-2,3,0,1.5", null, "-2, 0, 1.5, 3, 3");
ArrayCheck("BubbleSort", "1", null, "1");
foreach (var input in new[] { "", "NaN", "1,x", "1000001", "-79228162514264337593543950335", new string('1', 3001) })
{
    var controller = new DELMUNDOMARCKEVIN_241639_ArraysController();
    controller.SumAverage(new() { Input = input });
    Check(controller.ViewBag.Error != null, "Invalid array accepted");
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
Check((int)loop.ViewBag.Attempts == 2 && loop.ViewBag.IsWon == true, "Guessing must stop on a correct guess");
loop = NewLoop();
loop.HttpContext.Session.SetInt32("SecretNumber", 42);
loop.NumberGuessing("10", null);
loop.NumberGuessing("80", null);
Check(((List<string>)loop.ViewBag.History).Count == 2, "History must retain earlier guesses");
loop.NumberGuessing();
Check(((List<string>)loop.ViewBag.History).Count == 2, "History must survive refresh");
loop.NumberGuessing("", "restart");
loop.NumberGuessing();
Check(((List<string>)loop.ViewBag.History).Count == 0, "New game must clear history");
loop = NewLoop();
loop.HttpContext.Session.SetInt32("SecretNumber", 42);
loop.NumberGuessing("10,x", null);
Check(loop.ViewBag.Error != null && loop.HttpContext.Session.GetInt32("GuessAttempts") == null, "Invalid batch must not consume guesses");
loop = NewLoop();
loop.HttpContext.Session.SetInt32("GuessAttempts", 5);
loop.NumberGuessing("x", null);
Check((int)loop.ViewBag.Attempts == 5 && loop.HttpContext.Session.GetInt32("GuessAttempts") == 5, "Invalid input must preserve the displayed guess count");
loop = NewLoop();
loop.HttpContext.Session.SetString("DemoPin", "1234");
loop.PinVerification("0000,1234,9999", null);
Check(loop.ViewBag.Success == true && ((List<string>)loop.ViewBag.History).Count == 2, "PIN must stop when verified");
loop = NewLoop();
loop.HttpContext.Session.SetString("DemoPin", "1234");
loop.PinVerification("0000,1111,2222", null);
Check(loop.ViewBag.IsLocked == true && (int)loop.ViewBag.RemainingAttempts == 0, "Three wrong PINs must lock the demo");
loop.PinVerification("1234", null);
Check(loop.ViewBag.IsLocked == true, "A locked PIN session must remain locked");
loop = NewLoop();
loop.HttpContext.Session.SetString("DemoPin", "1234");
loop.PinVerification("1234,bad", null);
Check(loop.ViewBag.Error != null && (int)loop.ViewBag.RemainingAttempts == 3, "Validate all input before consuming PIN attempts");
foreach (var (weight, expected) in new[] { (74.97, "Normal Weight"), (75.0, "Overweight"), (89.97, "Overweight"), (90.0, "Obese") })
{
    var conditional = new DELMUNDOMARCKEVIN_241639_ConditionalController();
    conditional.BmiChecker(weight, Math.Sqrt(3) * 100);
    Check((string)conditional.ViewBag.Category == expected, "Nested BMI boundary");
}
foreach (var (grade, income, failing, expected) in new[] { (95.0, 10000.0, false, true), (89.0, 10000.0, false, false), (95.0, 30000.0, false, false), (95.0, 10000.0, true, false) })
{
    var conditional = new DELMUNDOMARCKEVIN_241639_ConditionalController();
    conditional.ScholarshipQualification(grade, income, failing);
    Check((bool)conditional.ViewBag.IsQualified == expected, "Nested scholarship branch");
}
Console.WriteLine("Catalog, common arrays, validation, and do-while guessing/PIN checks passed.");

Check(string.Join(",", catalog.Programs.Where(p => (string)p["categoryId"] == "conditional").Select(p => p["action"])) ==
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
    Check(payroll.ViewBag.Error != null, "Payroll rejects invalid salary: " + invalid);
}
var payrollController = new DELMUNDOMARCKEVIN_241639_ConditionalController();
var payrollView = (Microsoft.AspNetCore.Mvc.ViewResult)payrollController.Payroll("30000.00");
Check(((PayrollBreakdown)payrollView.Model!).NetPay == 26542.45m, "Payroll POST returns payslip");
Check(PayrollCalculator.Calculate(decimal.MaxValue).NetPay > 0, "Largest decimal salary cannot overflow");
Console.WriteLine("Payroll checks passed: validation, contribution schedules, all tax brackets, net pay, navigation order.");


var crudStore = new InMemoryCrudStore();
var invalidBook = new DELMUNDOMARCKEVIN_241639_LibraryController(crudStore);
invalidBook.Edit(1, "Valid title", "Valid author", "Test", 1200);
Check(invalidBook.ViewBag.Error != null && Convert.ToInt32(crudStore.GetById("library", 1)!["year"]) != 1200, "Book edit rejects invalid years without changing data");
var invalidWish = new DELMUNDOMARCKEVIN_241639_WishlistController(crudStore);
invalidWish.Edit(1, "Valid item", "Test", -1, "High", false);
Check(invalidWish.ViewBag.Error != null && Convert.ToDouble(crudStore.GetById("wishlist", 1)!["price"]) >= 0, "Wishlist edit rejects negative prices");
var invalidGame = new DELMUNDOMARCKEVIN_241639_GameCollectionController(crudStore);
invalidGame.Edit(1, "Valid title", "Test", "PC", "Backlog", 6);
Check(invalidGame.ViewBag.Error != null, "Game edit rejects invalid rating instead of silently using five");
var invalidAttendance = new DELMUNDOMARCKEVIN_241639_AttendanceController(crudStore);
invalidAttendance.Edit(1, "Student", "123", "CS", "A", "2026-02-30", "Present");
Check(invalidAttendance.ViewBag.Error != null, "Attendance rejects impossible dates");
var invalidReport = new DELMUNDOMARCKEVIN_241639_LostFoundController(crudStore);
invalidReport.Edit(1, "Item", "Description", "Office", "2026-11-01", "Unknown");
Check(invalidReport.ViewBag.Error != null, "Lost and found rejects unknown statuses");
Check(CrudValidation.Wishlist("Item", double.NaN, "Medium") != null && CrudValidation.Wishlist("Item", double.PositiveInfinity, "Medium") != null, "Non-finite prices rejected");
Check(CrudValidation.Book("Book", "Author", 2026) == null && CrudValidation.Attendance("Student", "123", "2024-02-29", "Present") == null, "Valid CRUD inputs accepted");
var bindingFailure = new DELMUNDOMARCKEVIN_241639_GameCollectionController(crudStore);
bindingFailure.ModelState.AddModelError("rating", "Not an integer");
bindingFailure.Create("Game", "Test", "PC", "Backlog", 5);
Check(bindingFailure.ViewBag.Error != null, "CRUD rejects model-binding errors without adding models");
Console.WriteLine("CRUD validation regressions passed: years, prices, ratings, dates, statuses, and binding errors.");

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
