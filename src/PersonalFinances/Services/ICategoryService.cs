using PersonalFinances.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalFinances.Services;

public interface ICategoryService
{
   Task<List<CategoryModel>> GetAllCategories();
   Task<List<CategoryModel>> GetIncomeCategories();
   Task<List<CategoryModel>> GetExpensesCategories();
}