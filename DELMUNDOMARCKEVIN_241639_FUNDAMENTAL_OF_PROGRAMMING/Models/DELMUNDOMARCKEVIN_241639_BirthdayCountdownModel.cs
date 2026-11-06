namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;

public sealed class DELMUNDOMARCKEVIN_241639_BirthdayCountdownModel : DELMUNDOMARCKEVIN_241639_IPageFeedback
{
    public string? Today { get; set; }
    public string? Birthday { get; set; }
    public string? Error { get; set; }
    public string? NextBirthday { get; set; }
    public int? DaysRemaining { get; set; }
    public int? Weeks { get; set; }
    public int? RemainingDays { get; set; }
    public bool Success { get; set; }
}
