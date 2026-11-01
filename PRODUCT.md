# FUNDAMENTAL OF PROGRAMMING

An ASP.NET Core MVC collection of interactive C# exercises and small record-management programs. Learners browse by category, open a program, enter values, and inspect results or optional teaching notes.

Keep existing calculations, validation, antiforgery checks, and CRUD workflows. Use controllers, Razor views, ViewBag, and services; array exercises use a validated input model. Records remain in memory and reset on restart.

The shared ProgramCatalog service owns category and program metadata for the index, sidebar, and global search. Add future programs there after implementing their controller action and view.

The catalog contains Basic Input-Output (5), Nested If…Else (5), Loops (1 for, 2 while, 2 do…while), Arrays (5), and CRUD (5), for 25 programs. The latest user request replaces the array set with largest/smallest, sum/average, reversal, bubble sort (four one-dimensional exercises), and matrix addition (one two-dimensional exercise), superseding the PDF array split. The do…while exercises are Number Guessing Game and PIN Verification; both process user-supplied attempts in order and stop on success. PIN verification also stops at a three-failure lockout. All seven updated exercises include How it works and actual C# code sections with copy buttons.

The application folder, project, namespaces, controller files/classes, view folders/files, and model use the prefix `DELMUNDOMARCKEVIN_241639_`. MVC convention files `_ViewImports.cshtml` and `_ViewStart.cshtml` keep their required filenames. Razor view lookup is configured for prefixed view filenames; action names remain readable.

Open `DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.slnx`, or run:

```powershell
dotnet run --project DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING
```

Verification: `tests/fundamental-requirements.py`, `dotnet run --project tests/Fundamentals`, `dotnet run --project tests/BirthdayCountdown`, and `node tests/ui.test.cjs`. Start a fresh verification instance on port 5214 before running `tests/fundamental-http.py`; its CRUD checks create and remove a test record.

Payroll Calculator replaces Internet Plan Recommender and appears last in Conditional, after Password Strength Checker; the old InternetPlan GET redirects to Payroll. It simulates basic monthly salary for an SSS-covered employee using the January 2025 SSS employee schedule, the 5% PhilHealth premium split equally, HDMF Circular 460, and BIR RR 11-2018 Annex E monthly withholding brackets effective January 2023 onwards. It excludes overtime, bonuses, loans, minimum-wage exemptions, and annual adjustments. The three interactive 2D array pages include focused C# equivalents of their JavaScript logic.
