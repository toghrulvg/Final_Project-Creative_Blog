namespace Creative_blog.Models
{
        public abstract class BaseEntity
        {
            public int Id { get; set; }
            public bool IsDeleted { get; set; } = false;
        }
}
