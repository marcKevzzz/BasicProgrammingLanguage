using Microsoft.AspNetCore.Mvc;

namespace _24_1639BasicProgrammingLanguage.Controllers
{
    public class InputOutputController : Controller
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

        // 2. Travel Fare Calculator
        [HttpGet]
        public IActionResult TravelFare()
        {
            // Default reasonable values for commuter convenience
            ViewBag.Distance = 12.5;
            ViewBag.Rate = 15.0;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TravelFare(double? distance, double? rate)
        {
            if (!distance.HasValue || distance.Value <= 0)
            {
                ViewBag.Error = "Distance must be greater than 0 kilometers.";
                ViewBag.Distance = distance;
                ViewBag.Rate = rate;
                return View();
            }

            if (!rate.HasValue || rate.Value <= 0)
            {
                ViewBag.Error = "Fare rate must be greater than 0 pesos per kilometer.";
                ViewBag.Distance = distance;
                ViewBag.Rate = rate;
                return View();
            }

            double totalFare = distance.Value * rate.Value;

            ViewBag.Distance = distance.Value;
            ViewBag.Rate = rate.Value;
            ViewBag.TotalFare = totalFare;
            ViewBag.Success = true;
            return View();
        }

        // 3. Study Time Calculator
        [HttpGet]
        public IActionResult StudyTime()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StudyTime(string[]? subjectNames, double[]? studyHours)
        {
            if (subjectNames == null || studyHours == null || subjectNames.Length == 0)
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
                if (hours < 0 || hours > 24)
                {
                    ViewBag.Error = $"Study hours for '{name}' must be between 0 and 24 hours.";
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
            ViewBag.DaysRemaining = 45;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BirthdayCountdown(int? daysRemaining)
        {
            if (!daysRemaining.HasValue || daysRemaining.Value < 0)
            {
                ViewBag.Error = "Remaining days must be zero or a positive whole number.";
                return View();
            }

            int days = daysRemaining.Value;
            int weeks = days / 7;
            int remainingDays = days % 7;

            ViewBag.DaysRemaining = days;
            ViewBag.Weeks = weeks;
            ViewBag.RemainingDays = remainingDays;
            ViewBag.Success = true;
            return View();
        }
    }
}
