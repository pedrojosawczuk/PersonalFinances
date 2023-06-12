using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
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

                  if (categories == null)
                  {
                     return Ok(new { message = "No Categories" });
                  }
                  return Ok(new { categories = categories });
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

                  if (incomeCategories == null)
                  {
                     return Ok(new { message = "No Income Categories" });
                  }
                  return Ok(new { categories = incomeCategories });
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

                  if (expensesCategories == null)
                  {
                     return Ok(new { message = "No Expense Categories" });
                  }
                  return Ok(new { categories = expensesCategories });
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