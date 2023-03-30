using System.ComponentModel.DataAnnotations;

namespace WebApiBlog.Models
{
    public class RegisterModel
    {
        [Required]
        public string Email { set; get; }
        [Required]
        public string Password { set; get; }
        public string FirstName { set; get; }

        public string LastName { set; get; }

        public void toString()
        {
            Console.WriteLine(Email + Password + FirstName + LastName);
        }
    }
}