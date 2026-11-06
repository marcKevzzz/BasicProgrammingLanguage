namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;

public sealed class DELMUNDOMARCKEVIN_241639_ElectricityBillModel : DELMUNDOMARCKEVIN_241639_IPageFeedback
{
    public double? Consumption { get; set; }
    public double? Rate { get; set; }
    public string? Error { get; set; }
    public double? TotalBill { get; set; }
    public bool Success { get; set; }
}
