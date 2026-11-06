using System.ComponentModel.DataAnnotations;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;
namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;
public sealed record DELMUNDOMARCKEVIN_241639_Game(int Id, string Title, string Genre, string Platform, string Status, int Rating) : DELMUNDOMARCKEVIN_241639_CrudRecord(Id);
