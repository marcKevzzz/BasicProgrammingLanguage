# FUNDAMENTAL OF PROGRAMMING

An ASP.NET Core MVC collection of interactive C# exercises and small record-management programs. Learners browse by category, open a program, enter values, and inspect results or optional teaching notes.

Keep existing calculations, validation, antiforgery checks, and CRUD workflows. Use controllers, Razor views, ViewBag, and services; array exercises use a validated input model. Records remain in memory and reset on restart.

The shared ProgramCatalog service owns category and program metadata for the index, sidebar, and global search. Add future programs there after implementing their controller action and view.

The catalog contains 20 required exercises: Basic Input-Output (5), Nested If…Else (5), Loops (1 for, 2 while, 2 do…while), and Arrays (2 one-dimensional, 3 two-dimensional); five CRUD programs are extra. Every exercise accepts user input. The 2D programs execute C# rectangular-array logic in GridExercises; their POST actions keep state in the session. Maze has five fixed solvable layouts, seating uses bool[,] and decimal[,], and Tic-Tac-Toe uses char[,]. JavaScript only submits forms without a reload and preserves focus; the forms work without it.

The application folder, project, namespaces, controller files/classes, view folders/files, and model use the prefix `DELMUNDOMARCKEVIN_241639_`. MVC convention files `_ViewImports.cshtml` and `_ViewStart.cshtml` keep their required filenames. Razor view lookup is configured for prefixed view filenames; action names remain readable.

Open `DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.slnx`, or run:

```powershell
dotnet run --project DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING
```

Verification: `dotnet run --project tests/Fundamentals`, `dotnet run --project tests/BirthdayCountdown`, `node tests/ui.test.cjs`, and `node tests/toast.test.cjs`.

Payroll Calculator replaces Internet Plan Recommender and appears last in Conditional, after Password Strength Checker; the old InternetPlan GET redirects to Payroll. It simulates basic monthly salary for an SSS-covered employee using the January 2025 SSS employee schedule, the 5% PhilHealth premium split equally, HDMF Circular 460, and BIR RR 11-2018 Annex E monthly withholding brackets effective January 2023 onwards. It excludes overtime, bonuses, loans, minimum-wage exemptions, and annual adjustments. The three interactive 2D array pages show the actual executing C# methods. Payroll nests the salary precision condition inside the positive-salary branch before calculating contributions and tax.

MVC maintenance: retain the existing models without adding new model classes. ArrayCalculations holds parsing, grade conversion, and bubble sort; controllers pass results to views. CrudValidation shares rules between all five CRUD Create/Edit pairs and checks numeric ranges, finite prices, date formats, and allowed selections. Legacy LargestSmallest, ReverseArray, and MatrixAddition GET/POST requests redirect to TheaterSeating, TicTacToe, and Maze respectively; their obsolete views and calculations have been removed.
