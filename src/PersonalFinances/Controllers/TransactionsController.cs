using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using PersonalFinances.DataContext;

using PersonalFinances.Models;

namespace PersonalFinances.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
	private readonly EFDataContext _context;
	public TransactionController(EFDataContext context)
	{
		_context = context;
	}

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
					List<TransactionModel> allTransactions = new List<TransactionModel>();
					var transactions = await _context.Transactions
						.Where(e => e.User!.UserID == id)
						.Select(e => new
						{
							id = e.TransactionID,
							description = e.Description,
							value = e.Value,
							date = e.Date,
							type = e.Type,
							category = e.Category
						})
						.OrderByDescending(e => e.date)
						.ToListAsync();

					if (transactions.Count == 0)
					{
						return NoContent();
					}
					return Ok(transactions);
				}
				return Unauthorized();
			}
			return Unauthorized();
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
					TransactionModel transaction = new TransactionModel();
					var transactions = await _context.Transactions
						.Where(e => e.User!.UserID == id && e.TransactionID == transactionId)
						.Select(e => new
						{
							id = e.TransactionID,
							description = e.Description,
							value = e.Value,
							date = e.Date,
							type = e.Type,
							category = e.Category
						})
						.ToListAsync();

					if (transactions.Count == 0)
					{
						return NoContent();
					}
					return Ok(transactions);
				}
				return Unauthorized();
			}
			return Unauthorized();
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
					List<TransactionModel> allTransactions = new List<TransactionModel>();
					var transactions = await _context.Transactions
						.Where(e => e.User!.UserID == id && e.Type == "I")
						.Select(e => new
						{
							id = e.TransactionID,
							description = e.Description,
							value = e.Value,
							date = e.Date,
							type = e.Type,
							category = e.Category
						})
						.OrderByDescending(e => e.date)
						.ToListAsync();

					if (transactions.Count == 0)
					{
						return NoContent();
					}
					return Ok(transactions);
				}
				return Unauthorized();
			}
			return Unauthorized();
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
					List<TransactionModel> allTransactions = new List<TransactionModel>();
					var transactions = await _context.Transactions
						.Where(e => e.User!.UserID == id && e.Type == "E")
						.Select(e => new
						{
							id = e.TransactionID,
							description = e.Description,
							value = e.Value,
							date = e.Date,
							type = e.Type,
							category = e.Category
						})
						.OrderByDescending(e => e.date)
						.ToListAsync();

					if (transactions.Count == 0)
					{
						return NoContent();
					}
					return Ok(transactions);
				}
				return Unauthorized();
			}
			return Unauthorized();
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
					List<TransactionModel> allTransactions = new List<TransactionModel>();
					var transactions = await _context.Transactions
						.Where(e => e.User!.UserID == id && e.Category!.CategoryID == categoryId)
						.Select(e => new
						{
							id = e.TransactionID,
							description = e.Description,
							value = e.Value,
							date = e.Date,
							type = e.Type,
							category = e.Category
						})
						.ToListAsync();

					if (transactions.Count == 0)
					{
						return NoContent();
					}
					return Ok(transactions);
				}
				return Unauthorized();
			}
			return Unauthorized();
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
					var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.UserID == id);
					var dbCategory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryID == transaction.CategoryID);

					if (dbUser != null && dbCategory != null)
					{
						var newTransaction = new TransactionModel
						{
							Description = transaction.Description,
							Value = transaction.Value,
							Type = transaction.Type,
							Date = transaction.Date,
							User = dbUser,
							UserID = dbUser.UserID,
							Category = dbCategory,
							CategoryID = dbCategory.CategoryID,
						};

						await _context.Transactions.AddAsync(newTransaction);
						await _context.SaveChangesAsync();

						return Ok(newTransaction);
					}
					return BadRequest();
				}
				return Unauthorized();
			}
			return Unauthorized();
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
					var dbTransaction = await _context.Transactions.FirstOrDefaultAsync(t => t.TransactionID == updatedTransaction.TransactionID);

					if (dbTransaction != null && dbTransaction.User!.UserID == id)
					{
						dbTransaction.TransactionID = updatedTransaction.TransactionID;
						dbTransaction.Description = updatedTransaction.Description;
						dbTransaction.Value = updatedTransaction.Value;
						dbTransaction.Type = updatedTransaction.Type;
						dbTransaction.Date = updatedTransaction.Date;
						dbTransaction.Category = updatedTransaction.Category;

						await _context.SaveChangesAsync();

						return Ok(dbTransaction);
					}
					return NotFound();
				}
				return Unauthorized();
			}
			return BadRequest();
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
					var dbTransaction = await _context.Transactions.FirstOrDefaultAsync(t => t.TransactionID == transactionId);

					if (dbTransaction != null && dbTransaction.User!.UserID == id)
					{
						_context.Transactions.Remove(dbTransaction);
						await _context.SaveChangesAsync();

						return Ok();
					}
					return NotFound();
				}
				return Unauthorized();
			}
			return BadRequest();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"[Server] Error: {ex.Message}");
			return StatusCode(500, new { error = ex.Message });
		}
	}
}