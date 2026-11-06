namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;

public sealed class DELMUNDOMARCKEVIN_241639_TravelFareModel : DELMUNDOMARCKEVIN_241639_IPageFeedback
{
    public decimal? FirstNumber { get; set; }
    public decimal? SecondNumber { get; set; }
    public string? Operation { get; set; }
    public string? Error { get; set; }
    public decimal? Result { get; set; }
    public bool Success { get; set; }
}
