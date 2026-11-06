namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;

public sealed class DELMUNDOMARCKEVIN_241639_PayrollModel : DELMUNDOMARCKEVIN_241639_IPageFeedback
{
    public string? Salary { get; set; }
    public string? Error { get; set; }
    public DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services.PayrollBreakdown? Result { get; set; }
}
