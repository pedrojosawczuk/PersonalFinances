using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace PersonalFinances.Models;

[Table("tb_transaction")]
public partial class TransactionModel
{
   private long? _transactionID;
   private string? _description = string.Empty;
   private double? _value;
   private string? _type = string.Empty;
   private DateOnly? _date;
   private DateTime? _createdAt;
   private DateTime? _updatedAt;
   private long _fkUser;
   private long _fkCategory;

   public TransactionModel()
   {
      // Parameterless constructor
   }
   public TransactionModel(long transactionID, string description, double? value, string type, DateOnly? date, long fkUser, long fkCategory)
   {
      TransactionID = transactionID;
      Description = description;
      Value = value;
      Type = type;
      Date = date;
      FkUser = fkUser;
      FkCategory = fkCategory;
   }
   public TransactionModel(string description, double? value, string type, DateOnly? date, long fkUser, long fkCategory)
   {
      Description = description;
      Value = value;
      Type = type;
      Date = date;
      FkUser = fkUser;
      FkCategory = fkCategory;
   }

   [Column("id")]
   [JsonProperty("transactionID")]
   public long? TransactionID
   {
      get { return _transactionID; }
      set
      {
         if (value == 0)
         {
            throw new ArgumentException("TransactionID cannot be null.");
         }
         _transactionID = value;
      }
   }

   [Column("description")]
   [JsonProperty("description")]
   public string? Description
   {
      get { return _description; }
      set
      {
         if (string.IsNullOrEmpty(value))
         {
            throw new ArgumentException("Description cannot be null.");
         }
         _description = value;
      }
   }

   [Column("value")]
   [JsonProperty("value")]
   public double? Value
   {
      get { return _value; }
      set
      {
         if (value == 0)
         {
            throw new ArgumentException("Value cannot be null.");
         }
         _value = value;
      }
   }

   [Column("type")]
   [JsonProperty("type")]
   public string? Type
   {
      get { return _type; }
      set
      {
         if (string.IsNullOrEmpty(value))
         {
            throw new ArgumentException("Type cannot be null.");
         }
         _type = value;
      }
   }

   [Column("date")]
   [JsonProperty("date")]
   public DateOnly? Date
   {
      get { return _date; }
      set
      {
         if (value == null)
         {
            throw new ArgumentException("Date cannot be null.");
         }
         _date = value;
      }
   }

   [Column("created_at")]
   [JsonProperty("created_at")]
   public DateTime? CreatedAt
   {
      get { return _createdAt; }
      protected set { _createdAt = value; }
   }

   [Column("updated_at")]
   [JsonProperty("updated_at")]
   public DateTime? UpdatedAt
   {
      get { return _updatedAt; }
      protected set { _updatedAt = value; }
   }

   [Column("fk_user")]
   [JsonProperty("fk_user")]
   public long FkUser
   {
      get { return _fkUser; }
      set
      {
         if (value == 0)
         {
            throw new ArgumentException("FKUser cannot be null.");
         }
         _fkUser = value;
      }
   }

   [Column("fk_category")]
   [JsonProperty("fk_category")]
   public long FkCategory
   {
      get { return _fkCategory; }
      set
      {
         if (value == 0)
         {
            throw new ArgumentException("FKCategory cannot be null.");
         }
         _fkCategory = value;
      }
   }
}