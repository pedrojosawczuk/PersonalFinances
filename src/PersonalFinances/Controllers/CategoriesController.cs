using Microsoft.AspNetCore.Mvc;
using PersonalFinances.Models;
using PersonalFinances.Services;

namespace PersonalFinances.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
   private readonly ICategoryService _categoryService;

   public CategoriesController(ICategoryService categoryService)
   {
      _categoryService = categoryService;
   }

   [HttpGet]
   public async Task<ActionResult<IEnumerable<CategoryModel>>> ListAllCategories()
   {
      try
      {
         var categories = await _categoryService.GetAllCategories();

         if (categories.Count == 0)
         {
            return NoContent();
         }

         return Ok(categories);
      }
      catch (Exception ex)
      {
         Console.WriteLine($"[Server] Error: {ex.Message}");
         return StatusCode(500, new { error = ex.Message });
      }
   }

   [HttpGet("income")]
   public async Task<ActionResult<IEnumerable<CategoryModel>>> ListAllIncomeCategories()
   {
      try
      {
         var incomeCategories = await _categoryService.GetIncomeCategories();

         if (incomeCategories.Count == 0)
         {
            return NoContent();
         }

         return Ok(incomeCategories);
      }
      catch (Exception ex)
      {
         Console.WriteLine($"[Server] Error: {ex.Message}");
         return StatusCode(500, new { error = ex.Message });
      }
   }

   [HttpGet("expenses")]
   public async Task<ActionResult<IEnumerable<CategoryModel>>> ListAllExpensesCategories()
   {
      try
      {
         var expensesCategories = await _categoryService.GetExpensesCategories();

         if (expensesCategories.Count == 0)
         {
            return NoContent();
         }

         return Ok(expensesCategories);
      }
      catch (Exception ex)
      {
         Console.WriteLine($"[Server] Error: {ex.Message}");
         return StatusCode(500, new { error = ex.Message });
      }
   }
}