using Microsoft.AspNetCore.Identity;

namespace WebApiBlog.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string Address { set; get; }
        public List<Category> Categories { set; get; }
    }
}