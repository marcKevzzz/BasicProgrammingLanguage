using System.ComponentModel.DataAnnotations;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;

namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;

public sealed class DELMUNDOMARCKEVIN_241639_BookInput : IValidatableObject
{
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Genre { get; set; }
    public int? Year { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var error = CrudValidation.DELMUNDOMARCKEVIN_241639_Book(Title, Author, Year);
        if (error != null) yield return new ValidationResult(error);
    }
}
