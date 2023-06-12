using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using PersonalFinances.Models;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using PersonalFinances.Middleware;

namespace PersonalFinances;

public class Program
{
   public static void Main(string[] args)
   {
      var builder = WebApplication.CreateBuilder(args);

      builder.Services.AddControllers();

      builder.Services.AddEndpointsApiExplorer();

      builder.Services.AddCors(options =>
      {
         options.AddDefaultPolicy(builder =>
         {
            builder.WithOrigins("http://127.0.0.1:5500")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
         });
      });

      var app = builder.Build();

      app.UseCors();
      
      //app.UseHttpsRedirection();

      app.UseAuthorization();

      app.MapControllers();

      app.Run();
   }
}