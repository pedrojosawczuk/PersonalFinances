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
public class TransactionController : ControllerBase
{
   [HttpGet]
   public async Task<ActionResult<IEnumerable<object>>> ListTransactions()
   {
      try
      {
         using (var context = new EFDataContext())
         {
            var transactions = await context.Transactions.Select(e => new
            {
               description = e.Description,
               value = e.Value,
               date = e.Date,
               type = e.Type
            }).ToListAsync();

            if (transactions == null)
            {
               return Ok(new { message = "No transactions" });
            }

            return Ok(transactions);
         }
      }
      catch (Exception ex)
      {
         Console.WriteLine($"[Server] Error: {ex.Message}");
         return BadRequest(new { error = ex.Message });
      }
   }

   [HttpPost]
   public async Task<ActionResult<TransactionModel>> AddTransaction(TransactionModel transaction)
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
               if (transaction != null)
               {
                  if (transaction.Description != null && transaction.Value != 0 && transaction.Type != null && transaction.Date != null && transaction.FkCategory != 0)
                  {
                     using (var context = new EFDataContext())
                     {
                        var dbUser = await context.Users.FirstOrDefaultAsync(u => u.UserID == id);
                        if (dbUser != null)
                        {
                           var newTransaction = new TransactionModel
                           {
                              Description = transaction.Description,
                              Value = transaction.Value,
                              Type = transaction.Type,
                              Date = transaction.Date,
                              FkUser = id,
                              FkCategory = transaction.FkCategory
                           };

                           await context.Transactions.AddAsync(newTransaction);
                           await context.SaveChangesAsync();

                           var res = new
                           {
                              id = newTransaction.TransactionID,
                              description = newTransaction.Description,
                              value = newTransaction.Value,
                              type = newTransaction.Type,
                              date = newTransaction.Date,
                              createdAt = newTransaction.CreatedAt,
                              updatedAt = newTransaction.UpdatedAt,
                              fkUser = newTransaction.FkUser,
                              fkCategory = newTransaction.FkCategory
                           };

                           return Ok(res);
                        }
                        return BadRequest(new { error = "Failed to retrieve the user!" });
                     }
                  }
               }
               return NotFound(new { error = "No Data Recive." });
            }
            return Unauthorized(new { error = "Invalid Token!" });
         }
         return Unauthorized(new { error = "No token!" });
      }
      catch (Exception ex)
      {
         Console.WriteLine($"[Server] Error: {ex.Message}");
         return BadRequest(new { error = ex.Message });
      }
   }

   [HttpPatch]
   public async Task<ActionResult<TransactionModel>> UpdateTransaction(TransactionModel updatedTransaction)
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
               if (updatedTransaction != null)
               {
                  if (updatedTransaction.TransactionID != 0 && updatedTransaction.Description != null && updatedTransaction.Value != 0 && updatedTransaction.Type != null && updatedTransaction.Date != null && updatedTransaction.FkCategory != 0)
                  {
                     using (var context = new EFDataContext())
                     {
                        var dbTransaction = await context.Transactions.FindAsync(updatedTransaction.TransactionID);

                        if (dbTransaction != null && dbTransaction.FkUser == id)
                        {
                           dbTransaction.Description = updatedTransaction.Description;
                           dbTransaction.Value = updatedTransaction.Value;
                           dbTransaction.Type = updatedTransaction.Type;
                           dbTransaction.Date = updatedTransaction.Date;
                           dbTransaction.FkCategory = updatedTransaction.FkCategory;

                           await context.SaveChangesAsync();

                           return Ok(dbTransaction);
                        }
                        return BadRequest(new { error = "Failed!" });
                     }
                  }
               }
               return NotFound(new { error = "No Data Recive." });
            }
            return Unauthorized(new { error = "Invalid Token!" });
         }
         return BadRequest(new { error = "No token!" });
      }
      catch (Exception ex)
      {
         Console.WriteLine($"[Server] Error: {ex.Message}");
         return BadRequest(new { error = ex.Message });
      }
   }

   [HttpDelete]
   public async Task<ActionResult<TransactionModel>> DeleteTransaction(int id)
   {
      try
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
      catch (Exception ex)
      {
         Console.WriteLine($"[Server] Error: {ex.Message}");
         return BadRequest(new { error = ex.Message });
      }
   }
}