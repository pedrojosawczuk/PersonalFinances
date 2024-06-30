using PersonalFinances.Models;

namespace PersonalFinances.Services;

public interface ICategoryService
{
   Task<List<CategoryModel>> GetAllCategories();
   Task<List<CategoryModel>> GetIncomeCategories();
   Task<List<CategoryModel>> GetExpensesCategories();
}