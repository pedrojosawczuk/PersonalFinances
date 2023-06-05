using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PersonalFinances.DataContext;

using PersonalFinances.Models;

namespace PersonalFinances.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
   [HttpGet]
   public async Task<ActionResult<IEnumerable<object>>> ListTransactions()
   {
      using (var context = new EFDataContext())
      {
         var transactions = await context.Transactions.Select(e => new
         {
            description = e.Description,
            value = e.Value,
            date = e.Date,
            type = e.Type
         })
         .ToListAsync();
         if (transactions == null)
         {
            return Ok(new { message = "No transactions" });
         }

         return Ok(transactions);
         /*
                     var objs = new List<object>();
                     foreach (var e in transactions)
                     {
                         var obj = new
                         {
                             description = e.description,
                             value = e.value,
                             date = e.date,
                             type = e.type
                         };
                         objs.Add(obj);
                     }
                     return Ok(objs);*/
      }
   }

   [HttpPost]
   public async Task<ActionResult<TransactionModel>> AddTransaction(TransactionModel transaction)
   {
      using (var context = new EFDataContext())
      {
         context.Transactions.Add(transaction);
         await context.SaveChangesAsync();

         return Ok(transaction);
      }
   }

   [HttpPut("{id}")]
   public async Task<ActionResult<TransactionModel>> UpdateTransaction(int id, TransactionModel updatedTransaction)
   {
      using (var context = new EFDataContext())
      {
         var transaction = await context.Transactions.FindAsync(id);
         if (transaction == null)
         {
            return NotFound();
         }

         transaction.Description = updatedTransaction.Description;
         transaction.Value = updatedTransaction.Value;
         transaction.Date = updatedTransaction.Date;
         transaction.Type = updatedTransaction.Type;

         await context.SaveChangesAsync();

         return Ok(transaction);
      }
   }
   [HttpDelete("{id}")]
   public async Task<ActionResult<TransactionModel>> DeleteTransaction(int id)
   {
      using (var context = new EFDataContext())
      {
         var transaction = await context.Transactions.FindAsync(id);
         if (transaction == null)
         {
            return NotFound();
         }

         context.Transactions.Remove(transaction);
         await context.SaveChangesAsync();

         return Ok(transaction);
      }
   }

}