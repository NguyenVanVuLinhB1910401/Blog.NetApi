using System.ComponentModel.DataAnnotations;

namespace WebApiBlog.Models
{
    public class LoginModel
    {

        public string UserName { set; get; }
        public string Password { set; get; }
    }
}