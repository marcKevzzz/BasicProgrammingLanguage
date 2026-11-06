namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;

public sealed class DELMUNDOMARCKEVIN_241639_StudyTimeModel : DELMUNDOMARCKEVIN_241639_IPageFeedback
{
    public List<DELMUNDOMARCKEVIN_241639_StudyEntry>? Entries { get; set; }
    public string? Error { get; set; }
    public double TotalHours { get; set; }
    public double AverageHours { get; set; }
    public bool Success { get; set; }
}
