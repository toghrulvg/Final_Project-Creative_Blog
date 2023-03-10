using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Creative_blog.Models
{
    public class Blog : BaseEntity
    {
        
        public string Image { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Desc { get; set; }
        public int CommentCount { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Can't be empty")]
        public IFormFile Photo { get; set; }
        public List<Comment> Comments { get; set; }
        [Required]
        public DateTime LastModified { get; set; }
    }
}
