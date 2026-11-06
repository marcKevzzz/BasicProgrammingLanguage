using System.ComponentModel.DataAnnotations;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;
namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;
public sealed record DELMUNDOMARCKEVIN_241639_WishlistItem(int Id, string Name, string Category, double Price, string Priority, bool Purchased) : DELMUNDOMARCKEVIN_241639_CrudRecord(Id);
