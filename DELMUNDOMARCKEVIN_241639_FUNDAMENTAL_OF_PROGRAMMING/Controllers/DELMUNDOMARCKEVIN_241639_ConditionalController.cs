using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;
using Microsoft.AspNetCore.Mvc;

namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Controllers
{
    public class DELMUNDOMARCKEVIN_241639_ConditionalController : Controller
    {
        // 1. Outfit Recommendation
        [HttpGet]
        public IActionResult OutfitRecommendation()
        {
            var model = new DELMUNDOMARCKEVIN_241639_OutfitRecommendationModel();
            model.Temperature = 24.0;
            model.WeatherCondition = "Sunny";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OutfitRecommendation(double? temperature, string? weatherCondition)
        {
            var model = new DELMUNDOMARCKEVIN_241639_OutfitRecommendationModel();
            if (!temperature.HasValue)
            {
                model.Error = "Please enter the ambient temperature.";
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(weatherCondition))
            {
                model.Error = "Please select the current weather condition.";
                return View(model);
            }

            double temp = temperature.Value;
            string weather = weatherCondition.Trim();
            string category;
            string top;
            string bottom;
            string accessories;
            string footwear;

            // Temperature conditional ladder
            if (temp < 10)
            {
                category = "Freezing / Very Cold";
                top = "Thermal undershirt, heavy wool sweater, and a thick insulated parka coat";
                bottom = "Fleece-lined denim or heavy trousers";
                footwear = "Insulated winter boots with thick thermal socks";
            }
            else
            {
                if (temp >= 10 && temp < 20)
                {
                    category = "Cool / Chilly";
                    top = "Long-sleeve shirt paired with a cozy cardigan, hoodie, or denim jacket";
                    bottom = "Comfortable chinos or classic jeans";
                    footwear = "Leather sneakers or ankle boots";
                }
                else
                {
                    if (temp >= 20 && temp <= 28)
                    {
                        category = "Mild & Pleasant";
                        top = "Breathable cotton t-shirt or light button-down polo";
                        bottom = "Lightweight trousers, culottes, or relaxed jeans";
                        footwear = "Everyday canvas sneakers or loafers";
                    }
                    else
                    {
                        category = "Hot & Tropical";
                        top = "Ultra-lightweight linen shirt or moisture-wicking sleeveless top";
                        bottom = "Cotton shorts or loose airy linen pants";
                        footwear = "Breathable slip-on shoes or comfortable sandals";
                    }
                }
            }

            // Weather accessory modifiers using switch statement
            switch (weather.ToLowerInvariant())
            {
                case "rainy":
                accessories = "Carry a sturdy umbrella, waterproof windbreaker, and keep valuables in a water-resistant backpack.";
                footwear += " (waterproof / quick-drying)";
                break;
                case "windy":
                accessories = "Add a wind-resistant shell jacket or scarf to block brisk gusts.";
                break;
                case "snowy":
                accessories = "Wear a warm beanie, thermal gloves, and an insulated neck gaiter.";
                break;
                case "sunny":
                accessories = "UV-blocking sunglasses, broad-spectrum sunscreen (SPF 50+), and a wide-brim hat or cap.";
                break;
                case "cloudy":
                default:
                accessories = "A light packable layer in case the sky turns overcast or breezy.";
                break;
            }

            model.Temperature = temp;
            model.WeatherCondition = weather;
            model.Category = category;
            model.Top = top;
            model.Bottom = bottom;
            model.Footwear = footwear;
            model.Accessories = accessories;
            model.Success = true;

            return View(model);
        }

        // 2. Scholarship Qualification
        [HttpGet]
        public IActionResult ScholarshipQualification()
        {
            var model = new DELMUNDOMARCKEVIN_241639_ScholarshipQualificationModel();
            model.AverageGrade = 92.5;
            model.HouseholdIncome = 22000.0;
            model.HasFailingGrades = false;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ScholarshipQualification(double? averageGrade, double? householdIncome, bool hasFailingGrades)
        {
            var model = new DELMUNDOMARCKEVIN_241639_ScholarshipQualificationModel();
            if (!averageGrade.HasValue || averageGrade.Value < 0 || averageGrade.Value > 100)
            {
                model.Error = "Please enter an average grade between 0 and 100.";
                return View(model);
            }

            if (!householdIncome.HasValue || householdIncome.Value < 0)
            {
                model.Error = "Please enter a valid monthly household income.";
                return View(model);
            }

            double gpa = averageGrade.Value;
            double income = householdIncome.Value;

            bool gpaPassed = gpa >= 90.0;
            bool incomePassed = income <= 25000.0;
            bool noFailsPassed = !hasFailingGrades;

            bool isQualified = false;
            if (gpaPassed)
            {
                if (incomePassed)
                {
                    if (noFailsPassed) isQualified = true;
                }
            }

            var criteria = new List<DELMUNDOMARCKEVIN_241639_ScholarshipCriterion>
            {
                new("Academic Grade Requirement (≥ 90.0)", gpaPassed, $"Your Grade: {gpa:F1}"),
                new("Monthly Household Income Ceiling (≤ ₱25,000)", incomePassed, $"Your Income: ₱{income:N2}"),
                new("Clean Academic Record (No Failing Marks)", noFailsPassed, hasFailingGrades ? "Has failing marks reported" : "No failing marks")
            };

            string explanation;
            if (isQualified)
            {
                explanation = "Congratulations! You meet all academic and financial requirements for the Merit & Financial Assistance Scholarship Grant.";
            }
            else
            {
                var failedReasons = new List<string>();
                if (!gpaPassed) failedReasons.Add("Average grade is below the 90.0 threshold");
                if (!incomePassed) failedReasons.Add("Monthly household income exceeds the ₱25,000 maximum ceiling");
                if (!noFailsPassed) failedReasons.Add("Applicant has one or more failing marks");

                explanation = "Application was not qualified due to: " + string.Join("; ", failedReasons) + ".";
            }

            model.AverageGrade = gpa;
            model.HouseholdIncome = income;
            model.HasFailingGrades = hasFailingGrades;
            model.IsQualified = isQualified;
            model.Explanation = explanation;
            model.Criteria = criteria;
            model.Success = true;

            return View(model);
        }

        [HttpGet]
        public IActionResult InternetPlan() => RedirectToAction(nameof(Payroll));

        [HttpGet]
        public IActionResult Payroll()
        {
            return View(new DELMUNDOMARCKEVIN_241639_PayrollModel { Salary = "30000" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Payroll(string? salary)
        {
            var model = new DELMUNDOMARCKEVIN_241639_PayrollModel { Salary = salary };
            if (!decimal.TryParse(salary, System.Globalization.NumberStyles.AllowDecimalPoint |
                System.Globalization.NumberStyles.AllowLeadingSign | System.Globalization.NumberStyles.AllowLeadingWhite |
                System.Globalization.NumberStyles.AllowTrailingWhite, System.Globalization.CultureInfo.InvariantCulture,
                out decimal basicSalary) || basicSalary <= 0 || basicSalary != Services.PayrollCalculator.Money(basicSalary))
            {
                model.Error = "Enter a positive monthly salary with up to two decimal places, such as 30000.00.";
                return View(model);
            }
            model.Result = Services.PayrollCalculator.Calculate(basicSalary);
            return View(model);
        }

        // 4. BMI Category Checker
        [HttpGet]
        public IActionResult BmiChecker()
        {
            var model = new DELMUNDOMARCKEVIN_241639_BmiCheckerModel();
            model.Weight = 68.0;
            model.Height = 172.0;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BmiChecker(double? weight, double? height)
        {
            var model = new DELMUNDOMARCKEVIN_241639_BmiCheckerModel();
            if (!weight.HasValue || weight.Value <= 10 || weight.Value > 300)
            {
                model.Error = "Please enter a valid weight in kilograms (10 - 300 kg).";
                return View(model);
            }

            if (!height.HasValue || height.Value <= 50 || height.Value > 260)
            {
                model.Error = "Please enter a valid height in centimeters (50 - 260 cm).";
                return View(model);
            }

            double w = weight.Value;
            double hMeters = height.Value / 100.0;
            double bmi = w / (hMeters * hMeters);

            string category;
            string badgeClass;
            string advice;

            if (bmi < 18.5)
            {
                category = "Underweight";
                badgeClass = "bg-warning text-dark";
                advice = "Your BMI suggests you may be below the optimal weight range. Consider consulting a nutritionist to build lean muscle mass with nutrient-dense meals.";
            }
            else
            {
                if (bmi < 25)
                {
                    category = "Normal Weight";
                    badgeClass = "bg-success text-white";
                    advice = "Great job! Your weight falls within the generally healthy range for your height. Maintain a balanced diet and regular physical activity.";
                }
                else
                {
                    if (bmi < 30)
                    {
                        category = "Overweight";
                        badgeClass = "bg-warning text-dark";
                        advice = "Your BMI is slightly above the standard range. Introducing moderate aerobic exercise and mindful portion control can promote optimal cardiovascular health.";
                    }
                    else
                    {
                        category = "Obese";
                        badgeClass = "bg-danger text-white";
                        advice = "Your BMI indicates obesity, which may increase risks of hypertension and metabolic conditions. We recommend partnering with a medical professional.";
                    }
                }
            }

            model.Weight = w;
            model.Height = height.Value;
            model.Bmi = bmi;
            model.Category = category;
            model.BadgeClass = badgeClass;
            model.Advice = advice;
            model.Success = true;

            return View(model);
        }

        // 5. Password Strength Checker
        [HttpGet]
        public IActionResult PasswordStrength()
        {
            var model = new DELMUNDOMARCKEVIN_241639_PasswordStrengthModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PasswordStrength(string? password)
        {
            var model = new DELMUNDOMARCKEVIN_241639_PasswordStrengthModel();
            if (string.IsNullOrEmpty(password))
            {
                model.Error = "Please enter a password to evaluate.";
                return View(model);
            }

            int length = password.Length;
            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

            int score = 0;
            if (length >= 8) score++;
            if (length >= 12) score++;
            if (hasUpper && hasLower) score++;
            if (hasDigit) score++;
            if (hasSpecial) score++;

            string rating;
            string badgeClass;
            int progressPercent;

            if (score <= 2)
            {
                rating = "Weak";
                badgeClass = "bg-danger text-white";
                progressPercent = 25;
            }
            else
            {
                if (score == 3 || score == 4)
                {
                    rating = "Medium";
                    badgeClass = "bg-warning text-dark";
                    progressPercent = 65;
                }
                else
                {
                    rating = "Strong";
                    badgeClass = "bg-success text-white";
                    progressPercent = 100;
                }
            }

            var suggestions = new List<string>();
            if (length < 12) suggestions.Add("Increase total length to at least 12-16 characters.");
            if (!hasUpper) suggestions.Add("Include at least one uppercase letter (A-Z).");
            if (!hasLower) suggestions.Add("Include at least one lowercase letter (a-z).");
            if (!hasDigit) suggestions.Add("Include at least one numeric digit (0-9).");
            if (!hasSpecial) suggestions.Add("Include at least one symbol or special character (!@#$%^&*).");

            if (suggestions.Count == 0)
            {
                suggestions.Add("Excellent! Your password fulfills all high-entropy security criteria.");
            }

            // Note: Never log or store the password. Pass only length and metrics to view.
            model.Rating = rating;
            model.BadgeClass = badgeClass;
            model.Score = score;
            model.Length = length;
            model.HasUpper = hasUpper;
            model.HasLower = hasLower;
            model.HasDigit = hasDigit;
            model.HasSpecial = hasSpecial;
            model.ProgressPercent = progressPercent;
            model.Suggestions = suggestions;
            model.Success = true;

            return View(model);
        }
    }
}
