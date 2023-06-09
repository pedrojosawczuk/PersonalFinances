using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinances.Models;

[Table("tb_transaction")]
public partial class TransactionModel
{
   private long _transactionID;
   private string _description = string.Empty;
   private double _value;
   private string _type = string.Empty;
   private DateOnly _date;
   private DateTime? _createdAt;
   private DateTime? _updatedAt;
   private long _fkUser;
   private long _fkCategory;

   [Column("id")]
   public long TransactionID
   {
      get { return _transactionID; }
      protected set { _transactionID = value; }
   }

   [Column("description")]
   public string Description
   {
      get { return _description; }
      set
      {
         if (value == null)
         {
            throw new ArgumentNullException(nameof(Description), "Description cannot be null.");
         }
         _description = value;
      }
   }

   [Column("value")]
   public double Value
   {
      get { return _value; }
      set
      {
         if (value == 0)
         {
            throw new ArgumentNullException(nameof(Value), "Value cannot be null.");
         }
         _value = value;
      }
   }

   [Column("type")]
   public string Type
   {
      get { return _type; }
      set
      {
         if (value == null)
         {
            throw new ArgumentNullException(nameof(Type), "Type cannot be null.");
         }
         _type = value;
      }
   }

   [Column("date")]
   public DateOnly Date
   {
      get { return _date; }
      set
      {
         if (value == null)
         {
            throw new ArgumentNullException(nameof(Date), "Date cannot be null.");
         }
         _date = value;
      }
   }

   [Column("created_at")]
   public DateTime? CreatedAt
   {
      get { return _createdAt; }
      protected set { _createdAt = value; }
   }

   [Column("updated_at")]
   public DateTime? UpdatedAt
   {
      get { return _updatedAt; }
      protected set { _updatedAt = value; }
   }

   [Column("fk_user")]
   public long FkUser
   {
      get { return _fkUser; }
      set
      {
         if (value == 0)
         {
            throw new ArgumentNullException(nameof(FkUser), "FKUser cannot be null.");
         }
         _fkUser = value;
      }
   }

   [Column("fk_category")]
   public long FkCategory
   {
      get { return _fkCategory; }
      set
      {
         if (value == 0)
         {
            throw new ArgumentNullException(nameof(FkCategory), "FKCategory cannot be null.");
         }
         _fkCategory = value;
      }
   }
}