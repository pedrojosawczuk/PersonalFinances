using PersonalFinances.Models;

namespace PersonalFinances.Services;

public interface ICategoryService
{
   List<TransactionModel> ListAllCategories(string token);
   List<TransactionModel> ListAllIncomeCategories(string token);
   List<TransactionModel> ListAllExpensesCategories(string token);
}