using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Controllers
{
    public class DELMUNDOMARCKEVIN_241639_RepetitionController : Controller
    {
        [HttpGet]
        public IActionResult NumberGuessing()
        {
            EnsureSecretNumber();
            ViewBag.History = GetGuessHistory();
            ViewBag.Attempts = HttpContext.Session.GetInt32("GuessAttempts") ?? 0;
            ViewBag.IsWon = HttpContext.Session.GetInt32("GuessWon") == 1;
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult NumberGuessing(string? guesses, string? action)
        {
            if (action == "restart")
            {
                HttpContext.Session.Remove("SecretNumber");
                HttpContext.Session.Remove("GuessAttempts");
                HttpContext.Session.Remove("GuessHistory");
                HttpContext.Session.Remove("GuessWon");
                EnsureSecretNumber();
                ViewBag.Attempts = 0;
                ViewBag.Message = "New game started.";
                return View();
            }
            ViewBag.History = GetGuessHistory();
            ViewBag.Attempts = HttpContext.Session.GetInt32("GuessAttempts") ?? 0;
            var tokens = (guesses ?? "").Split(',', StringSplitOptions.TrimEntries);
            var numbers = new int[tokens.Length];
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(guesses) || guesses.Length > 200 || tokens.Length > 20)
            {
                ViewBag.Error = "Enter 1–20 guesses from 1 to 100, separated by commas.";
                return View();
            }
            for (int i = 0; i < tokens.Length; i++)
            {
                if (!int.TryParse(tokens[i], out numbers[i]) || numbers[i] < 1 || numbers[i] > 100)
                {
                    ViewBag.Error = "Every guess must be a whole number from 1 to 100.";
                    return View();
                }
            }
            EnsureSecretNumber();
            int secret = HttpContext.Session.GetInt32("SecretNumber")!.Value;
            int attempts = HttpContext.Session.GetInt32("GuessAttempts") ?? 0;
            if (HttpContext.Session.GetInt32("GuessWon") == 1)
            {
                ViewBag.IsWon = true;
                ViewBag.Attempts = attempts;
                ViewBag.Message = "You already found the number. Start a new game to play again.";
                return View();
            }
            var history = GetGuessHistory();
            int index = 0;
            bool correct;
            do
            {
                int guess = numbers[index];
                attempts++;
                correct = guess == secret;
                string feedback = correct ? "Correct!" : guess < secret ? "Too low" : "Too high";
                history.Add($"Attempt {attempts}: {guess} — {feedback}");
                index++;
            } while (index < numbers.Length && !correct);

            HttpContext.Session.SetString("GuessHistory", JsonSerializer.Serialize(history));
            HttpContext.Session.SetInt32("GuessAttempts", attempts);
            HttpContext.Session.SetInt32("GuessWon", correct ? 1 : 0);
            ViewBag.Attempts = attempts;
            ViewBag.IsWon = correct;
            ViewBag.History = history;
            ViewBag.Message = correct ? "You found the number!" : "Try again using the hints below.";
            return View();
        }

        private List<string> GetGuessHistory() =>
            JsonSerializer.Deserialize<List<string>>(HttpContext.Session.GetString("GuessHistory") ?? "[]") ?? new();

        private void EnsureSecretNumber()
        {
            if (!HttpContext.Session.GetInt32("SecretNumber").HasValue)
                HttpContext.Session.SetInt32("SecretNumber", System.Security.Cryptography.RandomNumberGenerator.GetInt32(1, 101));
        }

        // 2. Daily Expense Tracker
        [HttpGet]
        public IActionResult ExpenseTracker()
        {
            var expenses = GetSessionExpenses();
            CalculateExpenseMetrics(expenses);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExpenseTracker(string? name, double? amount, string? action)
        {
            var expenses = GetSessionExpenses();

            if (action == "clear")
            {
                expenses.Clear();
                SaveSessionExpenses(expenses);
                ViewBag.Message = "All recorded expenses have been cleared.";
                CalculateExpenseMetrics(expenses);
                return View();
            }

            if (action == "add")
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    ViewBag.Error = "Please specify an expense description or category.";
                    CalculateExpenseMetrics(expenses);
                    return View();
                }

                if (!amount.HasValue || amount.Value <= 0)
                {
                    ViewBag.Error = "Expense amount must be greater than ₱0.00.";
                    CalculateExpenseMetrics(expenses);
                    return View();
                }

                expenses.Add(new Dictionary<string, object>
                {
                    ["name"] = name.Trim(),
                    ["amount"] = amount.Value,
                    ["date"] = DateTime.Now.ToString("MMM dd, yyyy HH:mm")
                });

                SaveSessionExpenses(expenses);
                ViewBag.Message = $"Added '{name}' for ₱{amount.Value:N2}.";
            }

            CalculateExpenseMetrics(expenses);
            return View();
        }

        private List<Dictionary<string, object>> GetSessionExpenses()
        {
            var json = HttpContext.Session.GetString("DailyExpenses");
            if (string.IsNullOrEmpty(json))
            {
                // Seed 3 starter daily expenses
                var initial = new List<Dictionary<string, object>>
                {
                    new() { ["name"] = "Breakfast Combo & Coffee", ["amount"] = 165.00, ["date"] = DateTime.Now.ToString("MMM dd, yyyy 08:30") },
                    new() { ["name"] = "Train & Jeepney Commute", ["amount"] = 85.00, ["date"] = DateTime.Now.ToString("MMM dd, yyyy 12:15") },
                    new() { ["name"] = "Notebook & Study Pens", ["amount"] = 210.00, ["date"] = DateTime.Now.ToString("MMM dd, yyyy 15:40") }
                };
                SaveSessionExpenses(initial);
                return initial;
            }

            try
            {
                var doc = JsonDocument.Parse(json);
                var list = new List<Dictionary<string, object>>();
                foreach (var el in doc.RootElement.EnumerateArray())
                {
                    list.Add(new Dictionary<string, object>
                    {
                        ["name"] = el.GetProperty("name").GetString() ?? "",
                        ["amount"] = el.GetProperty("amount").GetDouble(),
                        ["date"] = el.GetProperty("date").GetString() ?? ""
                    });
                }
                return list;
            }
            catch
            {
                return new List<Dictionary<string, object>>();
            }
        }

        private void SaveSessionExpenses(List<Dictionary<string, object>> expenses)
        {
            HttpContext.Session.SetString("DailyExpenses", JsonSerializer.Serialize(expenses));
        }

        private void CalculateExpenseMetrics(List<Dictionary<string, object>> expenses)
        {
            // Explicit Repetition Statement Requirement: Use C# loop to process collection
            double total = 0;
            int count = 0;

            while (count < expenses.Count)
            {
                total += Convert.ToDouble(expenses[count]["amount"]);
                count++;
            }

            double average = count > 0 ? total / count : 0.0;

            ViewBag.Expenses = expenses;
            ViewBag.TotalExpenses = total;
            ViewBag.AverageExpense = average;
            ViewBag.ExpenseCount = count;
        }

        [HttpGet]
        public IActionResult SavingsChallenge() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult SavingsChallenge(int? size, string? pattern)
        {
            ViewBag.Size = size; ViewBag.Pattern = pattern;
            if (!ModelState.IsValid || size is null or < 1 or > 20 || pattern is not ("left" or "inverted" or "right" or "pyramid" or "diamond"))
            { ViewBag.Error = "Choose a pattern and a base from 1 to 20."; return View(); }
            var lines = new List<string>();
            for (int row = 1; row <= size; row++)
            {
                int stars = pattern == "inverted" ? size.Value - row + 1 : row;
                lines.Add(pattern is "pyramid" or "diamond" ? new string(' ', size.Value - row) + new string('*', 2 * row - 1) :
                    pattern == "right" ? new string(' ', size.Value - row) + new string('*', stars) : new string('*', stars));
            }
            if (pattern == "diamond") for (int row = size.Value - 1; row >= 1; row--)
                lines.Add(new string(' ', size.Value - row) + new string('*', 2 * row - 1));
            ViewBag.Result = string.Join("\n", lines);
            return View();
        }

        // 4. Countdown Timer
        [HttpGet]
        public IActionResult CountdownTimer()
        {
            ViewBag.Duration = 10;
            GenerateCountdownSequence(10);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CountdownTimer(int? duration)
        {
            if (!duration.HasValue || duration.Value < 1 || duration.Value > 300)
            {
                ViewBag.Error = "Duration must be between 1 and 300 seconds.";
                ViewBag.Duration = 10;
                GenerateCountdownSequence(10);
                return View();
            }

            int sec = duration.Value;
            ViewBag.Duration = sec;
            GenerateCountdownSequence(sec);
            ViewBag.Success = true;

            return View();
        }

        private void GenerateCountdownSequence(int seconds)
        {
            // Repetition Statement: C# while loop to simulate countdown sequence ticks
            var sequence = new List<Dictionary<string, object>>();
            int current = seconds;

            while (current >= 0)
            {
                string milestone = current == 0 ? "🚀 Blastoff / Time's up!" :
                                   current == 1 ? "1 second remaining — Final tick!" :
                                   $"{current} seconds remaining";

                sequence.Add(new Dictionary<string, object>
                {
                    ["second"] = current,
                    ["percent"] = (int)Math.Round(((double)(seconds - current) / seconds) * 100),
                    ["milestone"] = milestone
                });

                current--;
            }

            ViewBag.Sequence = sequence;
        }

        // 5. PIN Verification System
        private string CurrentPin => HttpContext.Session.GetString("DemoPin")!;

        [HttpGet]
        public IActionResult PinVerification()
        {
            var previousPin = HttpContext.Session.GetString("DemoPin");
            string newPin;
            do { newPin = System.Security.Cryptography.RandomNumberGenerator.GetInt32(10000).ToString("D4"); }
            while (newPin == previousPin);
            HttpContext.Session.SetString("DemoPin", newPin);
            HttpContext.Session.SetInt32("PinRemaining", 3);
            HttpContext.Session.SetInt32("PinLocked", 0);
            ViewBag.RemainingAttempts = HttpContext.Session.GetInt32("PinRemaining") ?? 3;
            ViewBag.IsLocked = HttpContext.Session.GetInt32("PinLocked") == 1;
            ViewBag.DemoPin = CurrentPin;
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult PinVerification(string? pin, string? action)
        {
            EnsurePinSession();
            ViewBag.DemoPin = CurrentPin;
            if (action == "reset")
            {
                HttpContext.Session.SetInt32("PinRemaining", 3);
                HttpContext.Session.SetInt32("PinLocked", 0);
                ViewBag.RemainingAttempts = 3;
                ViewBag.IsLocked = false;
                ViewBag.Message = "Demo reset. You have three attempts.";
                return View();
            }
            int remaining = HttpContext.Session.GetInt32("PinRemaining") ?? 3;
            ViewBag.RemainingAttempts = remaining;
            ViewBag.IsLocked = HttpContext.Session.GetInt32("PinLocked") == 1;
            if (ViewBag.IsLocked)
            {
                ViewBag.Error = "The demo is locked. Reset it to try again.";
                return View();
            }
            var attempts = (pin ?? "").Split(',', StringSplitOptions.TrimEntries);
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(pin) || pin.Length > 30 || attempts.Length > 3 ||
                attempts.Any(value => value.Length != 4 || !value.All(char.IsAsciiDigit)))
            {
                ViewBag.Error = "Enter 1–3 four-digit PINs, separated by commas.";
                return View();
            }
            var history = new List<string>();
            int index = 0;
            bool verified;
            do
            {
                verified = attempts[index] == CurrentPin;
                history.Add($"Attempt {4 - remaining}: " + (verified ? "Access granted" : "Incorrect PIN"));
                if (!verified) remaining--;
                index++;
            } while (index < attempts.Length && !verified && remaining > 0);

            HttpContext.Session.SetInt32("PinRemaining", verified ? 3 : remaining);
            HttpContext.Session.SetInt32("PinLocked", remaining == 0 ? 1 : 0);
            ViewBag.History = history;
            ViewBag.RemainingAttempts = verified ? 3 : remaining;
            ViewBag.IsLocked = remaining == 0;
            ViewBag.Success = verified;
            if (verified) ViewBag.Message = "Access granted. PIN verified.";
            else ViewBag.Error = remaining == 0 ? "Three incorrect attempts. The demo is locked." : $"Incorrect PIN. {remaining} attempts remaining.";
            return View();
        }

        private void EnsurePinSession()
        {
            if (HttpContext.Session.GetString("DemoPin") == null)
                HttpContext.Session.SetString("DemoPin", System.Security.Cryptography.RandomNumberGenerator.GetInt32(10000).ToString("D4"));
            if (!HttpContext.Session.GetInt32("PinRemaining").HasValue)
            {
                HttpContext.Session.SetInt32("PinRemaining", 3);
                HttpContext.Session.SetInt32("PinLocked", 0);
            }
        }
    }
}
