using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;
namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;

// One catalog feeds navigation, search, and the program index.
public sealed class ProgramCatalog
{
    public IReadOnlyList<DELMUNDOMARCKEVIN_241639_ProgramCategory> Categories { get; }
    public IReadOnlyList<DELMUNDOMARCKEVIN_241639_ProgramEntry> Programs { get; }

    public ProgramCatalog()
    {
            var categories = new List<DELMUNDOMARCKEVIN_241639_ProgramCategory>
            {
                new("io", "Basic Input-Output", "bi-terminal", "Read input and calculate results."),
                new("conditional", "Nested If…Else", "bi-diagram-3", "Use nested if/else statements to make decisions."),
                new("repetition", "Loops", "bi-arrow-repeat", "Work with loops and repeated steps."),
                new("arrays", "Arrays", "bi-grid-3x3", "Two one-dimensional and three two-dimensional array exercises."),
                new("crud", "CRUD", "bi-database", "Add, edit, and remove records.")
            };

            var programs = new List<DELMUNDOMARCKEVIN_241639_ProgramEntry>
            {
                // Category 1: Input / Output
                new(1, "Age in Days Calculator", "Convert an age in years to an estimated number of days.", "Basic Input-Output", "io", "Beginner", "DELMUNDOMARCKEVIN_241639_InputOutput", "AgeCalculator", "bi-calendar-heart"),
                new(2, "Basic Calculator", "Add, subtract, multiply, or divide two numbers.", "Basic Input-Output", "io", "Beginner", "DELMUNDOMARCKEVIN_241639_InputOutput", "TravelFare", "bi-calculator"),
                new(3, "Study Time Calculator", "Find total and average study hours.", "Basic Input-Output", "io", "Beginner", "DELMUNDOMARCKEVIN_241639_InputOutput", "StudyTime", "bi-book"),
                new(4, "Electricity Bill Estimator", "Estimate a bill from usage and electricity rate.", "Basic Input-Output", "io", "Beginner", "DELMUNDOMARCKEVIN_241639_InputOutput", "ElectricityBill", "bi-lightning-charge"),
                new(5, "Birthday Countdown", "Find the days and weeks until your next birthday.", "Basic Input-Output", "io", "Beginner", "DELMUNDOMARCKEVIN_241639_InputOutput", "BirthdayCountdown", "bi-gift"),

                // Category 2: Conditions
                new(6, "Outfit Recommendation", "Choose an outfit for the weather.", "Nested If…Else", "conditional", "Beginner", "DELMUNDOMARCKEVIN_241639_Conditional", "OutfitRecommendation", "bi-cloud-sun"),
                new(7, "Scholarship Qualification", "Check eligibility using grades and household income.", "Nested If…Else", "conditional", "Intermediate", "DELMUNDOMARCKEVIN_241639_Conditional", "ScholarshipQualification", "bi-award"),
                new(9, "BMI Category Checker", "Calculate BMI from height and weight.", "Nested If…Else", "conditional", "Beginner", "DELMUNDOMARCKEVIN_241639_Conditional", "BmiChecker", "bi-heart-pulse"),
                new(10, "Password Strength Checker", "Check password length and character types.", "Nested If…Else", "conditional", "Intermediate", "DELMUNDOMARCKEVIN_241639_Conditional", "PasswordStrength", "bi-shield-check"),

                new(8, "Payroll Calculator", "Calculate monthly take-home pay after Philippine contributions and withholding tax.", "Nested If…Else", "conditional", "Intermediate", "DELMUNDOMARCKEVIN_241639_Conditional", "Payroll", "bi-receipt"),

                // Loops: for, while, then do…while.
                new(13, "Star Patterns (for)", "Create five star patterns.", "Loops", "repetition", "Beginner", "DELMUNDOMARCKEVIN_241639_Repetition", "SavingsChallenge", "bi-piggy-bank"),
                new(12, "Daily Expense Tracker (while)", "Add expenses and calculate the total.", "Loops", "repetition", "Intermediate", "DELMUNDOMARCKEVIN_241639_Repetition", "ExpenseTracker", "bi-wallet2"),
                new(14, "Countdown Timer (while)", "Start, pause, and reset a countdown.", "Loops", "repetition", "Intermediate", "DELMUNDOMARCKEVIN_241639_Repetition", "CountdownTimer", "bi-stopwatch"),
                new(11, "Number Guessing Game (do…while)", "Guess the secret number, repeating attempts until correct.", "Loops", "repetition", "Intermediate", "DELMUNDOMARCKEVIN_241639_Repetition", "NumberGuessing", "bi-dice-5"),
                new(15, "PIN Verification (do…while)", "Retry a demo PIN, stopping on success or after three failures.", "Loops", "repetition", "Intermediate", "DELMUNDOMARCKEVIN_241639_Repetition", "PinVerification", "bi-key"),

                new(22, "General Weighted Average (1D)", "Calculate your average and school grade equivalent.", "Arrays", "arrays", "Intermediate", "DELMUNDOMARCKEVIN_241639_Arrays", "SumAverage", "bi-calculator"),
                new(25, "Bubble Sort (1D)", "Sort an array in ascending order using adjacent swaps.", "Arrays", "arrays", "Intermediate", "DELMUNDOMARCKEVIN_241639_Arrays", "BubbleSort", "bi-sort-numeric-down"),
                new(23, "Maze Explorer (2D)", "Navigate a maze from start to exit.", "Arrays", "arrays", "Intermediate", "DELMUNDOMARCKEVIN_241639_Arrays", "Maze", "bi-grid-3x3"),
                new(21, "Theater Seating Chart (2D)", "Select and release seats in a theater grid.", "Arrays", "arrays", "Intermediate", "DELMUNDOMARCKEVIN_241639_Arrays", "TheaterSeating", "bi-ticket-perforated"),
                new(24, "Tic-Tac-Toe (2D)", "Play a two-player game on a 3 × 3 board.", "Arrays", "arrays", "Intermediate", "DELMUNDOMARCKEVIN_241639_Arrays", "TicTacToe", "bi-grid-3x3-gap"),

                // Category 5: CRUD
                new(16, "Mini Library Manager", "Keep track of books, authors, and genres.", "CRUD", "crud", "Advanced", "DELMUNDOMARCKEVIN_241639_Library", "Index", "bi-journal-bookmark"),
                new(17, "Personal Wishlist Manager", "Track items, prices, and purchases.", "CRUD", "crud", "Advanced", "DELMUNDOMARCKEVIN_241639_Wishlist", "Index", "bi-stars"),
                new(18, "Lost and Found Tracker", "Record lost items and their recovery status.", "CRUD", "crud", "Advanced", "DELMUNDOMARCKEVIN_241639_LostFound", "Index", "bi-search-heart"),
                new(19, "Student Attendance Manager", "Record attendance by student, section, and date.", "CRUD", "crud", "Advanced", "DELMUNDOMARCKEVIN_241639_Attendance", "Index", "bi-person-check"),
                new(20, "Game Collection Manager", "Track platforms, ratings, and play status.", "CRUD", "crud", "Advanced", "DELMUNDOMARCKEVIN_241639_GameCollection", "Index", "bi-controller")
            };

        Categories = categories.Select(category => category with { Count = programs.Count(program => program.CategoryId == category.Id) }).ToArray();
        Programs = programs.AsReadOnly();
    }
}
