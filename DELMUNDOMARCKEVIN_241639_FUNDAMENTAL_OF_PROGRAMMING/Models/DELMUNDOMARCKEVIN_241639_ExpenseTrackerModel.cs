namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;
public sealed class DELMUNDOMARCKEVIN_241639_ExpenseTrackerModel : DELMUNDOMARCKEVIN_241639_IPageFeedback
{
    public string? Message { get; set; }
    public string? Error { get; set; }
    public List<DELMUNDOMARCKEVIN_241639_ExpenseEntry> Expenses { get; set; } = new();
    public double TotalExpenses { get; set; }
    public double AverageExpense { get; set; }
    public int ExpenseCount { get; set; }
}
