using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.RegularExpressions;

using PersonalFinances.DataContext;
using PersonalFinances.Services;
using PersonalFinances.Models;

namespace PersonalFinances.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
   // private readonly IConfiguration? _configuration;
   [HttpGet("verify")]
   public async Task<IActionResult> Verify()
   {
      try
      {
         if (HttpContext.Request.Headers["Authorization"] != String.Empty)
         {
            var token = HttpContext.Request.Headers["Authorization"];

            var jwtToken = new JwtSecurityToken(token);
            var payload = jwtToken.Payload;

            if (payload.TryGetValue("id", out object? idObj) && long.TryParse(idObj.ToString(), out long id))
            {
               using (var context = new EFDataContext())
               {
                  var dbUser = await context.Users.FirstOrDefaultAsync(u => u.UserID == id);

                  if (dbUser != null)
                  {
                     var res = new
                     {
                        id = dbUser.UserID,
                        name = dbUser.Name,
                        lastName = dbUser.LastName,
                        email = dbUser.Email,
                        photo = dbUser.Photo
                     };

                     return Ok();
                  }
                  return Unauthorized();
               }
            }
            return Unauthorized();
         }
         return Unauthorized();
      }
      catch (Exception ex)
      {
         Console.WriteLine($"[Server] Error: {ex.Message}");
         return StatusCode(500, new { error = ex.Message });
      }
   }

   [HttpPost("login")]
   public async Task<IActionResult> Login([FromBody] UserModel user)
   {
      try
      {
         using (var context = new EFDataContext())
         {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("cafecafecafecafecafecafecafecafecafecafecafecafecafecafecafecafecafecafecafecafecafecafecafecafe");

            var hashedPassword = PasswordUtility.HashPassword(user.Password!);
            var dbUser = await context.Users.FirstAsync(u => u.Email == user.Email && u.Password == hashedPassword) ?? throw new InvalidOperationException("Email/Password is wrong!");

            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            if (!Regex.IsMatch(user.Email!, pattern))
            {
               throw new InvalidOperationException("Email invalid!");
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
               Subject = new ClaimsIdentity(new[]
               {
                     new Claim("id", dbUser.UserID.ToString() ?? throw new ArgumentException("Null id")),
                     new Claim("email", dbUser.Email ?? throw new ArgumentException("Null Email")),
                  }),
               Expires = DateTime.UtcNow.AddDays(30),
               SigningCredentials = new SigningCredentials(
                  new SymmetricSecurityKey(key),
                  SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var res = new
            {
               token = tokenString
            };

            return Ok(res);
         }
      }
      catch (Exception ex)
      {
         Console.WriteLine($"[Server] Error: {ex.Message}");
         return StatusCode(500, new { error = ex.Message });
      }
   }

   [HttpPost("register")]
   public async Task<ActionResult<UserModel>> RegisterUser()
   {
      try
      {
         var stream = HttpContext.Request.Body;
         var buffer = new byte[Convert.ToInt32(HttpContext.Request.ContentLength)];
         await stream.ReadAsync(buffer, 0, buffer.Length);
         var json = Encoding.UTF8.GetString(buffer);
         var user = JsonConvert.DeserializeObject<UserModel>(json);

         using (var context = new EFDataContext())
         {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            if (!Regex.IsMatch(user!.Email!, pattern))
            {
               throw new InvalidOperationException("Email invalid!");
            }

            var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser != null)
            {
               throw new InvalidOperationException("User is already registered!");
            }
            string hashedPassword = PasswordUtility.HashPassword(user.Password!);

            var newUser = new UserModel
            {
               Name = user.Name,
               LastName = user.LastName,
               Email = user.Email,
               Password = hashedPassword
            };

            await context.Users.AddAsync(newUser);
            await context.SaveChangesAsync();

            var res = new
            {
               id = newUser.UserID,
               name = newUser.Name,
               lastName = newUser.LastName,
               email = newUser.Email,
               photo = newUser.Photo
            };

            return Ok(res);
         }
      }
      catch (Exception ex)
      {
         Console.WriteLine($"[Server] Error: {ex.Message}");
         return StatusCode(500, new { error = ex.Message });
      }
   }

   [HttpPatch()]
   public async Task<ActionResult<UserModel>> UpdateUser([FromBody] UserModel updatedUser)
   {
      try
      {
         if (HttpContext.Request.Headers["Authorization"] != String.Empty)
         {
            var token = HttpContext.Request.Headers["Authorization"];

            var jwtToken = new JwtSecurityToken(token);
            var payload = jwtToken.Payload;

            if (payload.TryGetValue("id", out object? idObj) && long.TryParse(idObj.ToString(), out long id))
            {
               using (var context = new EFDataContext())
               {
                  var dbUser = await context.Users.FirstOrDefaultAsync(u => u.UserID == id);

                  if (dbUser != null)
                  {
                     dbUser.Name = updatedUser.Name;
                     dbUser.LastName = updatedUser.LastName;
                     dbUser.Email = updatedUser.Email;
                     dbUser.Password = updatedUser.Password;
                     dbUser.Photo = updatedUser.Photo;

                     await context.SaveChangesAsync();

                     var res = new
                     {
                        id = dbUser.UserID,
                        name = dbUser.Name,
                        lastName = dbUser.LastName,
                        email = dbUser.Email,
                        photo = dbUser.Photo
                     };

                     return Ok(res);
                  }
                  return Unauthorized();
               }
            }
            return Unauthorized();
         }
         return Unauthorized();
      }
      catch (Exception ex)
      {
         Console.WriteLine($"[Server] Error: {ex.Message}");
         return StatusCode(500, new { error = ex.Message });
      }
   }

   [HttpDelete]
   public async Task<ActionResult<UserModel>> DeleteUser()
   {
      try
      {
         if (HttpContext.Request.Headers["Authorization"] != String.Empty)
         {
            var token = HttpContext.Request.Headers["Authorization"];

            var jwtToken = new JwtSecurityToken(token);
            var payload = jwtToken.Payload;

            if (payload.TryGetValue("id", out object? idObj) && long.TryParse(idObj.ToString(), out long id))
            {
               using (var context = new EFDataContext())
               {
                  var dbUser = await context.Users.FirstOrDefaultAsync(u => u.UserID == id);

                  if (dbUser != null)
                  {
                     context.Users.Remove(dbUser);
                     await context.SaveChangesAsync();
                     return Ok();
                  }
                  return Unauthorized();
               }
            }
            return Unauthorized();
         }
         return Unauthorized();
      }
      catch (Exception ex)
      {
         Console.WriteLine($"[Server] Error: {ex.Message}");
         return StatusCode(500, new { error = ex.Message });
      }
   }
}