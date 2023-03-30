using System.ComponentModel.DataAnnotations;

namespace WebApiBlog.Models
{
    public class Post
    {
        public int Id { set; get; }
        [Required]
        public string Title { set; get; }
        [DataType(DataType.Text)]
        [Required]
        public string Desciption { set; get; }
        public string Link { set; get; }
        public DateTime CreateAt { set; get; }
        [Required]
        public int CategoryId { set; get; }
        public Category Category { set; get; }
    }
}