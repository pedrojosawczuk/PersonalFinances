using PersonalFinances.Models;

namespace PersonalFinances.Services;

public interface IUserService
{
   Task<UserModel?> GetUserById(long id);
   Task<UserModel?> AuthenticateUser(string email, string password);
   Task SaveChangesAsync();
   Task DeleteUser(UserModel user);
   string GenerateJwtToken(UserModel user);
   bool IsEmailValid(string email);
}