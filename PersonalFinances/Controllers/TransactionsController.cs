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
         if (HttpContext.Request.Headers["Authorization"] != String.Empty)
         {
            var token = HttpContext.Request.Headers["Authorization"];

            var jwtToken = new JwtSecurityToken(token);
            var payload = jwtToken.Payload;

            if (payload.TryGetValue("id", out object? idObj) && long.TryParse(idObj.ToString(), out long id))
            {
               using (var context = new EFDataContext())
               {
                  List<TransactionModel> allTransactions = new List<TransactionModel>();
                  var transactions = await context.Transactions
                     .Where(e => e.FkUser == id)
                     .Select(e => new
                     {
                        id = e.TransactionID,
                        description = e.Description,
                        value = e.Value,
                        date = e.Date,
                        type = e.Type,
                        category = e.FkCategory
                     })
                     .OrderByDescending(e => e.date)
                     .ToListAsync();

                  if (transactions == null)
                  {
                     return Ok(new { message = "No transactions" });
                  }
                  return Ok(new { transaction = transactions });
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
   [HttpGet("{transactionId}")]
   public async Task<ActionResult<IEnumerable<object>>> GetOneTransaction(long transactionId)
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
                  TransactionModel transaction = new TransactionModel();
                  var transactions = await context.Transactions
                     .Where(e => e.FkUser == id && e.TransactionID == transactionId)
                     .Select(e => new
                     {
                        id = e.TransactionID,
                        description = e.Description,
                        value = e.Value,
                        date = e.Date,
                        type = e.Type,
                        category = e.FkCategory
                     })
                     .ToListAsync();

                  if (transactions == null)
                  {
                     return Ok(new { message = "No transactions" });
                  }
                  return Ok(new { transaction = transactions });
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
   public async Task<ActionResult<IEnumerable<object>>> ListIncome()
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
                  List<TransactionModel> allTransactions = new List<TransactionModel>();
                  var transactions = await context.Transactions
                     .Where(e => e.FkUser == id && e.Type == "I")
                     .Select(e => new
                     {
                        id = e.TransactionID,
                        description = e.Description,
                        value = e.Value,
                        date = e.Date,
                        type = e.Type,
                        category = e.FkCategory
                     })
                     .OrderByDescending(e => e.date)
                     .ToListAsync();

                  if (transactions == null)
                  {
                     return Ok(new { message = "No Income" });
                  }
                  return Ok(new { transaction = transactions });
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
   public async Task<ActionResult<IEnumerable<object>>> ListExpenses()
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
                  List<TransactionModel> allTransactions = new List<TransactionModel>();
                  var transactions = await context.Transactions
                     .Where(e => e.FkUser == id && e.Type == "E")
                     .Select(e => new
                     {
                        id = e.TransactionID,
                        description = e.Description,
                        value = e.Value,
                        date = e.Date,
                        type = e.Type,
                        category = e.FkCategory
                     })
                     .OrderByDescending(e => e.date)
                     .ToListAsync();

                  if (transactions == null)
                  {
                     return Ok(new { message = "No Expenses" });
                  }
                  return Ok(new { transaction = transactions });
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
   [HttpGet("category/{categoryId}")]
   public async Task<ActionResult<IEnumerable<object>>> ListExpenses(long categoryId)
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
                  List<TransactionModel> allTransactions = new List<TransactionModel>();
                  var transactions = await context.Transactions
                     .Where(e => e.FkUser == id && e.FkCategory == categoryId)
                     .Select(e => new
                     {
                        id = e.TransactionID,
                        description = e.Description,
                        value = e.Value,
                        date = e.Date,
                        type = e.Type,
                        category = e.FkCategory
                     })
                     .ToListAsync();

                  if (transactions == null)
                  {
                     return Ok(new { message = "No Transaction" });
                  }
                  return Ok(new { transaction = transactions });
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
               using (var context = new EFDataContext())
               {
                  var dbUser = await context.Users.FirstOrDefaultAsync(u => u.UserID == id);
                  if (dbUser != null)
                  {
                     var newTransaction = new TransactionModel(
                        transaction.Description,
                        transaction.Value,
                        transaction.Type,
                        transaction.Date,
                        id,
                        transaction.FkCategory
                     );

                     await context.Transactions.AddAsync(newTransaction);
                     await context.SaveChangesAsync();

                     return Ok(new { transaction = newTransaction });
                  }
                  return BadRequest(new { error = "Failed to retrieve the user!" });
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

   [HttpPatch()]
   public async Task<ActionResult<TransactionModel>> UpdateTransaction([FromBody] TransactionModel updatedTransaction)
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
                  var dbTransaction = await context.Transactions.FirstOrDefaultAsync(t => t.TransactionID == updatedTransaction.TransactionID);

                  if (dbTransaction != null && dbTransaction.FkUser == id)
                  {
                     dbTransaction.TransactionID = updatedTransaction.TransactionID;
                     dbTransaction.Description = updatedTransaction.Description;
                     dbTransaction.Value = updatedTransaction.Value;
                     dbTransaction.Type = updatedTransaction.Type;
                     dbTransaction.Date = updatedTransaction.Date;
                     dbTransaction.FkCategory = updatedTransaction.FkCategory;

                     await context.SaveChangesAsync();

                     return Ok(new { transaction = dbTransaction });
                  }
                  return NotFound(new { error = "No Transaction found." });
               }
            }
            return Unauthorized(new { error = "Invalid Token!" });
         }
         return BadRequest(new { error = "No token!" });
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

   [HttpDelete("{transactionId}")]
   public async Task<ActionResult<TransactionModel>> DeleteTransaction(long transactionId)
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
                  var dbTransaction = await context.Transactions.FirstOrDefaultAsync(t => t.TransactionID == transactionId);

                  if (dbTransaction != null && dbTransaction.FkUser == id)
                  {
                     context.Transactions.Remove(dbTransaction);
                     await context.SaveChangesAsync();
                     return Ok();
                  }
                  return NotFound(new { error = "No Transaction found." });
               }
            }
            return Unauthorized(new { error = "Invalid Token!" });
         }
         return BadRequest(new { error = "No token!" });
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