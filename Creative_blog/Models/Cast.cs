using System.Reflection.Metadata.Ecma335;

namespace Creative_blog.Models
{
    public class Cast : BaseEntity
    {
        public string Image { get; set; }
        public string  Position { get; set; }
        public string  FullName { get; set; }
        public string Desc { get; set; }
        public string FbLink { get; set; }
        public string TwLink { get; set; }
        public string BxLink { get; set; }

    }
}
