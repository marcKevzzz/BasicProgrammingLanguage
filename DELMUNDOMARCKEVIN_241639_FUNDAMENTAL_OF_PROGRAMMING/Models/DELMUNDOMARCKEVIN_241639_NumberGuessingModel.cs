namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;
public sealed class DELMUNDOMARCKEVIN_241639_NumberGuessingModel : DELMUNDOMARCKEVIN_241639_IPageFeedback
{
    public List<string> History { get; set; } = new();
    public int Attempts { get; set; }
    public bool IsWon { get; set; }
    public string? Message { get; set; }
    public string? Error { get; set; }
    public string? MessageType { get; set; }
}
