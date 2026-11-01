using System.Globalization;
namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;

public static class CrudValidation
{
    public static string? Book(string? title, string? author, int? year)
    {
        if (string.IsNullOrWhiteSpace(title)) return "Book title is required.";
        if (string.IsNullOrWhiteSpace(author)) return "Author name is required.";
        if (!year.HasValue || year < 1400 || year > DateTime.Now.Year + 1)
            return $"Publication year must be between 1400 and {DateTime.Now.Year + 1}.";
        return null;
    }
    public static string? Wishlist(string? name, double? price, string? priority)
    {
        if (string.IsNullOrWhiteSpace(name)) return "Item name is required.";
        if (!price.HasValue || !double.IsFinite(price.Value) || price < 0) return "Estimated price must be a finite, non-negative amount.";
        if (!string.IsNullOrWhiteSpace(priority) && priority.Trim() is not ("High" or "Medium" or "Low")) return "Choose a valid priority.";
        return null;
    }
    public static string? Game(string? title, int? rating, string? status, string? platform)
    {
        if (string.IsNullOrWhiteSpace(title)) return "Game title is required.";
        if (rating is null or < 1 or > 5) return "Rating must be between 1 and 5 stars.";
        if (!string.IsNullOrWhiteSpace(status) && status.Trim() is not ("Backlog" or "In Progress" or "Completed" or "Abandoned")) return "Choose a valid game status.";
        if (!string.IsNullOrWhiteSpace(platform) && platform.Trim() is not ("PC" or "PlayStation 5" or "Nintendo Switch" or "Xbox Series X" or "Mobile / iOS")) return "Choose a valid platform.";
        return null;
    }
    public static string? Attendance(string? name, string? studentNumber, string? date, string? status)
    {
        if (string.IsNullOrWhiteSpace(name)) return "Student name is required.";
        if (string.IsNullOrWhiteSpace(studentNumber)) return "Student number is required.";
        if (!ValidDate(date)) return "Enter a valid date in YYYY-MM-DD format.";
        if (!string.IsNullOrWhiteSpace(status) && status.Trim() is not ("Present" or "Late" or "Absent")) return "Choose a valid attendance status.";
        return null;
    }
    public static string? LostFound(string? name, string? location, string? date, string? status)
    {
        if (string.IsNullOrWhiteSpace(name)) return "Item name is required.";
        if (string.IsNullOrWhiteSpace(location)) return "Location is required.";
        if (!ValidDate(date)) return "Enter a valid date in YYYY-MM-DD format.";
        if (!string.IsNullOrWhiteSpace(status) && status.Trim() is not ("Found" or "Lost" or "Claimed")) return "Choose a valid report status.";
        return null;
    }
    private static bool ValidDate(string? date) => string.IsNullOrWhiteSpace(date) ||
        DateOnly.TryParseExact(date.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
}
