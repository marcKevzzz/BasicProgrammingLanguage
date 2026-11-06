using System.ComponentModel.DataAnnotations;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;
namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;
public sealed class DELMUNDOMARCKEVIN_241639_GameCollectionInput : IValidatableObject
{
    public string? Title { get; set; } = "";
    public string? Genre { get; set; } = "General";
    public string? Platform { get; set; } = "PC";
    public string? Status { get; set; } = "Backlog";
    public int? Rating { get; set; }
    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    { var error = CrudValidation.DELMUNDOMARCKEVIN_241639_Game(Title, Rating, Status, Platform); if (error != null) yield return new ValidationResult(error); }
    public DELMUNDOMARCKEVIN_241639_Game ToRecord(int id = 0) => new(id, string.IsNullOrWhiteSpace(Title) ? "" : Title.Trim(), string.IsNullOrWhiteSpace(Genre) ? "General" : Genre.Trim(), string.IsNullOrWhiteSpace(Platform) ? "PC" : Platform.Trim(), string.IsNullOrWhiteSpace(Status) ? "Backlog" : Status.Trim(), Rating!.Value);
    public static DELMUNDOMARCKEVIN_241639_GameCollectionInput FromRecord(DELMUNDOMARCKEVIN_241639_Game record) => new() { Title = record.Title, Genre = record.Genre, Platform = record.Platform, Status = record.Status, Rating = record.Rating };
}
