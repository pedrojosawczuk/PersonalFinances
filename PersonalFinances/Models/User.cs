using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinances.Models;

[Table("tb_user")]
public class UserModel
{
   [Column("id")]
   public long? UserID { get; set; }

   [Column("name")]
   public string? Name { get; set; }

   [Column("lastName")]
   public string? LastName { get; set; }

   [Column("email")]
   public string? Email { get; set; }

   [Column("password")]
   public string? Password { get; set; }

   [Column("photo")]
   public byte[]? Photo { get; set; }
   /*
       [Column("created_at")]
       public DateTime? CreatedAt { get; set; }

       [Column("updated_at")]
       public DateTime? UpdatedAt { get; set; }
       */
   /*

   [Column("name")]
   private string? _name;

   [Column("lastname")]
   private string? _lastName;

   [Column("email")]
   private string? _email;

   [Column("password")]
   private string? _password;

   [Column("photo")]
   private byte[]? _photo;*/
   /*
       public UserModel(long userID, string name, string lastName, string email, string password, byte[] photo)
       {
           UserID = userID;
           _name = name;
           _lastName = lastName;
           _email = email;
           _password = password;
           _photo = photo;
       }
       public UserModel(string name, string lastName, string email, string password)
       {
           _name = name;
           _lastName = lastName;
           _email = email;
           _password = password;
       }

       [JsonConstructor]
       public UserModel(string email, string password)
       {
           _email = email;
           _password = password;
       }

       public string Name
       {
           get { return _name; }
           set
           {
               if (value != null)
                   _name = value;
               else
                   throw new ArgumentException("Name cannot be null.");
           }
       }
       public string LastName
       {
           get { return _lastName; }
           set
           {
               if (value != null)
                   _lastName = value;
               else
                   throw new ArgumentException("Last name cannot be null.");
           }
       }
       public string Email
       {
           get { return _email; }
           set
           {
               if (value != null)
                   _email = value;
               else
                   throw new ArgumentException("Email cannot be null.");
           }
       }
       public string Password
       {
           get { return _password; }
           set
           {
               if (value.Length > 8)
                   _password = value;
               else
                   throw new ArgumentException("Password must be longer than 8 characters!");
           }
       }
       public byte[]? Photo
       {

           get { return _photo; }
           set { _photo = value; }
       }
   */
   public string FullName
   {
      get
      {
         return $"{Name} {LastName}";
      }
   }

}