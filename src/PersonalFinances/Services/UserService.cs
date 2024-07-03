using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PersonalFinances.DataContext;
using PersonalFinances.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace PersonalFinances.Services
{
	public class UserService : IUserService
	{
		private readonly EFDataContext _context;
		private readonly string _key = "cafecafecafecafecafecafecafecafecafecafecafecafecafecafecafecafecafecafecafecafecafecafecafecafe";

		public UserService(EFDataContext context)
		{
			_context = context;
		}

		public async Task<UserModel?> GetUserById(long id)
		{
			return await _context.Users.FirstOrDefaultAsync(u => u.UserID == id);
		}

		public async Task<UserModel?> GetUserByEmail(string email)
		{
			return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
		}

		public async Task<UserModel?> AuthenticateUser(string email, string password)
		{
			var hashedPassword = PasswordUtility.HashPassword(password);
			return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == hashedPassword);
		}

		public async Task RegisterUser(UserModel user)
		{
			user.Password = PasswordUtility.HashPassword(user.Password!);
			_context.Users.Add(user);
			await _context.SaveChangesAsync();
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}

		public async Task DeleteUser(UserModel user)
		{
			_context.Users.Remove(user);
			await _context.SaveChangesAsync();
		}

		public string GenerateJwtToken(UserModel user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_key);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				 {
						  new Claim("id", user.UserID.ToString() ?? throw new ArgumentException("Null id")),
						  new Claim("email", user.Email ?? throw new ArgumentException("Null Email")),
					 }),
				Expires = DateTime.UtcNow.AddDays(30),
				SigningCredentials = new SigningCredentials(
					  new SymmetricSecurityKey(key),
					  SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
		public bool ValidateJwtToken(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_key);

			try
			{
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false,
					// Define um tempo máximo de tolerância para tokens expirados
					ClockSkew = TimeSpan.Zero
				}, out SecurityToken validatedToken);

				return true;
			}
			catch
			{
				return false;
			}
		}

		public bool IsEmailValid(string email)
		{
			string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
			return Regex.IsMatch(email, pattern);
		}
	}
}
