using System;
using System.Security.Cryptography;
using System.Text;

namespace PersonalFinances.Services;

public class PasswordUtility
{
   public static string HashPassword(string password)
   {
      using (SHA512 sha512 = SHA512.Create())
      {
         // Convert the password string to byte array
         byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

         // Compute the hash value of the password bytes
         byte[] hashBytes = sha512.ComputeHash(passwordBytes);

         // Convert the hashed bytes to a hexadecimal string
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
      string hashedInput = HashPassword(password);
      return string.Equals(hashedInput, hashedPassword, StringComparison.OrdinalIgnoreCase);
   }
}