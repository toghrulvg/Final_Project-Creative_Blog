namespace Creative_blog.Models
{
    public class Blog : BaseEntity
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public int CommentCount { get; set; }
    }
}
