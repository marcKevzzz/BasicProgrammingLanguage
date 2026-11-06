using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Controllers;

var today = DateOnly.FromDateTime(DateTime.Today);
void Check(DateOnly birthDate, int expected)
{
    var controller = new DELMUNDOMARCKEVIN_241639_InputOutputController();
    controller.BirthdayCountdown(birthDate);
    if (((dynamic)controller.ViewData.Model!).Success != true || (int)((dynamic)controller.ViewData.Model!).DaysRemaining != expected ||
        (int)((dynamic)controller.ViewData.Model!).Weeks != expected / 7 || (int)((dynamic)controller.ViewData.Model!).RemainingDays != expected % 7)
        throw new Exception($"Incorrect countdown for {birthDate}");
}
Check(today.AddYears(-20), 0);
var tomorrow = today.AddDays(1);
Check(tomorrow.AddYears(-20), 1);
var yesterday = today.AddDays(-1);
var next = yesterday.AddYears(1);
Check(yesterday.AddYears(-20), next.DayNumber - today.DayNumber);
var leap = new DateOnly(2000, 2, 29);
var nextLeapBirthday = leap.AddYears(today.Year - 2000);
if (nextLeapBirthday < today) nextLeapBirthday = leap.AddYears(today.Year + 1 - 2000);
Check(leap, nextLeapBirthday.DayNumber - today.DayNumber);
foreach (DateOnly? invalid in new DateOnly?[] { null, today.AddDays(1) })
{
    var controller = new DELMUNDOMARCKEVIN_241639_InputOutputController();
    controller.BirthdayCountdown(invalid);
    if (((dynamic)controller.ViewData.Model!).Error == null || ((dynamic)controller.ViewData.Model!).Success == true)
        throw new Exception("Invalid birth date accepted");
}
var malformed = new DELMUNDOMARCKEVIN_241639_InputOutputController();
malformed.ModelState.AddModelError("birthday", "Invalid date");
malformed.BirthdayCountdown(today);
if (((dynamic)malformed.ViewData.Model!).Error == null) throw new Exception("Malformed date accepted");
Console.WriteLine("Birthday countdown passed: today, tomorrow, past birthday, leap birthday, missing/future/malformed input.");

var study = new DELMUNDOMARCKEVIN_241639_InputOutputController();
study.StudyTime(new[] { "My own subject", "Research" }, new[] { 1.23, 30.0 });
if (((dynamic)study.ViewData.Model!).Success != true || Math.Abs((double)((dynamic)study.ViewData.Model!).TotalHours - 31.23) > .00001)
    throw new Exception("Custom study hours failed");
foreach (var invalid in new[] { -1.0, double.NaN, double.PositiveInfinity }) {
    study = new DELMUNDOMARCKEVIN_241639_InputOutputController();
    study.StudyTime(new[] { "Test" }, new[] { invalid });
    if (((dynamic)study.ViewData.Model!).Error == null) throw new Exception("Invalid study hours accepted");
}
var session = new TestSession();
var pinController = new DELMUNDOMARCKEVIN_241639_RepetitionController {
    ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext {
        HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext { Session = session }
    }
};
pinController.PinVerification();
string firstPin = ((dynamic)pinController.ViewData.Model!).DemoPin;
if (firstPin.Length != 4 || !firstPin.All(char.IsDigit)) throw new Exception("Invalid generated PIN");
pinController.PinVerification(firstPin, null);
if (((dynamic)pinController.ViewData.Model!).Success != true) throw new Exception("Generated PIN not accepted");
pinController.PinVerification();
if ((string)((dynamic)pinController.ViewData.Model!).DemoPin == firstPin) throw new Exception("PIN did not change on refresh");
Console.WriteLine("Study hours and refreshed session PIN checks passed.");

class TestSession : Microsoft.AspNetCore.Http.ISession {
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
