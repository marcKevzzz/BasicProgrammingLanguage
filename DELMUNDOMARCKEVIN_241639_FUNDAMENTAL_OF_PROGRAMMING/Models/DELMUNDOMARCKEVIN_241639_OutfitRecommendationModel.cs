namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;

public sealed class DELMUNDOMARCKEVIN_241639_OutfitRecommendationModel : DELMUNDOMARCKEVIN_241639_IPageFeedback
{
    public double? Temperature { get; set; }
    public string? WeatherCondition { get; set; }
    public string? Error { get; set; }
    public string? Category { get; set; }
    public string? Top { get; set; }
    public string? Bottom { get; set; }
    public string? Footwear { get; set; }
    public string? Accessories { get; set; }
    public bool Success { get; set; }
}
