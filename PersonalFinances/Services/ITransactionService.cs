using PersonalFinances.Models;

namespace PersonalFinances.Services;

public interface ITransactionService
{
   List<TransactionModel> ListTransactions(string token);
   void AddTransaction(TransactionModel transaction);
   void UpdateTransaction(string id, TransactionModel transaction);
   void RemoveTransaction(string id);
}