namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;

public sealed class DELMUNDOMARCKEVIN_241639_PasswordStrengthModel : DELMUNDOMARCKEVIN_241639_IPageFeedback
{
    public string? Error { get; set; }
    public string? Rating { get; set; }
    public string? BadgeClass { get; set; }
    public int Score { get; set; }
    public int Length { get; set; }
    public bool HasUpper { get; set; }
    public bool HasLower { get; set; }
    public bool HasDigit { get; set; }
    public bool HasSpecial { get; set; }
    public int ProgressPercent { get; set; }
    public List<string>? Suggestions { get; set; }
    public bool Success { get; set; }
}
