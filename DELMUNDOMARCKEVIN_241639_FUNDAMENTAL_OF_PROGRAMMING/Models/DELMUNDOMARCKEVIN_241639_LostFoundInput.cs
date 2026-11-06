using System.ComponentModel.DataAnnotations;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;
namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;
public sealed class DELMUNDOMARCKEVIN_241639_LostFoundInput : IValidatableObject
{
    public string? Name { get; set; } = "";
    public string? Description { get; set; } = "No additional description provided.";
    public string? Location { get; set; } = "";
    public string? Date { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
    public string? Status { get; set; } = "Found";
    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    { var error = CrudValidation.LostFound(Name, Location, Date, Status); if (error != null) yield return new ValidationResult(error); }
    public DELMUNDOMARCKEVIN_241639_LostFoundItem ToRecord(int id = 0) => new(id, string.IsNullOrWhiteSpace(Name) ? "" : Name.Trim(), string.IsNullOrWhiteSpace(Description) ? "No additional description provided." : Description.Trim(), string.IsNullOrWhiteSpace(Location) ? "" : Location.Trim(), string.IsNullOrWhiteSpace(Date) ? DateTime.Now.ToString("yyyy-MM-dd") : Date.Trim(), string.IsNullOrWhiteSpace(Status) ? "Found" : Status.Trim());
    public static DELMUNDOMARCKEVIN_241639_LostFoundInput FromRecord(DELMUNDOMARCKEVIN_241639_LostFoundItem record) => new() { Name = record.Name, Description = record.Description, Location = record.Location, Date = record.Date, Status = record.Status };
}
