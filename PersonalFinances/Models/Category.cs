using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinances.Models;

public class CategoryModel
{
   [Column("id")]
   public long? CategoryID { get; set; }

   [Column("name")]
   public string Name { get; set; } = string.Empty;

   [Column("description")]
   public string Description { get; set; } = string.Empty;

   [Column("type")]
   public string Type { get; set; } = string.Empty;
}