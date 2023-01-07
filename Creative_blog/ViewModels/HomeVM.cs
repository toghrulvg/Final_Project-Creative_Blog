using Creative_blog.Models;
using System.Collections.Generic;

namespace Creative_blog.ViewModels
{
    public class HomeVM
    {

        public List<Blog> Blogs { get; set; }
        public List<Cast> Casts { get; set; }
        public List<ContactInfo> ContactInfos { get; set; }
        public List<OurService> OurServices { get; set; }
        public List<Portfolio> Portfolios { get; set; }
        public List<Service> Services { get; set; }
        public List<Stat> Stats { get; set; }
        public List<Testimonial> Testimonials { get; set; }
        public List<WeSee> WeSees { get; set; }
        public List<WhatWeDo> WhatWeDos { get; set; }
        public List<TouchMessage> TouchMessages { get; set; }



    }
}
