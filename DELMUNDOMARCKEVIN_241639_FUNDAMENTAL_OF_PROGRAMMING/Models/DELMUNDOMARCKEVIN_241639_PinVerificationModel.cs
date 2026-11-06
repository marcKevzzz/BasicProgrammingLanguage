namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;
public sealed class DELMUNDOMARCKEVIN_241639_PinVerificationModel : DELMUNDOMARCKEVIN_241639_IPageFeedback
{
    public int RemainingAttempts { get; set; }
    public bool IsLocked { get; set; }
    public string? DemoPin { get; set; }
    public string? Message { get; set; }
    public string? Error { get; set; }
    public List<string> History { get; set; } = new();
    public bool Success { get; set; }
}
