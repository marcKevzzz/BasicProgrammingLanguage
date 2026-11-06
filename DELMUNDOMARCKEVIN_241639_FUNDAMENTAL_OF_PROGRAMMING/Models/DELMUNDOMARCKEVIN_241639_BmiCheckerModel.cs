namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;

public sealed class DELMUNDOMARCKEVIN_241639_BmiCheckerModel : DELMUNDOMARCKEVIN_241639_IPageFeedback
{
    public double? Weight { get; set; }
    public double? Height { get; set; }
    public string? Error { get; set; }
    public double? Bmi { get; set; }
    public string? Category { get; set; }
    public string? BadgeClass { get; set; }
    public string? Advice { get; set; }
    public bool Success { get; set; }
}
