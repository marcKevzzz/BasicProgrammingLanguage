using System.ComponentModel.DataAnnotations;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;
namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;
public sealed class DELMUNDOMARCKEVIN_241639_AttendanceInput : IValidatableObject
{
    public string? Name { get; set; } = "";
    public string? StudentNumber { get; set; } = "";
    public string? Course { get; set; } = "BS Computer Science";
    public string? Section { get; set; } = "CS-1A";
    public string? Date { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
    public string? Status { get; set; } = "Present";
    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    { var error = CrudValidation.Attendance(Name, StudentNumber, Date, Status); if (error != null) yield return new ValidationResult(error); }
    public DELMUNDOMARCKEVIN_241639_AttendanceRecord ToRecord(int id = 0) => new(id, string.IsNullOrWhiteSpace(Name) ? "" : Name.Trim(), string.IsNullOrWhiteSpace(StudentNumber) ? "" : StudentNumber.Trim(), string.IsNullOrWhiteSpace(Course) ? "BS Computer Science" : Course.Trim(), string.IsNullOrWhiteSpace(Section) ? "CS-1A" : Section.Trim(), string.IsNullOrWhiteSpace(Date) ? DateTime.Now.ToString("yyyy-MM-dd") : Date.Trim(), string.IsNullOrWhiteSpace(Status) ? "Present" : Status.Trim());
    public static DELMUNDOMARCKEVIN_241639_AttendanceInput FromRecord(DELMUNDOMARCKEVIN_241639_AttendanceRecord record) => new() { Name = record.Name, StudentNumber = record.StudentNumber, Course = record.Course, Section = record.Section, Date = record.Date, Status = record.Status };
}
