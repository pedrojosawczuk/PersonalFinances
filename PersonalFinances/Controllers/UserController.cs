using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using DotNetEnv;

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
      catch (Exception ex)
      {
         Console.WriteLine($"[Server] Error: {ex.Message}");
         return BadRequest(new { error = ex.Message });
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
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("SECRET_KEY") ?? throw new NullReferenceException());

            var dbUser = await context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (user != null && user.Password != null && dbUser != null && dbUser.Password != null && dbUser.Email != null && dbUser.UserID != null)
            {
               if (!PasswordUtility.VerifyPassword(user.Password, dbUser.Password))
                  return Unauthorized(new { error = "Email/Password is wrong!" });

               var tokenDescriptor = new SecurityTokenDescriptor
               {
                  Subject = new ClaimsIdentity(new[]
                  {
                     new Claim("id", dbUser.UserID.ToString() ?? throw new ArgumentNullException()),
                     new Claim("email", dbUser.Email),
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
                  id = dbUser.UserID,
                  name = dbUser.Name,
                  lastName = dbUser.LastName,
                  email = dbUser.Email,
                  photo = dbUser.Photo
               };

               Response.Headers.Add("Authorization", tokenString);
               return Ok(new { user = res });
            }
            return BadRequest(new { error = "Failed to retrieve the user!" });
         }
      }
      catch (Exception ex)
      {
         Console.WriteLine($"[Server] Error: {ex.Message}");
         return BadRequest(new { error = ex.Message });
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
         if (user != null && user.Password != null)
         {
            using (var context = new EFDataContext())
            {
               var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

               if (existingUser == null)
               {
                  string hashedPassword = PasswordUtility.HashPassword(user.Password);

                  var newUser = new UserModel
                  {
                     Name = user.Name,
                     LastName = user.LastName,
                     Email = user.Email,
                     Password = hashedPassword
                  };

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

                  return Ok(res);
               }
               return BadRequest(new { error = "User is already registered!" });
            }
         }
         return BadRequest(new { error = "Unable to extract user!" });
      }
      catch (Exception ex)
      {
         Console.WriteLine($"[Server] Error: {ex.Message}");
         return BadRequest(new { error = ex.Message });
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
                  return NotFound(new { error = "No user found." });
               }
            }
            return Unauthorized(new { error = "Invalid Token!" });
         }
         return Unauthorized(new { error = "No token!" });
      }
      catch (Exception ex)
      {
         Console.WriteLine($"[Server] Error: {ex.Message}");
         return BadRequest(new { error = ex.Message });
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
      catch (Exception ex)
      {
         Console.WriteLine($"[Server] Error: {ex.Message}");
         return BadRequest(new { error = ex.Message });
      }
   }
}