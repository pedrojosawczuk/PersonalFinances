using Newtonsoft.Json;

namespace PersonalFinances.Models;

public partial class TransactionModel
{
	public long? TransactionID { get; set; } = 0;
	public string? Description { get; set; } = string.Empty;
	public double? Value { get; set; } = 0;
	public string? Type { get; set; } = string.Empty;
	public DateOnly? Date { get; set; }
	public DateTime? CreatedAt { get; private set; }
	public DateTime? UpdatedAt { get; private set; }

	[JsonIgnore]
	public UserModel? User { get; set; }

	[JsonProperty("fk_user")]
	public long? UserID { get; set; }

	[JsonIgnore]
	public CategoryModel? Category { get; set; }

	[JsonProperty("fk_category")]
	public long? CategoryID { get; set; }
}