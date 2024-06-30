using PersonalFinances.Models;

namespace PersonalFinances.Services;

public interface ITransactionService
{
	List<TransactionModel> ListTransactions(string token);
	TransactionModel GetOneTransaction(string id, string token);
	List<TransactionModel> ListIncome(string token);
	List<TransactionModel> ListExpenses(string token);
	List<TransactionModel> ListByCategory(string id, string token);
	void AddTransaction(TransactionModel transaction);
	void UpdateTransaction(string id, TransactionModel transaction);
	void RemoveTransaction(string id);
}