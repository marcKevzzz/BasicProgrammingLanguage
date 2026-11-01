namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;

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
                    ["name"] = "Basic Input-Output",
                    ["icon"] = "bi-terminal",
                    ["desc"] = "Read input and calculate results."
                },
                new()
                {
                    ["id"] = "conditional",
                    ["name"] = "Nested If…Else",
                    ["icon"] = "bi-diagram-3",
                    ["desc"] = "Use nested if/else statements to make decisions."
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
                    ["id"] = "arrays",
                    ["name"] = "Arrays",
                    ["icon"] = "bi-grid-3x3",
                    ["desc"] = "Four common one-dimensional array programs and matrix addition."
                },
                new()
                {
                    ["id"] = "crud",
                    ["name"] = "CRUD",
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
                    ["desc"] = "Play a two-player game on a 3 × 3 board.",
                    ["category"] = "Basic Input-Output",
                    ["categoryId"] = "io",
                    ["difficulty"] = "Beginner",
                    ["controller"] = "DELMUNDOMARCKEVIN_241639_InputOutput",
                    ["action"] = "AgeCalculator",
                    ["icon"] = "bi-calendar-heart"
                },
                new() {
                    ["id"] = 2,
                    ["title"] = "Basic Calculator",
                    ["desc"] = "Add, subtract, multiply, or divide two numbers.",
                    ["category"] = "Basic Input-Output",
                    ["categoryId"] = "io",
                    ["difficulty"] = "Beginner",
                    ["controller"] = "DELMUNDOMARCKEVIN_241639_InputOutput",
                    ["action"] = "TravelFare",
                    ["icon"] = "bi-calculator"
                },
                new() {
                    ["id"] = 3,
                    ["title"] = "Study Time Calculator",
                    ["desc"] = "Find total and average study hours.",
                    ["category"] = "Basic Input-Output",
                    ["categoryId"] = "io",
                    ["difficulty"] = "Beginner",
                    ["controller"] = "DELMUNDOMARCKEVIN_241639_InputOutput",
                    ["action"] = "StudyTime",
                    ["icon"] = "bi-book"
                },
                new() {
                    ["id"] = 4,
                    ["title"] = "Electricity Bill Estimator",
                    ["desc"] = "Estimate a bill from usage and electricity rate.",
                    ["category"] = "Basic Input-Output",
                    ["categoryId"] = "io",
                    ["difficulty"] = "Beginner",
                    ["controller"] = "DELMUNDOMARCKEVIN_241639_InputOutput",
                    ["action"] = "ElectricityBill",
                    ["icon"] = "bi-lightning-charge"
                },
                new() {
                    ["id"] = 5,
                    ["title"] = "Birthday Countdown",
                    ["desc"] = "Find the days and weeks until your next birthday.",
                    ["category"] = "Basic Input-Output",
                    ["categoryId"] = "io",
                    ["difficulty"] = "Beginner",
                    ["controller"] = "DELMUNDOMARCKEVIN_241639_InputOutput",
                    ["action"] = "BirthdayCountdown",
                    ["icon"] = "bi-gift"
                },

                // Category 2: Conditions
                new() {
                    ["id"] = 6,
                    ["title"] = "Outfit Recommendation",
                    ["desc"] = "Choose an outfit for the weather.",
                    ["category"] = "Nested If…Else",
                    ["categoryId"] = "conditional",
                    ["difficulty"] = "Beginner",
                    ["controller"] = "DELMUNDOMARCKEVIN_241639_Conditional",
                    ["action"] = "OutfitRecommendation",
                    ["icon"] = "bi-cloud-sun"
                },
                new() {
                    ["id"] = 7,
                    ["title"] = "Scholarship Qualification",
                    ["desc"] = "Check eligibility using grades and household income.",
                    ["category"] = "Nested If…Else",
                    ["categoryId"] = "conditional",
                    ["difficulty"] = "Intermediate",
                    ["controller"] = "DELMUNDOMARCKEVIN_241639_Conditional",
                    ["action"] = "ScholarshipQualification",
                    ["icon"] = "bi-award"
                },
                new() {
                    ["id"] = 9,
                    ["title"] = "BMI Category Checker",
                    ["desc"] = "Calculate BMI from height and weight.",
                    ["category"] = "Nested If…Else",
                    ["categoryId"] = "conditional",
                    ["difficulty"] = "Beginner",
                    ["controller"] = "DELMUNDOMARCKEVIN_241639_Conditional",
                    ["action"] = "BmiChecker",
                    ["icon"] = "bi-heart-pulse"
                },
                new() {
                    ["id"] = 10,
                    ["title"] = "Password Strength Checker",
                    ["desc"] = "Check password length and character types.",
                    ["category"] = "Nested If…Else",
                    ["categoryId"] = "conditional",
                    ["difficulty"] = "Intermediate",
                    ["controller"] = "DELMUNDOMARCKEVIN_241639_Conditional",
                    ["action"] = "PasswordStrength",
                    ["icon"] = "bi-shield-check"
                },

                new() {
                    ["id"] = 8,
                    ["title"] = "Payroll Calculator",
                    ["desc"] = "Calculate monthly take-home pay after Philippine contributions and withholding tax.",
                    ["category"] = "Nested If…Else",
                    ["categoryId"] = "conditional",
                    ["difficulty"] = "Intermediate",
                    ["controller"] = "DELMUNDOMARCKEVIN_241639_Conditional",
                    ["action"] = "Payroll",
                    ["icon"] = "bi-receipt"
                },

                // Loops: for, while, then do…while.
                new() {
                    ["id"] = 13,
                    ["title"] = "Star Patterns (for)",
                    ["desc"] = "Create five star patterns.",
                    ["category"] = "Loops",
                    ["categoryId"] = "repetition",
                    ["difficulty"] = "Beginner",
                    ["controller"] = "DELMUNDOMARCKEVIN_241639_Repetition",
                    ["action"] = "SavingsChallenge",
                    ["icon"] = "bi-piggy-bank"
                },
                new() {
                    ["id"] = 12,
                    ["title"] = "Daily Expense Tracker (while)",
                    ["desc"] = "Add expenses and calculate the total.",
                    ["category"] = "Loops",
                    ["categoryId"] = "repetition",
                    ["difficulty"] = "Intermediate",
                    ["controller"] = "DELMUNDOMARCKEVIN_241639_Repetition",
                    ["action"] = "ExpenseTracker",
                    ["icon"] = "bi-wallet2"
                },
                new() {
                    ["id"] = 14,
                    ["title"] = "Countdown Timer (while)",
                    ["desc"] = "Start, pause, and reset a countdown.",
                    ["category"] = "Loops",
                    ["categoryId"] = "repetition",
                    ["difficulty"] = "Intermediate",
                    ["controller"] = "DELMUNDOMARCKEVIN_241639_Repetition",
                    ["action"] = "CountdownTimer",
                    ["icon"] = "bi-stopwatch"
                },
                new() {
                    ["id"] = 11,
                    ["title"] = "Number Guessing Game (do…while)",
                    ["desc"] = "Guess the secret number, repeating attempts until correct.",
                    ["category"] = "Loops",
                    ["categoryId"] = "repetition",
                    ["difficulty"] = "Intermediate",
                    ["controller"] = "DELMUNDOMARCKEVIN_241639_Repetition",
                    ["action"] = "NumberGuessing",
                    ["icon"] = "bi-dice-5"
                },
                new() {
                    ["id"] = 15,
                    ["title"] = "PIN Verification (do…while)",
                    ["desc"] = "Retry a demo PIN, stopping on success or after three failures.",
                    ["category"] = "Loops",
                    ["categoryId"] = "repetition",
                    ["difficulty"] = "Intermediate",
                    ["controller"] = "DELMUNDOMARCKEVIN_241639_Repetition",
                    ["action"] = "PinVerification",
                    ["icon"] = "bi-key"
                },

                new() {
                    ["id"] = 22,
                    ["title"] = "General Weighted Average (1D)",
                    ["desc"] = "Calculate your average and school grade equivalent.",
                    ["category"] = "Arrays",
                    ["categoryId"] = "arrays",
                    ["difficulty"] = "Intermediate",
                    ["controller"] = "DELMUNDOMARCKEVIN_241639_Arrays",
                    ["action"] = "SumAverage",
                    ["icon"] = "bi-calculator"
                },
                new() {
                    ["id"] = 25,
                    ["title"] = "Bubble Sort (1D)",
                    ["desc"] = "Sort an array in ascending order using adjacent swaps.",
                    ["category"] = "Arrays",
                    ["categoryId"] = "arrays",
                    ["difficulty"] = "Intermediate",
                    ["controller"] = "DELMUNDOMARCKEVIN_241639_Arrays",
                    ["action"] = "BubbleSort",
                    ["icon"] = "bi-sort-numeric-down"
                },
                new() {
                    ["id"] = 23,
                    ["title"] = "Maze Explorer (2D)",
                    ["desc"] = "Navigate a maze from start to exit.",
                    ["category"] = "Arrays",
                    ["categoryId"] = "arrays",
                    ["difficulty"] = "Intermediate",
                    ["controller"] = "DELMUNDOMARCKEVIN_241639_Arrays",
                    ["action"] = "Maze",
                    ["icon"] = "bi-grid-3x3"
                },
                new() {
                    ["id"] = 21,
                    ["title"] = "Theater Seating Chart (2D)",
                    ["desc"] = "Select and release seats in a theater grid.",
                    ["category"] = "Arrays",
                    ["categoryId"] = "arrays",
                    ["difficulty"] = "Intermediate",
                    ["controller"] = "DELMUNDOMARCKEVIN_241639_Arrays",
                    ["action"] = "TheaterSeating",
                    ["icon"] = "bi-ticket-perforated"
                },
                new() {
                    ["id"] = 24,
                    ["title"] = "Tic-Tac-Toe (2D)",
                    ["desc"] = "Play a two-player game on a 3 × 3 board.",
                    ["category"] = "Arrays",
                    ["categoryId"] = "arrays",
                    ["difficulty"] = "Intermediate",
                    ["controller"] = "DELMUNDOMARCKEVIN_241639_Arrays",
                    ["action"] = "TicTacToe",
                    ["icon"] = "bi-grid-3x3-gap"
                },

                // Category 5: CRUD
                new() {
                    ["id"] = 16,
                    ["title"] = "Mini Library Manager",
                    ["desc"] = "Keep track of books, authors, and genres.",
                    ["category"] = "CRUD",
                    ["categoryId"] = "crud",
                    ["difficulty"] = "Advanced",
                    ["controller"] = "DELMUNDOMARCKEVIN_241639_Library",
                    ["action"] = "Index",
                    ["icon"] = "bi-journal-bookmark"
                },
                new() {
                    ["id"] = 17,
                    ["title"] = "Personal Wishlist Manager",
                    ["desc"] = "Track items, prices, and purchases.",
                    ["category"] = "CRUD",
                    ["categoryId"] = "crud",
                    ["difficulty"] = "Advanced",
                    ["controller"] = "DELMUNDOMARCKEVIN_241639_Wishlist",
                    ["action"] = "Index",
                    ["icon"] = "bi-stars"
                },
                new() {
                    ["id"] = 18,
                    ["title"] = "Lost and Found Tracker",
                    ["desc"] = "Record lost items and their recovery status.",
                    ["category"] = "CRUD",
                    ["categoryId"] = "crud",
                    ["difficulty"] = "Advanced",
                    ["controller"] = "DELMUNDOMARCKEVIN_241639_LostFound",
                    ["action"] = "Index",
                    ["icon"] = "bi-search-heart"
                },
                new() {
                    ["id"] = 19,
                    ["title"] = "Student Attendance Manager",
                    ["desc"] = "Record attendance by student, section, and date.",
                    ["category"] = "CRUD",
                    ["categoryId"] = "crud",
                    ["difficulty"] = "Advanced",
                    ["controller"] = "DELMUNDOMARCKEVIN_241639_Attendance",
                    ["action"] = "Index",
                    ["icon"] = "bi-person-check"
                },
                new() {
                    ["id"] = 20,
                    ["title"] = "Game Collection Manager",
                    ["desc"] = "Track platforms, ratings, and play status.",
                    ["category"] = "CRUD",
                    ["categoryId"] = "crud",
                    ["difficulty"] = "Advanced",
                    ["controller"] = "DELMUNDOMARCKEVIN_241639_GameCollection",
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
