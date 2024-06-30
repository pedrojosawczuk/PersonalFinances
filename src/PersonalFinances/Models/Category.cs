using Newtonsoft.Json;

namespace PersonalFinances.Models;

public class CategoryModel
{
	public long CategoryID { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public string Type { get; set; } = string.Empty;
	
	[JsonIgnore]
	public ICollection<TransactionModel> Transactions { get; set; } = new List<TransactionModel>();
}