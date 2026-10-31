# Fundamental of Programming implementation plan

**Goal:** Match the four PDF exercise categories, keep CRUD as category five, and use DELMUNDOMARCKEVIN_241639_ names.
**Architecture:** Keep ASP.NET MVC, existing styles, ViewBag exercises and the in-memory CRUD service. Add a named array input model for the new array exercises. Preserve existing uncommitted exercise changes.
**Spec:** User-confirmed four PDF categories plus CRUD. All exercises accept user input.

- [x] Check the category counts and names with a failing requirements check.
- [x] Rename the application folder/project, namespaces, controller classes/files, view folders/files, references, and test paths. Keep MVC's required _ViewStart and _ViewImports filenames.
- [x] Add two one-dimensional array exercises (statistics, search) and three two-dimensional exercises (matrix addition, transpose, row totals), with bounded validated input.
- [x] Make the five conditional samples demonstrate nested if/else, and expose one for/two while/two do-while loop samples. Keep the existing guessing/PIN work accessible.
- [x] Update the catalog, title, documentation, solution, and route/view configuration.
- [x] Run build, existing tests, new controller checks, and HTTP checks for every catalog page and form validation.

Risks to verify: view resolution after renaming; hardcoded route references; malformed/oversized numeric input; matrix shape mismatches; preserving CRUD redirects and antiforgery validation.

Verification record: naming checks, 25-page HTTP smoke check, array and loop POSTs, antiforgery rejection, all CRUD form pages, library create/edit/delete, existing birthday/session checks, and UI JavaScript checks passed. Final rebuild follows teaching-code synchronization.
