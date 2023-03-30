using System.ComponentModel.DataAnnotations;

namespace WebApiBlog.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Title { set; get; }
        [Required]
        [DataType(DataType.Text)]
        public string Description { set; get; }
        public string UserId { set; get; }
        public AppUser User { set; get; }
        public List<Post> Posts { set; get; }
    }
}