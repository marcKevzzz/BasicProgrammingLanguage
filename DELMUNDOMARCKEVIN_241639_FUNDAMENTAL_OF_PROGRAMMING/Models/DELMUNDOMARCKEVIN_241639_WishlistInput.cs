using System.ComponentModel.DataAnnotations;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;
namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;
public sealed class DELMUNDOMARCKEVIN_241639_WishlistInput : IValidatableObject
{
    public string? Name { get; set; } = "";
    public string? Category { get; set; } = "General";
    public double? Price { get; set; }
    public string? Priority { get; set; } = "Medium";
    public bool Purchased { get; set; } = false;
    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    { var error = CrudValidation.Wishlist(Name, Price, Priority); if (error != null) yield return new ValidationResult(error); }
    public DELMUNDOMARCKEVIN_241639_WishlistItem ToRecord(int id = 0) => new(id, string.IsNullOrWhiteSpace(Name) ? "" : Name.Trim(), string.IsNullOrWhiteSpace(Category) ? "General" : Category.Trim(), Price!.Value, string.IsNullOrWhiteSpace(Priority) ? "Medium" : Priority.Trim(), Purchased);
    public static DELMUNDOMARCKEVIN_241639_WishlistInput FromRecord(DELMUNDOMARCKEVIN_241639_WishlistItem record) => new() { Name = record.Name, Category = record.Category, Price = record.Price, Priority = record.Priority, Purchased = record.Purchased };
}
