namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;

public sealed class DELMUNDOMARCKEVIN_241639_ScholarshipQualificationModel : DELMUNDOMARCKEVIN_241639_IPageFeedback
{
    public double? AverageGrade { get; set; }
    public double? HouseholdIncome { get; set; }
    public bool HasFailingGrades { get; set; }
    public string? Error { get; set; }
    public bool IsQualified { get; set; }
    public string? Explanation { get; set; }
    public List<DELMUNDOMARCKEVIN_241639_ScholarshipCriterion>? Criteria { get; set; }
    public bool Success { get; set; }
}
