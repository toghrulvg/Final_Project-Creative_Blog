using Creative_blog.Models;
using System.Collections.Generic;

namespace Creative_blog.ViewModels
{
    public class BlogDetailVM
    {
        public Blog blog { get; set; }
        public IEnumerable<Comment> comments { get; set; }
    }
}
