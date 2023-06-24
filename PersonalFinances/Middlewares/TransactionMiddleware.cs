using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

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
         if (IsValidToken(token, out var userId))
         {
            // Adicione o usuário ao contexto
            context.Items["UserId"] = userId;
            // Chamar o próximo middleware na pipeline
            await _next(context);
         }
         // Token inválido, retorne uma resposta de erro ou faça o redirecionamento adequado
         context.Response.StatusCode = StatusCodes.Status401Unauthorized;
         return;
      }
      // Token não fornecido, retorne uma resposta de erro ou faça o redirecionamento adequado
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