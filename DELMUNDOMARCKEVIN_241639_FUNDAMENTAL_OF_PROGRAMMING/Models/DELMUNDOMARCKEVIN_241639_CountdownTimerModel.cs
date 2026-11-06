namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;
public sealed class DELMUNDOMARCKEVIN_241639_CountdownTimerModel : DELMUNDOMARCKEVIN_241639_IPageFeedback
{
    public int Duration { get; set; }
    public string? Error { get; set; }
    public bool Success { get; set; }
    public List<DELMUNDOMARCKEVIN_241639_CountdownTick> Sequence { get; set; } = new();
}
