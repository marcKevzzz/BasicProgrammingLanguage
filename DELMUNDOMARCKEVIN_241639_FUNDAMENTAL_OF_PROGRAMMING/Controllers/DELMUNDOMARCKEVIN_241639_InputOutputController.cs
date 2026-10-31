using Microsoft.AspNetCore.Mvc;

namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Controllers
{
    public class DELMUNDOMARCKEVIN_241639_InputOutputController : Controller
    {
        // 1. Age in Days Calculator
        [HttpGet]
        public IActionResult AgeCalculator()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgeCalculator(int? age)
        {
            if (!age.HasValue || age.Value < 0 || age.Value > 130)
            {
                ViewBag.Error = "Please enter a valid age between 0 and 130 years.";
                return View();
            }

            ViewBag.Age = age.Value;
            ViewBag.Days = age.Value * 365;
            ViewBag.Success = true;
            return View();
        }

        // Keep the existing route so saved program links continue to work.
        [HttpGet]
        public IActionResult TravelFare() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult TravelFare(decimal? firstNumber, decimal? secondNumber, string? operation)
        {
            ViewBag.FirstNumber = firstNumber;
            ViewBag.SecondNumber = secondNumber;
            ViewBag.Operation = operation;
            if (!ModelState.IsValid || !firstNumber.HasValue || !secondNumber.HasValue)
            {
                ViewBag.Error = "Enter two valid numbers.";
                return View();
            }
            if (operation is not ("add" or "subtract" or "multiply" or "divide"))
            {
                ViewBag.Error = "Choose an operation.";
                return View();
            }
            if (operation == "divide" && secondNumber == 0)
            {
                ViewBag.Error = "Cannot divide by zero.";
                return View();
            }
            try
            {
                ViewBag.Result = operation switch
                {
                    "add" => firstNumber.Value + secondNumber.Value,
                    "subtract" => firstNumber.Value - secondNumber.Value,
                    "multiply" => firstNumber.Value * secondNumber.Value,
                    _ => firstNumber.Value / secondNumber.Value
                };
                ViewBag.Success = true;
            }
            catch (OverflowException) { ViewBag.Error = "The result is too large. Try smaller numbers."; }
            return View();
        }

        // 3. Study Time Calculator
        [HttpGet]
        public IActionResult StudyTime()
        {
            ViewBag.Entries = new List<Dictionary<string, object>>
            {
                new() { ["subject"] = "Programming Fundamentals", ["hours"] = 2.0 },
                new() { ["subject"] = "Mathematics", ["hours"] = 1.5 },
                new() { ["subject"] = "English", ["hours"] = 1.0 }
            };
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StudyTime(string[]? subjectNames, double[]? studyHours)
        {
            if (subjectNames == null || studyHours == null || subjectNames.Length == 0 || subjectNames.Length != studyHours.Length || !ModelState.IsValid)
            {
                ViewBag.Error = "Please enter at least one academic subject and its study duration.";
                return View();
            }

            var entries = new List<Dictionary<string, object>>();
            double totalHours = 0;

            for (int i = 0; i < subjectNames.Length && i < studyHours.Length; i++)
            {
                string name = (subjectNames[i] ?? "").Trim();
                double hours = studyHours[i];

                if (string.IsNullOrWhiteSpace(name)) continue;
                if (!double.IsFinite(hours) || hours < 0 || !double.IsFinite(totalHours + hours))
                {
                    ViewBag.Error = $"Study hours for '{name}' must be a finite, non-negative number.";
                    return View();
                }

                totalHours += hours;
                entries.Add(new Dictionary<string, object>
                {
                    ["subject"] = name,
                    ["hours"] = hours
                });
            }

            if (entries.Count == 0)
            {
                ViewBag.Error = "Please provide valid subject names.";
                return View();
            }

            double averageHours = totalHours / entries.Count;

            ViewBag.Entries = entries;
            ViewBag.TotalHours = totalHours;
            ViewBag.AverageHours = averageHours;
            ViewBag.Success = true;
            return View();
        }

        // 4. Electricity Bill Estimator
        [HttpGet]
        public IActionResult ElectricityBill()
        {
            ViewBag.Consumption = 240.0;
            ViewBag.Rate = 11.85;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ElectricityBill(double? consumption, double? rate)
        {
            if (!consumption.HasValue || consumption.Value < 0)
            {
                ViewBag.Error = "Electricity consumption cannot be negative.";
                ViewBag.Consumption = consumption;
                ViewBag.Rate = rate;
                return View();
            }

            if (!rate.HasValue || rate.Value <= 0)
            {
                ViewBag.Error = "Rate per kWh must be greater than zero.";
                ViewBag.Consumption = consumption;
                ViewBag.Rate = rate;
                return View();
            }

            double totalBill = consumption.Value * rate.Value;

            ViewBag.Consumption = consumption.Value;
            ViewBag.Rate = rate.Value;
            ViewBag.TotalBill = totalBill;
            ViewBag.Success = true;
            return View();
        }

        // 5. Birthday Countdown
        [HttpGet]
        public IActionResult BirthdayCountdown()
        {
            ViewBag.Today = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BirthdayCountdown(DateOnly? birthday)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            ViewBag.Today = today.ToString("yyyy-MM-dd");
            ViewBag.Birthday = birthday?.ToString("yyyy-MM-dd");
            if (!ModelState.IsValid || !birthday.HasValue || birthday.Value > today)
            {
                ViewBag.Error = "Enter a valid birth date that is today or earlier.";
                return View();
            }

            var birthDate = birthday.Value;
            var nextBirthday = birthDate.AddYears(today.Year - birthDate.Year);
            if (nextBirthday < today)
                nextBirthday = birthDate.AddYears(today.Year + 1 - birthDate.Year);

            int days = nextBirthday.DayNumber - today.DayNumber;
            ViewBag.NextBirthday = nextBirthday.ToString("MMMM d, yyyy");
            ViewBag.DaysRemaining = days;
            ViewBag.Weeks = days / 7;
            ViewBag.RemainingDays = days % 7;
            ViewBag.Success = true;
            return View();
        }
    }
}
