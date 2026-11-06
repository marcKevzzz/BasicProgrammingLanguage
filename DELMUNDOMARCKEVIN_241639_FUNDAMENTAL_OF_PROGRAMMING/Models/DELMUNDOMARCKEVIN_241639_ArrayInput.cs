using System.ComponentModel.DataAnnotations;

namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;

public class DELMUNDOMARCKEVIN_241639_ArrayInput
{
    public string? Error { get; set; }
    public string? Result { get; set; }
    public bool GradePassed { get; set; }

    public string Direction { get; set; } = "ascending";

    [Required, StringLength(3000)]
    public string Input { get; set; } = "";

    [StringLength(3000)]
    public string? SecondInput { get; set; }
}
