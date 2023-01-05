namespace Creative_blog.Models
{
    public class ContactInfo : BaseEntity
    {
        public string Location { get; set; }
        public int PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
    }
}
