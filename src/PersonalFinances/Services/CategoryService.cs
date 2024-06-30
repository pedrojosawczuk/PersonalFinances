using Microsoft.EntityFrameworkCore;
using PersonalFinances.DataContext;
using PersonalFinances.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinances.Services;

public class CategoryService : ICategoryService
{
   private readonly EFDataContext _context;

   public CategoryService(EFDataContext context)
   {
      _context = context;
   }

   public async Task<List<CategoryModel>> GetAllCategories()
   {
      return await _context.Categories.ToListAsync();
   }

   public async Task<List<CategoryModel>> GetIncomeCategories()
   {
      return await _context.Categories
          .Where(c => c.Type == "I")
          .ToListAsync();
   }

   public async Task<List<CategoryModel>> GetExpensesCategories()
   {
      return await _context.Categories
          .Where(c => c.Type == "E")
          .ToListAsync();
   }
}