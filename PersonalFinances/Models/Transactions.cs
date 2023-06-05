using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinances.Models;

[Table("tb_transaction")]
public partial class TransactionModel
{
   [Column("id")]
   public long TransactionID { get; set; }

   [Column("description")]
   public string Description { get; set; } = string.Empty;

   [Column("value")]
   public double Value { get; set; }

   [Column("type")]
   public string? Type { get; set; }

   [Column("date")]
   public DateOnly Date { get; set; }

   [Column("created_at")]
   public DateTime? CreatedAt { get; set; }

   [Column("updated_at")]
   public DateTime? UpdatedAt { get; set; }

   [Column("fk_user")]
   public long FkUser { get; set; }

   [Column("fk_category")]
   public long FkCategory { get; set; }

}