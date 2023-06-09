using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinances.Models;

[Table("tb_user")]
public class UserModel
{
   private long? _userID;
   private string? _name;
   private string? _lastName;
   private string? _email;
   private string? _password;
   private byte[]? _photo;

   [Column("id")]
   public long? UserID
   {
      get { return _userID; }
      protected set
      {
         if (value == null)
         {
            throw new ArgumentException("UserID cannot be null.");
         }
         _userID = value;
      }
   }

   [Column("name")]
   public string? Name
   {
      get { return _name; }
      set
      {
         if (value == null)
         {
            throw new ArgumentException("Name cannot be null.");
         }
         _name = value;
      }
   }

   [Column("lastName")]
   public string? LastName
   {
      get { return _lastName; }
      set
      {
         if (value == null)
         {
            throw new ArgumentException("LastName cannot be null.");
         }
         _lastName = value;
      }
   }

   [Column("email")]
   public string? Email
   {
      get { return _email; }
      set
      {
         if (value == null)
         {
            throw new ArgumentException("Email cannot be null.");
         }
         _email = value;
      }
   }

   [Column("password")]
   public string? Password
   {
      get { return _password; }
      set
      {
         if (value == null)
         {
            throw new ArgumentException("Password cannot be null.");
         }
         _password = value;
      }
   }

   [Column("photo")]
   public byte[]? Photo
   {
      get { return _photo; }
      set { _photo = value; }
   }

   public string FullName
   {
      get
      {
         return $"{Name} {LastName}";
      }
   }
}