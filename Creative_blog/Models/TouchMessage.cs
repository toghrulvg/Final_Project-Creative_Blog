using System;
using System.ComponentModel.DataAnnotations;

namespace Creative_blog.Models
{
    public class TouchMessage : BaseEntity
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Fullname { get; set; }
        [Required]
        public string Message { get; set; }
        
        public DateTime Date { get; set; }
    }
}
