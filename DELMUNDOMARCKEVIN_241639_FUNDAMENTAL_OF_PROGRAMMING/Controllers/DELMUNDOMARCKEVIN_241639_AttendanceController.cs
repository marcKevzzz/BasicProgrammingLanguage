using Microsoft.AspNetCore.Mvc;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;

namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Controllers
{
    public class DELMUNDOMARCKEVIN_241639_AttendanceController : Controller
    {
        private readonly InMemoryCrudStore _store;

        public DELMUNDOMARCKEVIN_241639_AttendanceController(InMemoryCrudStore store)
        {
            _store = store;
        }

        // GET: /Attendance
        public IActionResult Index(string? search, string? status)
        {
            var records = _store.GetAll("attendance");

            if (!string.IsNullOrWhiteSpace(search))
            {
                string query = search.Trim().ToLowerInvariant();
                records = records.Where(x =>
                    (x.ContainsKey("name") && x["name"].ToString()!.ToLowerInvariant().Contains(query)) ||
                    (x.ContainsKey("studentNumber") && x["studentNumber"].ToString()!.ToLowerInvariant().Contains(query)) ||
                    (x.ContainsKey("course") && x["course"].ToString()!.ToLowerInvariant().Contains(query)) ||
                    (x.ContainsKey("section") && x["section"].ToString()!.ToLowerInvariant().Contains(query))
                ).ToList();
                ViewBag.Search = search;
            }

            if (!string.IsNullOrWhiteSpace(status) && status != "All")
            {
                records = records.Where(x =>
                    x.ContainsKey("status") && x["status"].ToString()!.Equals(status, StringComparison.OrdinalIgnoreCase)
                ).ToList();
                ViewBag.StatusFilter = status;
            }

            ViewBag.AttendanceList = records;
            return View();
        }

        // GET: /DELMUNDOMARCKEVIN_241639_Attendance/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /DELMUNDOMARCKEVIN_241639_Attendance/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string? name, string? studentNumber, string? course, string? section, string? date, string? status)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ViewBag.Error = "Student name is required.";
                ViewBag.NameInput = name;
                ViewBag.StudentNumberInput = studentNumber;
                ViewBag.CourseInput = course;
                ViewBag.SectionInput = section;
                ViewBag.DateInput = date;
                ViewBag.StatusInput = status;
                return View();
            }

            if (string.IsNullOrWhiteSpace(studentNumber))
            {
                ViewBag.Error = "Student number is required.";
                ViewBag.NameInput = name;
                ViewBag.StudentNumberInput = studentNumber;
                ViewBag.CourseInput = course;
                ViewBag.SectionInput = section;
                ViewBag.DateInput = date;
                ViewBag.StatusInput = status;
                return View();
            }

            var record = new Dictionary<string, object>
            {
                ["name"] = name.Trim(),
                ["studentNumber"] = studentNumber.Trim(),
                ["course"] = string.IsNullOrWhiteSpace(course) ? "BS Computer Science" : course.Trim(),
                ["section"] = string.IsNullOrWhiteSpace(section) ? "CS-1A" : section.Trim(),
                ["date"] = string.IsNullOrWhiteSpace(date) ? DateTime.Now.ToString("yyyy-MM-dd") : date.Trim(),
                ["status"] = string.IsNullOrWhiteSpace(status) ? "Present" : status.Trim()
            };

            int newId = _store.Insert("attendance", record);
            TempData["Success"] = $"Attendance record #{newId} for '{record["name"]}' saved successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /DELMUNDOMARCKEVIN_241639_Attendance/Edit/1
        public IActionResult Edit(int id)
        {
            var record = _store.GetById("attendance", id);
            if (record == null)
            {
                TempData["Error"] = $"Attendance record #{id} was not found.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Record = record;
            return View();
        }

        // POST: /DELMUNDOMARCKEVIN_241639_Attendance/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, string? name, string? studentNumber, string? course, string? section, string? date, string? status)
        {
            var existing = _store.GetById("attendance", id);
            if (existing == null)
            {
                TempData["Error"] = $"Attendance record #{id} not found.";
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(studentNumber))
            {
                ViewBag.Error = "Student name and student number cannot be empty.";
                ViewBag.Record = new Dictionary<string, object>
                {
                    ["id"] = id,
                    ["name"] = name ?? "",
                    ["studentNumber"] = studentNumber ?? "",
                    ["course"] = course ?? "",
                    ["section"] = section ?? "",
                    ["date"] = date ?? "",
                    ["status"] = status ?? "Present"
                };
                return View();
            }

            var updated = new Dictionary<string, object>
            {
                ["name"] = name.Trim(),
                ["studentNumber"] = studentNumber.Trim(),
                ["course"] = string.IsNullOrWhiteSpace(course) ? "BS Computer Science" : course.Trim(),
                ["section"] = string.IsNullOrWhiteSpace(section) ? "CS-1A" : section.Trim(),
                ["date"] = string.IsNullOrWhiteSpace(date) ? DateTime.Now.ToString("yyyy-MM-dd") : date.Trim(),
                ["status"] = string.IsNullOrWhiteSpace(status) ? "Present" : status.Trim()
            };

            _store.Update("attendance", id, updated);
            TempData["Success"] = $"Attendance record #{id} for '{updated["name"]}' updated.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /DELMUNDOMARCKEVIN_241639_Attendance/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            bool removed = _store.Delete("attendance", id);
            if (removed)
            {
                TempData["Success"] = $"Attendance entry #{id} was removed.";
            }
            else
            {
                TempData["Error"] = $"Attendance entry #{id} not found.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
