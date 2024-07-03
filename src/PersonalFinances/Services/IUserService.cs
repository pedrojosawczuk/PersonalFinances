using PersonalFinances.Models;

namespace PersonalFinances.Services;

public interface IUserService
{
	Task<UserModel?> AuthenticateUser(string email, string password);
	Task<UserModel?> GetUserById(long id);
	Task<UserModel?> GetUserByEmail(string email);
	Task RegisterUser(UserModel user);
	Task SaveChangesAsync();
	Task DeleteUser(UserModel user);
	string GenerateJwtToken(UserModel user);
	bool ValidateJwtToken(string token);
	bool IsEmailValid(string email);
}