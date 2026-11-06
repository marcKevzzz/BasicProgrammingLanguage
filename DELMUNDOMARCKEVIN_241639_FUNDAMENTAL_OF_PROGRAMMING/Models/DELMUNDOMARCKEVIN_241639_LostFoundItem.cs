using System.ComponentModel.DataAnnotations;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;
namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;
public sealed record DELMUNDOMARCKEVIN_241639_LostFoundItem(int Id, string Name, string Description, string Location, string Date, string Status) : DELMUNDOMARCKEVIN_241639_CrudRecord(Id);
