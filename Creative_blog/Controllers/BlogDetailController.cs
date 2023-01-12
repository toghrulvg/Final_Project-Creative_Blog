using Creative_blog.Data;
using Creative_blog.Models;
using Creative_blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Creative_blog.Controllers
{
    public class BlogDetailController : Controller
    {
        private readonly AppDbContext _context;

        public BlogDetailController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? id)
        {
            if (id is null) return BadRequest();

            Blog blog = await _context.Blogs.Include(m => m.Comments)
                .FirstOrDefaultAsync(m => m.Id == id);


            if (blog is null) return NotFound();

            IEnumerable<Comment> comments = await _context.Comments.ToListAsync();


            List<Blog> blogs = await _context.Blogs.ToListAsync();
            


            BlogDetailVM blogVM = new BlogDetailVM
            {
                blog = blog,
                comments = (List<Comment>)comments
            };

            return View(blogVM);
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Comment(Comment newComment)
        {
            var commentCount = await _context.Comments.CountAsync();
            string unknown = "Anonymous";

            if (User.Identity.Name != null)
            {
                unknown = User.Identity.Name.ToString();
            }

            Comment comment = new Comment()
            {
                Content = newComment.Content,
                BlogId = newComment.BlogId,
                UserName = unknown,
                Datetime = DateTime.Now.ToString()
            };

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { id = comment.BlogId });
        }



    }
}
