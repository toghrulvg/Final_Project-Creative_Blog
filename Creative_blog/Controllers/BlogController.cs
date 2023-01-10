using Creative_blog.Data;
using Creative_blog.Models;
using Creative_blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Creative_blog.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        public BlogController(AppDbContext context)
        {
            _context = context;
        }

        public  async Task<IActionResult> Index()
        {
            List<Blog> blogs =await _context.Blogs.Include(m => m.Comments).ToListAsync();

            BlogVM blogVM = new BlogVM()
            {
                Blogs = blogs
            };

            return View(blogVM);
        }






        private async Task<Blog> GetByIdAsync(int id)
        {
            return await _context.Blogs.FindAsync(id);
        }
    }
}
