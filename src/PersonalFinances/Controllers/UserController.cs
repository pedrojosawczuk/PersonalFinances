using Microsoft.AspNetCore.Mvc;
using PersonalFinances.Services;
using PersonalFinances.Models;
using System.IdentityModel.Tokens.Jwt;

namespace PersonalFinances.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class UserController : ControllerBase
   {
      private readonly IUserService _userService;

      public UserController(IUserService userService)
      {
         _userService = userService;
      }

      [HttpGet("verify")]
      public async Task<ActionResult> Verify()
      {
         try
         {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token))
               return Unauthorized();

            var jwtToken = new JwtSecurityToken(token);
            var payload = jwtToken.Payload;

            if (payload.TryGetValue("id", out object? idObj) && long.TryParse(idObj.ToString(), out long id))
            {
               var dbUser = await _userService.GetUserById(id);

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

                  return Ok(res);
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
      public async Task<ActionResult> Login([FromBody] UserModel user)
      {
         try
         {
            if (!_userService.IsEmailValid(user.Email!))
               throw new InvalidOperationException("Email invalid!");

            var dbUser = await _userService.AuthenticateUser(user.Email!, user.Password!);
            if (dbUser == null)
               throw new InvalidOperationException("Email/Password is wrong!");

            var tokenString = _userService.GenerateJwtToken(dbUser);

            return Ok(tokenString);
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
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token))
               return Unauthorized();

            var jwtToken = new JwtSecurityToken(token);
            var payload = jwtToken.Payload;

            if (payload.TryGetValue("id", out object? idObj) && long.TryParse(idObj.ToString(), out long id))
            {
               var dbUser = await _userService.GetUserById(id);

               if (dbUser != null)
               {
                  dbUser.Name = updatedUser.Name;
                  dbUser.LastName = updatedUser.LastName;
                  dbUser.Email = updatedUser.Email;
                  dbUser.Password = updatedUser.Password;
                  dbUser.Photo = updatedUser.Photo;

                  await _userService.SaveChangesAsync();

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
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token))
               return Unauthorized();

            var jwtToken = new JwtSecurityToken(token);
            var payload = jwtToken.Payload;

            if (payload.TryGetValue("id", out object? idObj) && long.TryParse(idObj.ToString(), out long id))
            {
               var dbUser = await _userService.GetUserById(id);

               if (dbUser != null)
               {
                  _userService.DeleteUser(dbUser);
                  await _userService.SaveChangesAsync();
                  return Ok();
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
}