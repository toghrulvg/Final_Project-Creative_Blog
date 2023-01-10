using Creative_blog.Models;
using System.Collections.Generic;

namespace Creative_blog.ViewModels
{
    public class BlogVM
    {
        public List<Blog> Blogs { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
