using System.ComponentModel.DataAnnotations;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;
namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;
public sealed record DELMUNDOMARCKEVIN_241639_AttendanceRecord(int Id, string Name, string StudentNumber, string Course, string Section, string Date, string Status) : DELMUNDOMARCKEVIN_241639_CrudRecord(Id);
