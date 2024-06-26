using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.RegularExpressions;
using DotNetEnv;

using System.ComponentModel.DataAnnotations;

using PersonalFinances.DataContext;
using PersonalFinances.Services;
using PersonalFinances.Models;

namespace PersonalFinances.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
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

                     return Ok(new { user = res });
                  }
                  return NotFound(new { error = "No user found." });
               }
            }
            return Unauthorized(new { error = "Invalid Token!" });
         }
         return Unauthorized(new { error = "No token!" });
      }
      catch (ArgumentException ex)
      {
         return BadRequest(new { error = ex.Message });
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
            Env.Load();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("SECRET_KEY") ?? throw new ArgumentException("Error retrieving environment variables."));

            var hashedPassword = PasswordUtility.HashPassword(user.Password);
            var dbUser = await context.Users.FirstAsync(u => u.Email == user.Email && u.Password == hashedPassword) ?? throw new InvalidOperationException("Email/Password is wrong!");

            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            if (!Regex.IsMatch(user.Email, pattern))
            {
               throw new InvalidOperationException("Email invalid!");
            }
            /*
                        if (user.Password != null && dbUser.Password != null)
                           if (!PasswordUtility.VerifyPassword(user.Password, dbUser.Password))
                           {
                              throw new InvalidOperationException("Email/Password is wrong!");
                           }*/

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

/*
            var res = new
            {
               id = dbUser.UserID,
               name = dbUser.Name,
               lastName = dbUser.LastName,
               email = dbUser.Email,
               photo = dbUser.Photo
            };

            Response.Headers.Add("Authorization", tokenString);*/
            return Ok(new { res });
         }
      }
      catch (InvalidOperationException ex)
      {
         return BadRequest(new { error = ex.Message });
      }
      catch (ArgumentException ex)
      {
         return BadRequest(new { error = ex.Message });
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

            if (!Regex.IsMatch(user.Email, pattern))
            {
               throw new InvalidOperationException("Email invalid!");
            }

            var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser != null)
            {
               throw new InvalidOperationException("User is already registered!");
            }
            string hashedPassword = PasswordUtility.HashPassword(user.Password);

            var newUser = new UserModel(user.Name, user.LastName, user.Email, hashedPassword);

            context.Users.Add(newUser);
            await context.SaveChangesAsync();

            var res = new
            {
               id = newUser.UserID,
               name = newUser.Name,
               lastName = newUser.LastName,
               email = newUser.Email,
               photo = newUser.Photo
            };

            return Ok(new { user = res });
         }
      }
      catch (InvalidOperationException ex)
      {
         return BadRequest(new { error = ex.Message });
      }
      catch (ArgumentException ex)
      {
         return BadRequest(new { error = ex.Message });
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

                     return Ok(new { user = res });
                  }
                  return NotFound(new { error = "No User found." });
               }
            }
            return Unauthorized(new { error = "Invalid Token!" });
         }
         return Unauthorized(new { error = "No token!" });
      }
      catch (ArgumentException ex)
      {
         return BadRequest(new { error = ex.Message });
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
                  return NotFound(new { error = "No user found." });
               }
            }
            return Unauthorized(new { error = "Invalid Token!" });
         }
         return Unauthorized(new { error = "No token!" });
      }
      catch (ArgumentException ex)
      {
         return BadRequest(new { error = ex.Message });
      }
      catch (Exception ex)
      {
         Console.WriteLine($"[Server] Error: {ex.Message}");
         return StatusCode(500, new { error = ex.Message });
      }
   }
}