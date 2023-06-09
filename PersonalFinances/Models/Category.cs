using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinances.Models;

[Table("tb_category")]
public class CategoryModel
{
   private long _categoryID;
   private string _name = string.Empty;
   private string _description = string.Empty;
   private string _type = string.Empty;

   [Column("id")]
   public long CategoryID
   {
      get { return _categoryID; }
      protected set { _categoryID = value; }
   }

   [Column("name")]
   public string Name
   {
      get { return _name; }
      protected set { _name = value; }
   }

   [Column("description")]
   public string Description
   {
      get { return _description; }
      protected set { _description = value; }
   }

   [Column("type")]
   public string Type
   {
      get { return _type; }
      protected set { _type = value; }
   }
}