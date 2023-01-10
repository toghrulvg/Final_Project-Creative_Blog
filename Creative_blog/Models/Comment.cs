using System;
using System.ComponentModel.DataAnnotations;

namespace Creative_blog.Models
{
    public class Comment : BaseEntity
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        //[Required]
        public string Content { get; set; }
        public int BlogId { get; set; }
        public string Datetime { get; set; } = DateTime.Now.ToString();
        public Blog Blog { get; set; }
    }
}
