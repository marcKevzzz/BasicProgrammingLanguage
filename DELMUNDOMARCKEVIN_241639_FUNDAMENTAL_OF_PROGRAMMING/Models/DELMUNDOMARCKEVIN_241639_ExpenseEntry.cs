using System.Text.Json.Serialization;
namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;
public sealed record DELMUNDOMARCKEVIN_241639_ExpenseEntry(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("amount")] double Amount,
    [property: JsonPropertyName("date")] string Date);
