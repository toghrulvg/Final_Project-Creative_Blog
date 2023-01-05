using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Creative_blog.Models
{
    public class WhatWeDo : BaseEntity
    {
        
        public string Icon { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Desc { get; set; }
        [Required]
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
