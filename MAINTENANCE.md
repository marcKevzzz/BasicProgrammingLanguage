# Maintaining the application

## Models and responsibilities

All exercise and CRUD pages now receive typed models. Each model type has its own file, and both use the DELMUNDOMARCKEVIN_241639_ prefix, matching ArrayInput. Keep page titles and request metadata in ViewData; put structured inputs and results in Models.

- Crud models: immutable DELMUNDOMARCKEVIN_241639_WishlistItem, DELMUNDOMARCKEVIN_241639_Game, DELMUNDOMARCKEVIN_241639_AttendanceRecord, and DELMUNDOMARCKEVIN_241639_LostFoundItem records; editable input models; index and edit page models. IDs come from the route and are assigned by the store.
- Library models: the corresponding DELMUNDOMARCKEVIN_241639_Book models.
- Calculator models: page results for arithmetic, age, study time, electricity, birthdays, outfit recommendations, scholarships, BMI, password strength, and payroll. DELMUNDOMARCKEVIN_241639_PayrollModel wraps the existing PayrollBreakdown calculation result.
- Repetition models: guessing, PIN, expense, pattern, and countdown page state. Session-backed helpers populate these models.
- Grid models: maze, seating, and tic-tac-toe state.
- `DELMUNDOMARCKEVIN_241639_ArrayInput.cs`: array input, result, and validation feedback.
- Exercise entry models: typed study entries, scholarship criteria, expense entries, countdown ticks, and DELMUNDOMARCKEVIN_241639_IPageFeedback for shared notifications. Expense JSON property names preserve existing sessions.
- Catalog models: immutable program and category entries and the home page model.

## CRUD flow

MVC binds editable input models and runs IValidatableObject validation using CrudValidation. Controllers check ModelState before mapping input to a record. Do not call an action directly in a test and expect MVC to perform binding or validation; validate the input and populate ModelState, or test through HTTP.

TypedCrudStore indexes immutable records by ID, assigns IDs under a lock, and returns snapshots. Its Change operation performs read-modify-write atomically; Wishlist purchase toggling uses it. LibraryStore owns DELMUNDOMARCKEVIN_241639_Book storage and search. Both are singleton services registered in Program.cs. CrudSeedData supplies typed initial records for the four shared stores.

Create and Edit views share typed field partials. Use input/select/textarea tag helpers so attempted values and selections survive binding errors. Post actions retain antiforgery validation and redirect after successful writes. Keep search filters in the index model.

To add a field, update its stored record, editable input model, validation, ToRecord/FromRecord mapping, seed data, field partial, and regression checks together. Keep calculations in existing services and pass results to views rather than recalculating there.

## Verification

Requires the .NET 10 SDK. From the repository root:

```powershell
dotnet run --project .\DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING --launch-profile http
dotnet run --project .\tests\Fundamentals
dotnet run --project .\tests\BirthdayCountdown
```

Open http://localhost:5212 after starting the application. Stop the app before rebuilding if Windows reports locked output files. Checks can also run with `-p:OutputPath=bin/Validation/` to keep their output separate.

Storage remains in memory: CRUD data resets when the app restarts, and exercise state expires with the session. These changes do not add a database or deployment.
