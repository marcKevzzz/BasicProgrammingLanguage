using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;
using Microsoft.AspNetCore.Mvc;

namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Controllers
{
    public class DELMUNDOMARCKEVIN_241639_InputOutputController : Controller
    {
        // 1. Age in Days Calculator
        [HttpGet]
        public IActionResult AgeCalculator()
        {
            var model = new DELMUNDOMARCKEVIN_241639_AgeCalculatorModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgeCalculator(int? age)
        {
            var model = new DELMUNDOMARCKEVIN_241639_AgeCalculatorModel();
            if (!age.HasValue || age.Value < 0 || age.Value > 130)
            {
                model.Error = "Please enter a valid age between 0 and 130 years.";
                return View(model);
            }

            model.Age = age.Value;
            model.Days = age.Value * 365;
            model.Success = true;
            return View(model);
        }

        // Keep the existing route so saved program links continue to work.
        [HttpGet]
        public IActionResult TravelFare() => View(new DELMUNDOMARCKEVIN_241639_TravelFareModel());

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult TravelFare(decimal? firstNumber, decimal? secondNumber, string? operation)
        {
            var model = new DELMUNDOMARCKEVIN_241639_TravelFareModel();
            model.FirstNumber = firstNumber;
            model.SecondNumber = secondNumber;
            model.Operation = operation;
            if (!ModelState.IsValid || !firstNumber.HasValue || !secondNumber.HasValue)
            {
                model.Error = "Enter two valid numbers.";
                return View(model);
            }
            if (operation is not ("add" or "subtract" or "multiply" or "divide"))
            {
                model.Error = "Choose an operation.";
                return View(model);
            }
            if (operation == "divide" && secondNumber == 0)
            {
                model.Error = "Cannot divide by zero.";
                return View(model);
            }
            try
            {
                model.Result = operation switch
                {
                    "add" => firstNumber.Value + secondNumber.Value,
                    "subtract" => firstNumber.Value - secondNumber.Value,
                    "multiply" => firstNumber.Value * secondNumber.Value,
                    _ => firstNumber.Value / secondNumber.Value
                };
                model.Success = true;
            }
            catch (OverflowException) { model.Error = "The result is too large. Try smaller numbers."; }
            return View(model);
        }

        // 3. Study Time Calculator
        [HttpGet]
        public IActionResult StudyTime()
        {
            var model = new DELMUNDOMARCKEVIN_241639_StudyTimeModel();
            model.Entries = new List<DELMUNDOMARCKEVIN_241639_StudyEntry>
            {
                new("Programming Fundamentals", 2.0),
                new("Mathematics", 1.5),
                new("English", 1.0)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StudyTime(string[]? subjectNames, double[]? studyHours)
        {
            var model = new DELMUNDOMARCKEVIN_241639_StudyTimeModel();
            if (subjectNames == null || studyHours == null || subjectNames.Length == 0 || subjectNames.Length != studyHours.Length || !ModelState.IsValid)
            {
                model.Error = "Please enter at least one academic subject and its study duration.";
                return View(model);
            }

            var entries = new List<DELMUNDOMARCKEVIN_241639_StudyEntry>();
            double totalHours = 0;

            for (int i = 0; i < subjectNames.Length && i < studyHours.Length; i++)
            {
                string name = (subjectNames[i] ?? "").Trim();
                double hours = studyHours[i];

                if (string.IsNullOrWhiteSpace(name)) continue;
                if (!double.IsFinite(hours) || hours < 0 || !double.IsFinite(totalHours + hours))
                {
                    model.Error = $"Study hours for '{name}' must be a finite, non-negative number.";
                    return View(model);
                }

                totalHours += hours;
                entries.Add(new DELMUNDOMARCKEVIN_241639_StudyEntry(name, hours));
            }

            if (entries.Count == 0)
            {
                model.Error = "Please provide valid subject names.";
                return View(model);
            }

            double averageHours = totalHours / entries.Count;

            model.Entries = entries;
            model.TotalHours = totalHours;
            model.AverageHours = averageHours;
            model.Success = true;
            return View(model);
        }

        // 4. Electricity Bill Estimator
        [HttpGet]
        public IActionResult ElectricityBill()
        {
            var model = new DELMUNDOMARCKEVIN_241639_ElectricityBillModel();
            model.Consumption = 240.0;
            model.Rate = 11.85;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ElectricityBill(double? consumption, double? rate)
        {
            var model = new DELMUNDOMARCKEVIN_241639_ElectricityBillModel();
            if (!consumption.HasValue || consumption.Value < 0)
            {
                model.Error = "Electricity consumption cannot be negative.";
                model.Consumption = consumption;
                model.Rate = rate;
                return View(model);
            }

            if (!rate.HasValue || rate.Value <= 0)
            {
                model.Error = "Rate per kWh must be greater than zero.";
                model.Consumption = consumption;
                model.Rate = rate;
                return View(model);
            }

            double totalBill = consumption.Value * rate.Value;

            model.Consumption = consumption.Value;
            model.Rate = rate.Value;
            model.TotalBill = totalBill;
            model.Success = true;
            return View(model);
        }

        // 5. Birthday Countdown
        [HttpGet]
        public IActionResult BirthdayCountdown()
        {
            var model = new DELMUNDOMARCKEVIN_241639_BirthdayCountdownModel();
            model.Today = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BirthdayCountdown(DateOnly? birthday)
        {
            var model = new DELMUNDOMARCKEVIN_241639_BirthdayCountdownModel();
            var today = DateOnly.FromDateTime(DateTime.Today);
            model.Today = today.ToString("yyyy-MM-dd");
            model.Birthday = birthday?.ToString("yyyy-MM-dd");
            if (!ModelState.IsValid || !birthday.HasValue || birthday.Value > today)
            {
                model.Error = "Enter a valid birth date that is today or earlier.";
                return View(model);
            }

            var birthDate = birthday.Value;
            var nextBirthday = birthDate.AddYears(today.Year - birthDate.Year);
            if (nextBirthday < today)
                nextBirthday = birthDate.AddYears(today.Year + 1 - birthDate.Year);

            int days = nextBirthday.DayNumber - today.DayNumber;
            model.NextBirthday = nextBirthday.ToString("MMMM d, yyyy");
            model.DaysRemaining = days;
            model.Weeks = days / 7;
            model.RemainingDays = days % 7;
            model.Success = true;
            return View(model);
        }
    }
}
