using System.Security.Cryptography;
using System.Text;

namespace PersonalFinances.Services;

public class PasswordUtility
{
	public static string HashPassword(string password)
	{
		if (string.IsNullOrEmpty(password))
		{
			throw new ArgumentException("Password cannot be null.");
		}

		using (SHA512 sha512 = SHA512.Create())
		{
			byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
			byte[] hashBytes = sha512.ComputeHash(passwordBytes);

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < hashBytes.Length; i++)
			{
				sb.Append(hashBytes[i].ToString("x2"));
			}

			return sb.ToString();
		}
	}

	public static bool VerifyPassword(string password, string hashedPassword)
	{
		if (string.IsNullOrEmpty(password))
		{
			throw new ArgumentException("Password cannot be null.");
		}
		else if (string.IsNullOrEmpty(hashedPassword))
		{
			Console.WriteLine("DB can't retrieve value for password.");
			throw new Exception("Internal Server Error");
		}

		string hashedInput = HashPassword(password);
		return string.Equals(hashedInput, hashedPassword, StringComparison.OrdinalIgnoreCase);
	}
}