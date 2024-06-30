using Newtonsoft.Json;

namespace PersonalFinances.Models;

public class UserModel
{
	public long? UserID { get; set; } = 0;
	public string? Name { get; set; } = string.Empty;
	public string? LastName { get; set; } = string.Empty;
	public string? Email { get; set; } = string.Empty;
	public string? Password { get; set; } = string.Empty;
	public byte[]? Photo;
	
	[JsonIgnore]
	public ICollection<TransactionModel> Transactions { get; set; } = new List<TransactionModel>();
}