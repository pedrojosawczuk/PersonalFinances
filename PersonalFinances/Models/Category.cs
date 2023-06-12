using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace PersonalFinances.Models;

[Table("tb_category")]
public class CategoryModel
{
   private long _categoryID;
   private string _name = string.Empty;
   private string _description = string.Empty;
   private string _type = string.Empty;

   public CategoryModel(long categoryID, string name, string description, string type)
   {
      CategoryID = categoryID;
      Name = name;
      Description = description;
      Type = type;
   }

   [Column("id")]
   [JsonProperty("categoryID")]
   public long CategoryID
   {
      get { return _categoryID; }
      set { _categoryID = value; }
   }

   [Column("name")]
   [JsonProperty("name")]
   public string Name
   {
      get { return _name; }
      set { _name = value; }
   }

   [Column("description")]
   [JsonProperty("description")]
   public string Description
   {
      get { return _description; }
      set { _description = value; }
   }

   [Column("type")]
   [JsonProperty("type")]
   public string Type
   {
      get { return _type; }
      set { _type = value; }
   }
}