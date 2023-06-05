using PersonalFinances.Models;

namespace PersonalFinances.Services;

public interface IUserService
{
   UserModel LoginUser(string email, string password);
   void RegisterUser(UserModel user);
   void UpdateUser(string id, UserModel user);
   void RemoveUser(string id);
}