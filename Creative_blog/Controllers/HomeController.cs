using Creative_blog.Data;
using Creative_blog.Models;
using Creative_blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Creative_blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Blog> blogs = _context.Blogs.ToList();
            List<Cast> casts = _context.Casts.ToList();
            List<ContactInfo> contactInfos = _context.ContactInfos.ToList();
            List<OurService> ourServices = _context.OurServices.ToList();
            List<Portfolio> portfolios = _context.Portfolios.ToList();
            List<Service> services = _context.Services.ToList();
            List<Stat> stats = _context.Stats.ToList();
            List<Testimonial> testimonials = _context.Testimonials.ToList();
            List<WeSee> weSees = _context.WeSees.ToList();
            List<WhatWeDo> whatWeDos = _context.WhatWeDos.ToList();

            HomeVM home = new HomeVM
            {
                Blogs = blogs,
                Casts = casts,
                ContactInfos = contactInfos,
                OurServices = ourServices,
                Portfolios = portfolios,
                Services = services,
                Stats = stats,
                Testimonials = testimonials,
                WeSees = weSees,
                WhatWeDos = whatWeDos
            };

            return View(home);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
