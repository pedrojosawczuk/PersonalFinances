using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using PersonalFinances.DataContext;

using PersonalFinances.Models;

namespace PersonalFinances.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
   [HttpGet]
   public async Task<ActionResult<IEnumerable<object>>> ListAllCategories()
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
                  List<CategoryModel> allCategories = new List<CategoryModel>();
                  var categories = await context.Categories.ToListAsync();

                  if (categories.Count == 0)
                  {
                     return NoContent();
                  }
                  return Ok(categories);
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

   [HttpGet("income")]
   public async Task<ActionResult<IEnumerable<object>>> ListAllIncomeCategories()
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
                  List<CategoryModel> categories = new List<CategoryModel>();
                  var incomeCategories = await context.Categories
                     .Where(c => c.Type == "I")
                     .Select(c => new
                     {
                        id = c.CategoryID,
                        name = c.Name,
                        description = c.Description,
                        type = c.Type
                     })
                     .ToListAsync();

                  if (incomeCategories.Count == 0)
                  {
                     return NoContent();
                  }
                  return Ok(incomeCategories);
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
   [HttpGet("expenses")]
   public async Task<ActionResult<IEnumerable<object>>> ListAllExpensesCategories()
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
                  List<CategoryModel> categories = new List<CategoryModel>();
                  var expensesCategories = await context.Categories
                     .Where(c => c.Type == "E")
                     .Select(c => new
                     {
                        id = c.CategoryID,
                        name = c.Name,
                        description = c.Description,
                        type = c.Type
                     })
                     .ToListAsync();

                  if (expensesCategories.Count == 0)
                  {
                     return NoContent();
                  }
                  return Ok(expensesCategories);
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