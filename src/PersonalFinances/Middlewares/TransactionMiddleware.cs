using System.IdentityModel.Tokens.Jwt;

namespace PersonalFinances.Middleware;

public class TokenValidationMiddleware
{
   private readonly RequestDelegate _next;

   public TokenValidationMiddleware(RequestDelegate next)
   {
      _next = next;
   }

   public async Task InvokeAsync(HttpContext context)
   {
      if (context.Request.Headers.TryGetValue("Authorization", out var token))
      {
         if (IsValidToken(token!, out var userId))
         {
            context.Items["UserId"] = userId;
            await _next(context);
         }
         context.Response.StatusCode = StatusCodes.Status401Unauthorized;
         return;
      }
      context.Response.StatusCode = StatusCodes.Status401Unauthorized;
      return;
   }

   private bool IsValidToken(string token, out long userId)
   {
      var jwtToken = new JwtSecurityToken(token);
      var payload = jwtToken.Payload;

      userId = 0;

      if (payload.TryGetValue("id", out var idObj) && long.TryParse(idObj.ToString(), out userId))
      {
         return true;
      }
      return false;
   }
}