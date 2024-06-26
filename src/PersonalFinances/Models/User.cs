using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

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

   public UserModel()
   {
      // Parameterless constructor
   }
   public UserModel(long userID, string name, string lastName, string email, string password, byte[]? photo)
   {
      UserID = userID;
      Name = name;
      LastName = lastName;
      Email = email;
      Password = password;
      Photo = photo;
   }
   public UserModel(string name, string lastName, string email, string password)
   {
      Name = name;
      LastName = lastName;
      Email = email;
      Password = password;
   }
   public UserModel(string email, string password)
   {
      Email = email;
      Password = password;
   }

   [Column("id")]
   [JsonProperty("userID")]
   public long? UserID
   {
      get { return _userID; }
      set
      {
         if (value == 0)
         {
            throw new ArgumentException("UserID cannot be null.");
         }
         _userID = value;
      }
   }

   [Column("name")]
   [JsonProperty("name")]
   public string? Name
   {
      get { return _name; }
      set
      {
         if (string.IsNullOrEmpty(value))
         {
            throw new ArgumentException("Name cannot be null.");
         }
         _name = value;
      }
   }

   [Column("lastName")]
   [JsonProperty("lastName")]
   public string? LastName
   {
      get { return _lastName; }
      set
      {
         if (string.IsNullOrEmpty(value))
         {
            throw new ArgumentException("LastName cannot be null.");
         }
         _lastName = value;
      }
   }

   [Column("email")]
   [JsonProperty("email")]
   public string? Email
   {
      get { return _email; }
      set
      {
         if (string.IsNullOrEmpty(value))
         {
            throw new ArgumentException("Email cannot be null.");
         }
         _email = value;
      }
   }

   [Column("password")]
   [JsonProperty("password")]
   public string? Password
   {
      get { return _password; }
      set
      {
         if (string.IsNullOrEmpty(value))
         {
            throw new ArgumentException("Password cannot be null.");
         }
         _password = value;
      }
   }

   [Column("photo")]
   [JsonProperty("photo")]
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