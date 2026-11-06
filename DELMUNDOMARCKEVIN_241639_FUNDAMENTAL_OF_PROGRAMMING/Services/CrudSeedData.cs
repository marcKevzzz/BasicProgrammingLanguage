using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;
namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;
public static class CrudSeedData
{
    public static IReadOnlyList<DELMUNDOMARCKEVIN_241639_WishlistItem> WishlistItems { get; } = Array.AsReadOnly(new DELMUNDOMARCKEVIN_241639_WishlistItem[]
    {
        new(1, "Mechanical Keyboard with Linear Switches", "Electronics", 4500.00, "High", false),
        new(2, "Ergonomic Mesh Office Chair", "Furniture", 9200.00, "Medium", true),
        new(3, "27-inch 4K USB-C Monitor", "Electronics", 18500.00, "High", false),
        new(4, "Noise Cancelling Headphones", "Audio", 12000.00, "Low", false)
    });
    public static IReadOnlyList<DELMUNDOMARCKEVIN_241639_Game> Games { get; } = Array.AsReadOnly(new DELMUNDOMARCKEVIN_241639_Game[]
    {
        new(1, "The Legend of Zelda: Tears of the Kingdom", "Action-Adventure", "Nintendo Switch", "Completed", 5),
        new(2, "Elden Ring", "Action RPG", "PC", "In Progress", 5),
        new(3, "Hollow Knight", "Metroidvania", "PC", "Completed", 5),
        new(4, "Cyberpunk 2077: Phantom Liberty", "RPG", "PlayStation 5", "Backlog", 4),
        new(5, "Stardew Valley", "Farming Simulation", "PC", "In Progress", 5)
    });
    public static IReadOnlyList<DELMUNDOMARCKEVIN_241639_AttendanceRecord> AttendanceRecords { get; } = Array.AsReadOnly(new DELMUNDOMARCKEVIN_241639_AttendanceRecord[]
    {
        new(1, "John Carlo Santos", "2024-01042", "BS Computer Science", "CS-2A", "2026-10-18", "Present"),
        new(2, "Maria Clara Dela Cruz", "2024-01089", "BS Information Technology", "IT-2B", "2026-10-18", "Present"),
        new(3, "Liam Gabriel Mendoza", "2024-01123", "BS Computer Science", "CS-2A", "2026-10-18", "Late"),
        new(4, "Sophia Nicole Reyes", "2024-01190", "BS Information Systems", "IS-2A", "2026-10-18", "Absent"),
        new(5, "Rafael Ethan Garcia", "2024-01205", "BS Computer Science", "CS-2A", "2026-10-18", "Present")
    });
    public static IReadOnlyList<DELMUNDOMARCKEVIN_241639_LostFoundItem> LostFoundItems { get; } = Array.AsReadOnly(new DELMUNDOMARCKEVIN_241639_LostFoundItem[]
    {
        new(1, "Wireless Earbuds Case", "White charging case found on 3rd floor library desk", "University Library - 3rd Floor", "2026-10-14", "Found"),
        new(2, "Black Leather Wallet", "Contains student ID card and transport card", "Student Cafeteria", "2026-10-15", "Lost"),
        new(3, "Silver Hydro Flask Tumbler", "32oz tumbler with developer stickers", "Engineering Lab 204", "2026-10-17", "Found"),
        new(4, "Scientific Calculator (Casio fx-991EX)", "Left after Advanced Mathematics examination", "Room 402 Main Building", "2026-10-18", "Claimed")
    });
}
