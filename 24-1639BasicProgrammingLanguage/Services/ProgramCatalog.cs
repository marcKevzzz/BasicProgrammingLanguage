namespace _24_1639BasicProgrammingLanguage.Services;

// One catalog feeds navigation, search, and the program index.
public sealed class ProgramCatalog
{
    public IReadOnlyList<Dictionary<string, object>> Categories { get; }
    public IReadOnlyList<Dictionary<string, object>> Programs { get; }

    public ProgramCatalog()
    {
            var categories = new List<Dictionary<string, object>>
            {
                new()
                {
                    ["id"] = "io",
                    ["name"] = "Input / Output",
                    ["icon"] = "bi-terminal",
                    ["desc"] = "Read input and calculate results."
                },
                new()
                {
                    ["id"] = "conditional",
                    ["name"] = "Conditions",
                    ["icon"] = "bi-diagram-3",
                    ["desc"] = "Use conditions to make decisions."
                },
                new()
                {
                    ["id"] = "repetition",
                    ["name"] = "Loops",
                    ["icon"] = "bi-arrow-repeat",
                    ["desc"] = "Work with loops and repeated steps."
                },
                new()
                {
                    ["id"] = "crud",
                    ["name"] = "Collections",
                    ["icon"] = "bi-database",
                    ["desc"] = "Add, edit, and remove records."
                }
            };

            var programs = new List<Dictionary<string, object>>
            {
                // Category 1: Input / Output
                new() {
                    ["id"] = 1,
                    ["title"] = "Age in Days Calculator",
                    ["desc"] = "Convert your age in years to days.",
                    ["category"] = "Input / Output",
                    ["categoryId"] = "io",
                    ["difficulty"] = "Beginner",
                    ["controller"] = "InputOutput",
                    ["action"] = "AgeCalculator",
                    ["icon"] = "bi-calendar-heart"
                },
                new() {
                    ["id"] = 2,
                    ["title"] = "Travel Fare Calculator",
                    ["desc"] = "Calculate a fare from distance and rate per kilometer.",
                    ["category"] = "Input / Output",
                    ["categoryId"] = "io",
                    ["difficulty"] = "Beginner",
                    ["controller"] = "InputOutput",
                    ["action"] = "TravelFare",
                    ["icon"] = "bi-car-front"
                },
                new() {
                    ["id"] = 3,
                    ["title"] = "Study Time Calculator",
                    ["desc"] = "Find total and average study hours.",
                    ["category"] = "Input / Output",
                    ["categoryId"] = "io",
                    ["difficulty"] = "Beginner",
                    ["controller"] = "InputOutput",
                    ["action"] = "StudyTime",
                    ["icon"] = "bi-book"
                },
                new() {
                    ["id"] = 4,
                    ["title"] = "Electricity Bill Estimator",
                    ["desc"] = "Estimate a bill from usage and electricity rate.",
                    ["category"] = "Input / Output",
                    ["categoryId"] = "io",
                    ["difficulty"] = "Beginner",
                    ["controller"] = "InputOutput",
                    ["action"] = "ElectricityBill",
                    ["icon"] = "bi-lightning-charge"
                },
                new() {
                    ["id"] = 5,
                    ["title"] = "Birthday Countdown",
                    ["desc"] = "Convert days into weeks and remaining days.",
                    ["category"] = "Input / Output",
                    ["categoryId"] = "io",
                    ["difficulty"] = "Beginner",
                    ["controller"] = "InputOutput",
                    ["action"] = "BirthdayCountdown",
                    ["icon"] = "bi-gift"
                },

                // Category 2: Conditions
                new() {
                    ["id"] = 6,
                    ["title"] = "Outfit Recommendation",
                    ["desc"] = "Choose an outfit for the weather.",
                    ["category"] = "Conditions",
                    ["categoryId"] = "conditional",
                    ["difficulty"] = "Beginner",
                    ["controller"] = "Conditional",
                    ["action"] = "OutfitRecommendation",
                    ["icon"] = "bi-cloud-sun"
                },
                new() {
                    ["id"] = 7,
                    ["title"] = "Scholarship Qualification",
                    ["desc"] = "Check eligibility using grades and household income.",
                    ["category"] = "Conditions",
                    ["categoryId"] = "conditional",
                    ["difficulty"] = "Intermediate",
                    ["controller"] = "Conditional",
                    ["action"] = "ScholarshipQualification",
                    ["icon"] = "bi-award"
                },
                new() {
                    ["id"] = 8,
                    ["title"] = "Internet Plan Recommender",
                    ["desc"] = "Find a plan for your budget and household.",
                    ["category"] = "Conditions",
                    ["categoryId"] = "conditional",
                    ["difficulty"] = "Intermediate",
                    ["controller"] = "Conditional",
                    ["action"] = "InternetPlan",
                    ["icon"] = "bi-wifi"
                },
                new() {
                    ["id"] = 9,
                    ["title"] = "BMI Category Checker",
                    ["desc"] = "Calculate BMI from height and weight.",
                    ["category"] = "Conditions",
                    ["categoryId"] = "conditional",
                    ["difficulty"] = "Beginner",
                    ["controller"] = "Conditional",
                    ["action"] = "BmiChecker",
                    ["icon"] = "bi-heart-pulse"
                },
                new() {
                    ["id"] = 10,
                    ["title"] = "Password Strength Checker",
                    ["desc"] = "Check password length and character types.",
                    ["category"] = "Conditions",
                    ["categoryId"] = "conditional",
                    ["difficulty"] = "Intermediate",
                    ["controller"] = "Conditional",
                    ["action"] = "PasswordStrength",
                    ["icon"] = "bi-shield-check"
                },

                // Category 3: Loops
                new() {
                    ["id"] = 11,
                    ["title"] = "Number Guessing Game",
                    ["desc"] = "Guess a number from 1 to 100.",
                    ["category"] = "Loops",
                    ["categoryId"] = "repetition",
                    ["difficulty"] = "Intermediate",
                    ["controller"] = "Repetition",
                    ["action"] = "NumberGuessing",
                    ["icon"] = "bi-dice-5"
                },
                new() {
                    ["id"] = 12,
                    ["title"] = "Daily Expense Tracker",
                    ["desc"] = "Add expenses and calculate the total.",
                    ["category"] = "Loops",
                    ["categoryId"] = "repetition",
                    ["difficulty"] = "Intermediate",
                    ["controller"] = "Repetition",
                    ["action"] = "ExpenseTracker",
                    ["icon"] = "bi-wallet2"
                },
                new() {
                    ["id"] = 13,
                    ["title"] = "Savings Challenge Simulator",
                    ["desc"] = "See how daily deposits add up.",
                    ["category"] = "Loops",
                    ["categoryId"] = "repetition",
                    ["difficulty"] = "Beginner",
                    ["controller"] = "Repetition",
                    ["action"] = "SavingsChallenge",
                    ["icon"] = "bi-piggy-bank"
                },
                new() {
                    ["id"] = 14,
                    ["title"] = "Countdown Timer",
                    ["desc"] = "Start, pause, and reset a countdown.",
                    ["category"] = "Loops",
                    ["categoryId"] = "repetition",
                    ["difficulty"] = "Intermediate",
                    ["controller"] = "Repetition",
                    ["action"] = "CountdownTimer",
                    ["icon"] = "bi-stopwatch"
                },
                new() {
                    ["id"] = 15,
                    ["title"] = "PIN Verification System",
                    ["desc"] = "Try a PIN with a three-attempt limit.",
                    ["category"] = "Loops",
                    ["categoryId"] = "repetition",
                    ["difficulty"] = "Intermediate",
                    ["controller"] = "Repetition",
                    ["action"] = "PinVerification",
                    ["icon"] = "bi-key"
                },

                // Category 4: Collections
                new() {
                    ["id"] = 16,
                    ["title"] = "Mini Library Manager",
                    ["desc"] = "Keep track of books, authors, and genres.",
                    ["category"] = "Collections",
                    ["categoryId"] = "crud",
                    ["difficulty"] = "Advanced",
                    ["controller"] = "Library",
                    ["action"] = "Index",
                    ["icon"] = "bi-journal-bookmark"
                },
                new() {
                    ["id"] = 17,
                    ["title"] = "Personal Wishlist Manager",
                    ["desc"] = "Track items, prices, and purchases.",
                    ["category"] = "Collections",
                    ["categoryId"] = "crud",
                    ["difficulty"] = "Advanced",
                    ["controller"] = "Wishlist",
                    ["action"] = "Index",
                    ["icon"] = "bi-stars"
                },
                new() {
                    ["id"] = 18,
                    ["title"] = "Lost and Found Tracker",
                    ["desc"] = "Record lost items and their recovery status.",
                    ["category"] = "Collections",
                    ["categoryId"] = "crud",
                    ["difficulty"] = "Advanced",
                    ["controller"] = "LostFound",
                    ["action"] = "Index",
                    ["icon"] = "bi-search-heart"
                },
                new() {
                    ["id"] = 19,
                    ["title"] = "Student Attendance Manager",
                    ["desc"] = "Record attendance by student, section, and date.",
                    ["category"] = "Collections",
                    ["categoryId"] = "crud",
                    ["difficulty"] = "Advanced",
                    ["controller"] = "Attendance",
                    ["action"] = "Index",
                    ["icon"] = "bi-person-check"
                },
                new() {
                    ["id"] = 20,
                    ["title"] = "Game Collection Manager",
                    ["desc"] = "Track platforms, ratings, and play status.",
                    ["category"] = "Collections",
                    ["categoryId"] = "crud",
                    ["difficulty"] = "Advanced",
                    ["controller"] = "GameCollection",
                    ["action"] = "Index",
                    ["icon"] = "bi-controller"
                }
            };

        foreach (var category in categories)
            category["count"] = programs.Count(program => Equals(program["categoryId"], category["id"]));
        Categories = categories.AsReadOnly();
        Programs = programs.AsReadOnly();
    }
}
