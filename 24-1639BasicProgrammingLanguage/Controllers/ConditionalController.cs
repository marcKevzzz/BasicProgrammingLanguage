using Microsoft.AspNetCore.Mvc;

namespace _24_1639BasicProgrammingLanguage.Controllers
{
    public class ConditionalController : Controller
    {
        // 1. Outfit Recommendation
        [HttpGet]
        public IActionResult OutfitRecommendation()
        {
            ViewBag.Temperature = 24.0;
            ViewBag.WeatherCondition = "Sunny";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OutfitRecommendation(double? temperature, string? weatherCondition)
        {
            if (!temperature.HasValue)
            {
                ViewBag.Error = "Please enter the ambient temperature.";
                return View();
            }

            if (string.IsNullOrWhiteSpace(weatherCondition))
            {
                ViewBag.Error = "Please select the current weather condition.";
                return View();
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
            else if (temp >= 10 && temp < 20)
            {
                category = "Cool / Chilly";
                top = "Long-sleeve shirt paired with a cozy cardigan, hoodie, or denim jacket";
                bottom = "Comfortable chinos or classic jeans";
                footwear = "Leather sneakers or ankle boots";
            }
            else if (temp >= 20 && temp <= 28)
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

            ViewBag.Temperature = temp;
            ViewBag.WeatherCondition = weather;
            ViewBag.Category = category;
            ViewBag.Top = top;
            ViewBag.Bottom = bottom;
            ViewBag.Footwear = footwear;
            ViewBag.Accessories = accessories;
            ViewBag.Success = true;

            return View();
        }

        // 2. Scholarship Qualification
        [HttpGet]
        public IActionResult ScholarshipQualification()
        {
            ViewBag.AverageGrade = 92.5;
            ViewBag.HouseholdIncome = 22000.0;
            ViewBag.HasFailingGrades = false;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ScholarshipQualification(double? averageGrade, double? householdIncome, bool hasFailingGrades)
        {
            if (!averageGrade.HasValue || averageGrade.Value < 0 || averageGrade.Value > 100)
            {
                ViewBag.Error = "Please enter an average grade between 0 and 100.";
                return View();
            }

            if (!householdIncome.HasValue || householdIncome.Value < 0)
            {
                ViewBag.Error = "Please enter a valid monthly household income.";
                return View();
            }

            double gpa = averageGrade.Value;
            double income = householdIncome.Value;

            bool gpaPassed = gpa >= 90.0;
            bool incomePassed = income <= 25000.0;
            bool noFailsPassed = !hasFailingGrades;

            bool isQualified = gpaPassed && incomePassed && noFailsPassed;

            var criteria = new List<Dictionary<string, object>>
            {
                new() { ["name"] = "Academic Grade Requirement (≥ 90.0)", ["passed"] = gpaPassed, ["detail"] = $"Your Grade: {gpa:F1}" },
                new() { ["name"] = "Monthly Household Income Ceiling (≤ ₱25,000)", ["passed"] = incomePassed, ["detail"] = $"Your Income: ₱{income:N2}" },
                new() { ["name"] = "Clean Academic Record (No Failing Marks)", ["passed"] = noFailsPassed, ["detail"] = hasFailingGrades ? "Has failing marks reported" : "No failing marks" }
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

            ViewBag.AverageGrade = gpa;
            ViewBag.HouseholdIncome = income;
            ViewBag.HasFailingGrades = hasFailingGrades;
            ViewBag.IsQualified = isQualified;
            ViewBag.Explanation = explanation;
            ViewBag.Criteria = criteria;
            ViewBag.Success = true;

            return View();
        }

        // 3. Internet Plan Recommender
        [HttpGet]
        public IActionResult InternetPlan()
        {
            ViewBag.Budget = 1600.0;
            ViewBag.Usage = "Streaming";
            ViewBag.Users = 3;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InternetPlan(double? budget, string? usage, int? users)
        {
            if (!budget.HasValue || budget.Value <= 0)
            {
                ViewBag.Error = "Please specify a monthly budget greater than zero.";
                return View();
            }

            if (!users.HasValue || users.Value <= 0)
            {
                ViewBag.Error = "Number of users must be at least 1.";
                return View();
            }

            double userBudget = budget.Value;
            int userCount = users.Value;
            string usageType = usage ?? "Streaming";

            string planName;
            double planCost;
            string planSpeed;
            string explanation;

            // Conditional recommendation tree based on user count and bandwidth demand
            if (usageType == "Gaming" || userCount >= 6)
            {
                planName = "HyperFiber Titan 600";
                planCost = 2899.00;
                planSpeed = "600 Mbps Ultra Low-Latency Fiber";
                explanation = "Recommended for heavy multi-device environments, competitive online gaming, and simultaneous 4K HDR streams.";
            }
            else if (usageType == "Streaming" || userCount >= 3)
            {
                if (userBudget < 1500)
                {
                    planName = "Fiber Connect Plus 50";
                    planCost = 1299.00;
                    planSpeed = "50 Mbps Symmetrical";
                    explanation = "A budget-optimized plan delivering reliable HD streaming and stable Zoom/Teams remote work for small households.";
                }
                else
                {
                    planName = "SpeedStream Pro 200";
                    planCost = 1899.00;
                    planSpeed = "200 Mbps Fiber Unlimited";
                    explanation = "Optimal balance for family streaming, university research, remote video conferencing, and rapid cloud backups.";
                }
            }
            else
            {
                // Basic browsing / light usage
                planName = "Fiber Starter 35";
                planCost = 899.00;
                planSpeed = "35 Mbps Fiber";
                explanation = "Ideal for 1-2 users doing daily web browsing, email correspondence, social media, and standard music/video playback.";
            }

            bool withinBudget = userBudget >= planCost;
            double budgetDifference = userBudget - planCost;

            ViewBag.Budget = userBudget;
            ViewBag.Usage = usageType;
            ViewBag.Users = userCount;
            ViewBag.PlanName = planName;
            ViewBag.PlanCost = planCost;
            ViewBag.PlanSpeed = planSpeed;
            ViewBag.Explanation = explanation;
            ViewBag.WithinBudget = withinBudget;
            ViewBag.BudgetDifference = Math.Abs(budgetDifference);
            ViewBag.Success = true;

            return View();
        }

        // 4. BMI Category Checker
        [HttpGet]
        public IActionResult BmiChecker()
        {
            ViewBag.Weight = 68.0;
            ViewBag.Height = 172.0;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BmiChecker(double? weight, double? height)
        {
            if (!weight.HasValue || weight.Value <= 10 || weight.Value > 300)
            {
                ViewBag.Error = "Please enter a valid weight in kilograms (10 - 300 kg).";
                return View();
            }

            if (!height.HasValue || height.Value <= 50 || height.Value > 260)
            {
                ViewBag.Error = "Please enter a valid height in centimeters (50 - 260 cm).";
                return View();
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
            else if (bmi >= 18.5 && bmi < 24.9)
            {
                category = "Normal Weight";
                badgeClass = "bg-success text-white";
                advice = "Great job! Your weight falls within the generally healthy range for your height. Maintain a balanced diet and regular physical activity.";
            }
            else if (bmi >= 25.0 && bmi < 29.9)
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

            ViewBag.Weight = w;
            ViewBag.Height = height.Value;
            ViewBag.Bmi = bmi;
            ViewBag.Category = category;
            ViewBag.BadgeClass = badgeClass;
            ViewBag.Advice = advice;
            ViewBag.Success = true;

            return View();
        }

        // 5. Password Strength Checker
        [HttpGet]
        public IActionResult PasswordStrength()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PasswordStrength(string? password)
        {
            if (string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Please enter a password to evaluate.";
                return View();
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
            else if (score == 3 || score == 4)
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
            ViewBag.Rating = rating;
            ViewBag.BadgeClass = badgeClass;
            ViewBag.Score = score;
            ViewBag.Length = length;
            ViewBag.HasUpper = hasUpper;
            ViewBag.HasLower = hasLower;
            ViewBag.HasDigit = hasDigit;
            ViewBag.HasSpecial = hasSpecial;
            ViewBag.ProgressPercent = progressPercent;
            ViewBag.Suggestions = suggestions;
            ViewBag.Success = true;

            return View();
        }
    }
}
