using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace _24_1639BasicProgrammingLanguage.Controllers
{
    public class RepetitionController : Controller
    {
        // 1. Number Guessing Game (Uses Server-Side Session)
        [HttpGet]
        public IActionResult NumberGuessing()
        {
            EnsureSecretNumber();
            ViewBag.Attempts = HttpContext.Session.GetInt32("GuessAttempts") ?? 0;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NumberGuessing(int? guess, string? action)
        {
            if (action == "restart")
            {
                HttpContext.Session.Remove("SecretNumber");
                HttpContext.Session.Remove("GuessAttempts");
                HttpContext.Session.Remove("GuessHistory");
                EnsureSecretNumber();
                ViewBag.Message = "New game started! Guess a number between 1 and 100.";
                ViewBag.MessageType = "info";
                ViewBag.Attempts = 0;
                return View();
            }

            EnsureSecretNumber();
            int secret = HttpContext.Session.GetInt32("SecretNumber")!.Value;
            int attempts = (HttpContext.Session.GetInt32("GuessAttempts") ?? 0) + 1;
            HttpContext.Session.SetInt32("GuessAttempts", attempts);

            var historyJson = HttpContext.Session.GetString("GuessHistory");
            var history = string.IsNullOrEmpty(historyJson)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(historyJson) ?? new List<string>();

            if (!guess.HasValue || guess.Value < 1 || guess.Value > 100)
            {
                ViewBag.Error = "Please enter an integer guess between 1 and 100.";
                ViewBag.Attempts = attempts - 1; // do not penalize invalid format
                HttpContext.Session.SetInt32("GuessAttempts", attempts - 1);
                ViewBag.History = history;
                return View();
            }

            int playerGuess = guess.Value;
            string feedback;
            string feedbackType;
            bool isWon = false;

            if (playerGuess == secret)
            {
                feedback = $"🎉 Spectacular! You discovered the secret number ({secret}) in {attempts} attempt(s)!";
                feedbackType = "success";
                isWon = true;
                history.Insert(0, $"Guess {playerGuess}: Correct! (Attempt #{attempts})");
            }
            else if (playerGuess > secret)
            {
                feedback = $"📉 {playerGuess} is Too High! Try a smaller number.";
                feedbackType = "warning";
                history.Insert(0, $"Guess {playerGuess}: Too High");
            }
            else
            {
                feedback = $"📈 {playerGuess} is Too Low! Try a higher number.";
                feedbackType = "primary";
                history.Insert(0, $"Guess {playerGuess}: Too Low");
            }

            HttpContext.Session.SetString("GuessHistory", JsonSerializer.Serialize(history));

            ViewBag.PlayerGuess = playerGuess;
            ViewBag.Message = feedback;
            ViewBag.MessageType = feedbackType;
            ViewBag.Attempts = attempts;
            ViewBag.IsWon = isWon;
            ViewBag.History = history;
            ViewBag.Success = true;

            return View();
        }

        private void EnsureSecretNumber()
        {
            if (!HttpContext.Session.GetInt32("SecretNumber").HasValue)
            {
                var rand = new Random();
                int secret = rand.Next(1, 101); // 1 to 100
                HttpContext.Session.SetInt32("SecretNumber", secret);
                HttpContext.Session.SetInt32("GuessAttempts", 0);
                HttpContext.Session.SetString("GuessHistory", "[]");
            }
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

            foreach (var item in expenses)
            {
                total += Convert.ToDouble(item["amount"]);
                count++;
            }

            double average = count > 0 ? total / count : 0.0;

            ViewBag.Expenses = expenses;
            ViewBag.TotalExpenses = total;
            ViewBag.AverageExpense = average;
            ViewBag.ExpenseCount = count;
        }

        // 3. Savings Challenge Simulator
        [HttpGet]
        public IActionResult SavingsChallenge()
        {
            ViewBag.StartingSavings = 500.0;
            ViewBag.DailySavings = 50.0;
            ViewBag.Days = 14;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SavingsChallenge(double? startingSavings, double? dailySavings, int? days)
        {
            if (!startingSavings.HasValue || startingSavings.Value < 0)
            {
                ViewBag.Error = "Starting savings balance cannot be negative.";
                return View();
            }

            if (!dailySavings.HasValue || dailySavings.Value <= 0)
            {
                ViewBag.Error = "Daily savings deposit must be greater than ₱0.00.";
                return View();
            }

            if (!days.HasValue || days.Value <= 0 || days.Value > 365)
            {
                ViewBag.Error = "Number of days must be between 1 and 365.";
                return View();
            }

            double initial = startingSavings.Value;
            double increment = dailySavings.Value;
            int totalDays = days.Value;

            var timeline = new List<Dictionary<string, object>>();
            double runningTotal = initial;

            // Repetition Statement: C# for loop to calculate day-by-day progression
            for (int day = 1; day <= totalDays; day++)
            {
                double dailyDeposit = increment * day; // Incremental challenge model
                runningTotal += dailyDeposit;

                timeline.Add(new Dictionary<string, object>
                {
                    ["day"] = day,
                    ["deposit"] = dailyDeposit,
                    ["total"] = runningTotal
                });
            }

            ViewBag.StartingSavings = initial;
            ViewBag.DailySavings = increment;
            ViewBag.Days = totalDays;
            ViewBag.FinalSavings = runningTotal;
            ViewBag.TotalDeposited = runningTotal - initial;
            ViewBag.Timeline = timeline;
            ViewBag.Success = true;

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
        private const string DEMO_PIN = "8024";

        [HttpGet]
        public IActionResult PinVerification()
        {
            EnsurePinSession();
            ViewBag.RemainingAttempts = HttpContext.Session.GetInt32("PinRemaining") ?? 3;
            ViewBag.IsLocked = HttpContext.Session.GetInt32("PinLocked") == 1;
            ViewBag.DemoPin = DEMO_PIN;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PinVerification(string? pin, string? action)
        {
            EnsurePinSession();

            if (action == "reset")
            {
                HttpContext.Session.SetInt32("PinRemaining", 3);
                HttpContext.Session.SetInt32("PinLocked", 0);
                ViewBag.RemainingAttempts = 3;
                ViewBag.IsLocked = false;
                ViewBag.DemoPin = DEMO_PIN;
                ViewBag.Message = "Security demo lock state has been reset. You have 3 attempts.";
                ViewBag.MessageType = "info";
                return View();
            }

            bool isLocked = HttpContext.Session.GetInt32("PinLocked") == 1;
            int remaining = HttpContext.Session.GetInt32("PinRemaining") ?? 3;

            ViewBag.DemoPin = DEMO_PIN;

            if (isLocked)
            {
                ViewBag.Error = "⛔ System is LOCKED. Maximum verification attempts exceeded. Please click 'Reset Demo' to retry.";
                ViewBag.RemainingAttempts = 0;
                ViewBag.IsLocked = true;
                return View();
            }

            if (string.IsNullOrWhiteSpace(pin) || pin.Length != 4 || !pin.All(char.IsDigit))
            {
                ViewBag.Error = "Please enter exactly four numeric digits.";
                ViewBag.RemainingAttempts = remaining;
                ViewBag.IsLocked = isLocked;
                return View();
            }

            if (pin == DEMO_PIN)
            {
                // Correct PIN
                HttpContext.Session.SetInt32("PinRemaining", 3);
                HttpContext.Session.SetInt32("PinLocked", 0);

                ViewBag.Success = true;
                ViewBag.Message = "🔓 Access Granted! PIN verified successfully.";
                ViewBag.MessageType = "success";
                ViewBag.RemainingAttempts = 3;
                ViewBag.IsLocked = false;
                return View();
            }
            else
            {
                // Incorrect PIN: decrement attempts using while/do logic
                remaining--;
                HttpContext.Session.SetInt32("PinRemaining", remaining);

                if (remaining <= 0)
                {
                    HttpContext.Session.SetInt32("PinLocked", 1);
                    ViewBag.Error = "🚨 Security Lockout Triggered! 3 failed attempts reached. Terminal locked.";
                    ViewBag.RemainingAttempts = 0;
                    ViewBag.IsLocked = true;
                }
                else
                {
                    ViewBag.Error = $"❌ Incorrect PIN entered. {remaining} attempt(s) remaining before security lockout.";
                    ViewBag.RemainingAttempts = remaining;
                    ViewBag.IsLocked = false;
                }

                return View();
            }
        }

        private void EnsurePinSession()
        {
            if (!HttpContext.Session.GetInt32("PinRemaining").HasValue)
            {
                HttpContext.Session.SetInt32("PinRemaining", 3);
                HttpContext.Session.SetInt32("PinLocked", 0);
            }
        }
    }
}
