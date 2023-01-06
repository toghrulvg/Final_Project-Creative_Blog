using System.ComponentModel.DataAnnotations;

namespace Creative_blog.Models
{
    public class Stat : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public string Icon { get; set; }
    }
}
