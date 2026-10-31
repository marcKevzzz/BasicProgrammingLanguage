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
ArrayCheck("LargestSmallest", "1,2;3,4", null, "1  3\n2  4");

ArrayCheck("SumAverage", "90, 80, 100", null, "General weighted average: 90");
ArrayCheck("SumAverage", "90, 80, 100", null, "Numerical grade: 1.50");
ArrayCheck("SumAverage", "60, 60, 60", null, "Numerical grade: 5.00");
var invalidGrade = new DELMUNDOMARCKEVIN_241639_ArraysController();
invalidGrade.SumAverage(new() { Input = "101" });
Check(invalidGrade.ViewBag.Error != null, "Reject grades above 100");
ArrayCheck("ReverseArray", "1,2;3,4", null, "3\n7");

ArrayCheck("BubbleSort", "3,-2,3,0,1.5", null, "-2, 0, 1.5, 3, 3");
ArrayCheck("BubbleSort", "1", null, "1");
ArrayCheck("MatrixAddition", "1,2;3,4", "5,6;7,8", "6  8\n10  12");
foreach (var input in new[] { "", "NaN", "1,x", "1000001", "-79228162514264337593543950335", new string('1', 3001) })
{
    var controller = new DELMUNDOMARCKEVIN_241639_ArraysController();
    controller.SumAverage(new() { Input = input });
    Check(controller.ViewBag.Error != null, "Invalid array accepted");
}
foreach (var input in new[] { "1,2;3", ";", "1,2;", string.Join(';', Enumerable.Repeat("1", 11)) })
{
    var controller = new DELMUNDOMARCKEVIN_241639_ArraysController();
    controller.MatrixAddition(new() { Input = input, SecondInput = "1,2;3,4" });
    Check(controller.ViewBag.Error != null, "Invalid matrix accepted");
}
var mismatch = new DELMUNDOMARCKEVIN_241639_ArraysController();
mismatch.MatrixAddition(new() { Input = "1,2", SecondInput = "1;2" });
Check(mismatch.ViewBag.Error != null, "Mismatched matrices accepted");
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
