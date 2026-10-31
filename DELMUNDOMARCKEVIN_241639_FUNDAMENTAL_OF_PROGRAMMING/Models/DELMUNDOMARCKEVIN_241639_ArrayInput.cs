using System.ComponentModel.DataAnnotations;

namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;

public class DELMUNDOMARCKEVIN_241639_ArrayInput
{
    public string Direction { get; set; } = "ascending";

    [Required, StringLength(3000)]
    public string Input { get; set; } = "";

    [StringLength(3000)]
    public string? SecondInput { get; set; }
}
