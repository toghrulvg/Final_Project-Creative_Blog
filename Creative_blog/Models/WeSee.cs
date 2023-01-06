using System.ComponentModel.DataAnnotations;

namespace Creative_blog.Models
{
    public class WeSee : BaseEntity
    {
        [Required]
        public string Icon { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
