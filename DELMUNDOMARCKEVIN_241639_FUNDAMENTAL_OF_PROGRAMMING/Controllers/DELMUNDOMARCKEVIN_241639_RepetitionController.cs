using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Controllers
{
    public class DELMUNDOMARCKEVIN_241639_RepetitionController : Controller
    {
        [HttpGet]
        public IActionResult NumberGuessing()
        {
            var model = new DELMUNDOMARCKEVIN_241639_NumberGuessingModel();
            EnsureSecretNumber();
            model.History = GetGuessHistory();
            model.Attempts = HttpContext.Session.GetInt32("GuessAttempts") ?? 0;
            model.IsWon = HttpContext.Session.GetInt32("GuessWon") == 1;
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult NumberGuessing(string? guesses, string? action)
        {
            var model = new DELMUNDOMARCKEVIN_241639_NumberGuessingModel();
            if (action == "restart")
            {
                HttpContext.Session.Remove("SecretNumber");
                HttpContext.Session.Remove("GuessAttempts");
                HttpContext.Session.Remove("GuessHistory");
                HttpContext.Session.Remove("GuessWon");
                EnsureSecretNumber();
                model.Attempts = 0;
                model.Message = "New game started.";
                return View(model);
            }
            model.History = GetGuessHistory();
            model.Attempts = HttpContext.Session.GetInt32("GuessAttempts") ?? 0;
            var tokens = (guesses ?? "").Split(',', StringSplitOptions.TrimEntries);
            var numbers = new int[tokens.Length];
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(guesses) || guesses.Length > 200 || tokens.Length > 20)
            {
                model.Error = "Enter 1–20 guesses from 1 to 100, separated by commas.";
                return View(model);
            }
            for (int i = 0; i < tokens.Length; i++)
            {
                if (!int.TryParse(tokens[i], out numbers[i]) || numbers[i] < 1 || numbers[i] > 100)
                {
                    model.Error = "Every guess must be a whole number from 1 to 100.";
                    return View(model);
                }
            }
            EnsureSecretNumber();
            int secret = HttpContext.Session.GetInt32("SecretNumber")!.Value;
            int attempts = HttpContext.Session.GetInt32("GuessAttempts") ?? 0;
            if (HttpContext.Session.GetInt32("GuessWon") == 1)
            {
                model.IsWon = true;
                model.Attempts = attempts;
                model.Message = "You already found the number. Start a new game to play again.";
                return View(model);
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
            model.Attempts = attempts;
            model.IsWon = correct;
            model.History = history;
            int latestGuess = numbers[index - 1];
            model.Message = correct ? $"Correct! The number was {secret}." : latestGuess < secret ? $"{latestGuess} is too low. Try a higher number." : $"{latestGuess} is too high. Try a lower number.";
            model.MessageType = correct ? "success" : latestGuess < secret ? "higher" : "lower";
            return View(model);
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
            var model = new DELMUNDOMARCKEVIN_241639_ExpenseTrackerModel();
            var expenses = GetSessionExpenses();
            CalculateExpenseMetrics(expenses, model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExpenseTracker(string? name, double? amount, string? action)
        {
            var model = new DELMUNDOMARCKEVIN_241639_ExpenseTrackerModel();
            var expenses = GetSessionExpenses();

            if (action == "clear")
            {
                expenses.Clear();
                SaveSessionExpenses(expenses);
                model.Message = "All recorded expenses have been cleared.";
                CalculateExpenseMetrics(expenses, model);
                return View(model);
            }

            if (action == "add")
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    model.Error = "Please specify an expense description or category.";
                    CalculateExpenseMetrics(expenses, model);
                    return View(model);
                }

                if (!amount.HasValue || amount.Value <= 0)
                {
                    model.Error = "Expense amount must be greater than ₱0.00.";
                    CalculateExpenseMetrics(expenses, model);
                    return View(model);
                }

                expenses.Add(new DELMUNDOMARCKEVIN_241639_ExpenseEntry(name.Trim(), amount.Value, DateTime.Now.ToString("MMM dd, yyyy HH:mm")));

                SaveSessionExpenses(expenses);
                model.Message = $"Added '{name}' for ₱{amount.Value:N2}.";
            }

            CalculateExpenseMetrics(expenses, model);
            return View(model);
        }

        private List<DELMUNDOMARCKEVIN_241639_ExpenseEntry> GetSessionExpenses()
        {
            var json = HttpContext.Session.GetString("DailyExpenses");
            if (string.IsNullOrEmpty(json))
            {
                // Seed 3 starter daily expenses
                var initial = new List<DELMUNDOMARCKEVIN_241639_ExpenseEntry>
                {
                    new("Breakfast Combo & Coffee", 165.00, DateTime.Now.ToString("MMM dd, yyyy 08:30")),
                    new("Train & Jeepney Commute", 85.00, DateTime.Now.ToString("MMM dd, yyyy 12:15")),
                    new("Notebook & Study Pens", 210.00, DateTime.Now.ToString("MMM dd, yyyy 15:40"))
                };
                SaveSessionExpenses(initial);
                return initial;
            }

            try
            {
                return JsonSerializer.Deserialize<List<DELMUNDOMARCKEVIN_241639_ExpenseEntry>>(json) ?? new();
            }
            catch (JsonException)
            {
                return new List<DELMUNDOMARCKEVIN_241639_ExpenseEntry>();
            }
        }

        private void SaveSessionExpenses(List<DELMUNDOMARCKEVIN_241639_ExpenseEntry> expenses)
        {
            HttpContext.Session.SetString("DailyExpenses", JsonSerializer.Serialize(expenses));
        }

        private void CalculateExpenseMetrics(List<DELMUNDOMARCKEVIN_241639_ExpenseEntry> expenses, DELMUNDOMARCKEVIN_241639_ExpenseTrackerModel model)
        {
            // Explicit Repetition Statement Requirement: Use C# loop to process collection
            double total = 0;
            int count = 0;

            while (count < expenses.Count)
            {
                total += expenses[count].Amount;
                count++;
            }

            double average = count > 0 ? total / count : 0.0;

            model.Expenses = expenses;
            model.TotalExpenses = total;
            model.AverageExpense = average;
            model.ExpenseCount = count;
        }

        [HttpGet]
        public IActionResult SavingsChallenge() => View(new DELMUNDOMARCKEVIN_241639_SavingsChallengeModel());

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult SavingsChallenge(int? size, string? pattern)
        {
            var model = new DELMUNDOMARCKEVIN_241639_SavingsChallengeModel();
            model.Size = size; model.Pattern = pattern;
            if (!ModelState.IsValid || size is null or < 1 or > 20 || pattern is not ("left" or "inverted" or "right" or "pyramid" or "diamond"))
            { model.Error = "Choose a pattern and a base from 1 to 20."; return View(model); }
            var lines = new List<string>();
            for (int row = 1; row <= size; row++)
            {
                int stars = pattern == "inverted" ? size.Value - row + 1 : row;
                lines.Add(pattern is "pyramid" or "diamond" ? new string(' ', size.Value - row) + new string('*', 2 * row - 1) :
                    pattern == "right" ? new string(' ', size.Value - row) + new string('*', stars) : new string('*', stars));
            }
            if (pattern == "diamond") for (int row = size.Value - 1; row >= 1; row--)
                lines.Add(new string(' ', size.Value - row) + new string('*', 2 * row - 1));
            model.Result = string.Join("\n", lines);
            return View(model);
        }

        // 4. Countdown Timer
        [HttpGet]
        public IActionResult CountdownTimer()
        {
            var model = new DELMUNDOMARCKEVIN_241639_CountdownTimerModel();
            model.Duration = 10;
            GenerateCountdownSequence(10, model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CountdownTimer(int? duration)
        {
            var model = new DELMUNDOMARCKEVIN_241639_CountdownTimerModel();
            if (!duration.HasValue || duration.Value < 1 || duration.Value > 300)
            {
                model.Error = "Duration must be between 1 and 300 seconds.";
                model.Duration = 10;
                GenerateCountdownSequence(10, model);
                return View(model);
            }

            int sec = duration.Value;
            model.Duration = sec;
            GenerateCountdownSequence(sec, model);
            model.Success = true;

            return View(model);
        }

        private void GenerateCountdownSequence(int seconds, DELMUNDOMARCKEVIN_241639_CountdownTimerModel model)
        {
            // Repetition Statement: C# while loop to simulate countdown sequence ticks
            var sequence = new List<DELMUNDOMARCKEVIN_241639_CountdownTick>();
            int current = seconds;

            while (current >= 0)
            {
                string milestone = current == 0 ? "🚀 Blastoff / Time's up!" :
                                   current == 1 ? "1 second remaining — Final tick!" :
                                   $"{current} seconds remaining";

                sequence.Add(new DELMUNDOMARCKEVIN_241639_CountdownTick(current, (int)Math.Round(((double)(seconds - current) / seconds) * 100), milestone));

                current--;
            }

            model.Sequence = sequence;
        }

        // 5. PIN Verification System
        private string CurrentPin => HttpContext.Session.GetString("DemoPin")!;

        [HttpGet]
        public IActionResult PinVerification()
        {
            var model = new DELMUNDOMARCKEVIN_241639_PinVerificationModel();
            var previousPin = HttpContext.Session.GetString("DemoPin");
            string newPin;
            do { newPin = System.Security.Cryptography.RandomNumberGenerator.GetInt32(10000).ToString("D4"); }
            while (newPin == previousPin);
            HttpContext.Session.SetString("DemoPin", newPin);
            HttpContext.Session.SetInt32("PinRemaining", 3);
            HttpContext.Session.SetInt32("PinLocked", 0);
            model.RemainingAttempts = HttpContext.Session.GetInt32("PinRemaining") ?? 3;
            model.IsLocked = HttpContext.Session.GetInt32("PinLocked") == 1;
            model.DemoPin = CurrentPin;
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult PinVerification(string? pin, string? action)
        {
            var model = new DELMUNDOMARCKEVIN_241639_PinVerificationModel();
            EnsurePinSession();
            model.DemoPin = CurrentPin;
            if (action == "reset")
            {
                HttpContext.Session.SetInt32("PinRemaining", 3);
                HttpContext.Session.SetInt32("PinLocked", 0);
                model.RemainingAttempts = 3;
                model.IsLocked = false;
                model.Message = "Demo reset. You have three attempts.";
                return View(model);
            }
            int remaining = HttpContext.Session.GetInt32("PinRemaining") ?? 3;
            model.RemainingAttempts = remaining;
            model.IsLocked = HttpContext.Session.GetInt32("PinLocked") == 1;
            if (model.IsLocked)
            {
                model.Error = "The demo is locked. Reset it to try again.";
                return View(model);
            }
            var attempts = (pin ?? "").Split(',', StringSplitOptions.TrimEntries);
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(pin) || pin.Length > 30 || attempts.Length > 3 ||
                attempts.Any(value => value.Length != 4 || !value.All(char.IsAsciiDigit)))
            {
                model.Error = "Enter 1–3 four-digit PINs, separated by commas.";
                return View(model);
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
            model.History = history;
            model.RemainingAttempts = verified ? 3 : remaining;
            model.IsLocked = remaining == 0;
            model.Success = verified;
            if (verified) model.Message = "Access granted. PIN verified.";
            else model.Error = remaining == 0 ? "Three incorrect attempts. The demo is locked." : $"Incorrect PIN. {remaining} attempts remaining.";
            return View(model);
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
