using System.ComponentModel.DataAnnotations;

namespace Creative_blog.Models
{
    public class Service : BaseEntity
    {
        [Required]
        public string Icon { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Desc { get; set; }
    }
}
