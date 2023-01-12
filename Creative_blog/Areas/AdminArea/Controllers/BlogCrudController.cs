using Creative_blog.Data;
using Creative_blog.Helpers;
using Creative_blog.Helpers.Enums;
using Creative_blog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Creative_blog.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "Admin")]
    public class BlogCrudController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public BlogCrudController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Blogs.Where(m => !m.IsDeleted)
                .Include(m => m.Comments)
                .ToListAsync());
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {
            if (!ModelState.IsValid) return View();


            if (!blog.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Please choose correct image type");
                return View();
            }


            if (!blog.Photo.CheckFileSize(1000))
            {
                ModelState.AddModelError("Photo", "Please choose correct image size");
                return View();
            }


            string fileName = Guid.NewGuid().ToString() + "_" + blog.Photo.FileName;

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/blog/assets/images", fileName);

            await Helper.SaveFile(path, blog.Photo);

            Blog newBlog = new Blog
            {
                Image = fileName,
                Desc = blog.Desc,
                Name = blog.Name,
                CommentCount = 1,
                LastModified = DateTime.Now
            };


            await _context.Blogs.AddAsync(newBlog);


            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            Blog blog = await GetByIdAsync((int)id);

            if (blog == null) return NotFound();

            return View(blog);

        }
        public async Task<IActionResult> Delete(int id)
        {
            Blog blog = await GetByIdAsync(id);

            if (blog == null) return NotFound();


            blog.IsDeleted = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Blog blog)
        {
            if (id is null) return BadRequest();

            if (blog.Photo == null) return RedirectToAction(nameof(Index));

            var dbBlog = await GetByIdAsync((int)id);

            if (dbBlog == null) return NotFound();

            if (!blog.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Please choose correct image type");
                return View(dbBlog);
            }

            if (!blog.Photo.CheckFileSize(1000))
            {
                ModelState.AddModelError("Photo", "Please choose correct image size");
                return View(dbBlog);
            }

            string oldPath = Helper.GetFilePath(_env.WebRootPath, "assets/blog/assets/images", dbBlog.Image);

            Helper.DeleteFile(oldPath);

            string fileName = Guid.NewGuid().ToString() + "_" + blog.Photo.FileName;

            string newPath = Helper.GetFilePath(_env.WebRootPath, "assets/imgs", fileName);


            using (FileStream stream = new FileStream(newPath, FileMode.Create))
            {
                await blog.Photo.CopyToAsync(stream);
            }

            dbBlog.Image = fileName;
            dbBlog.Name = blog.Name;
            dbBlog.Desc = blog.Desc;
            dbBlog.LastModified = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            Blog blog = await GetByIdAsync((int)id);

            if (blog == null) return NotFound();

            return View(blog);

        }





        private async Task<Blog> GetByIdAsync(int id)
        {
            return await _context.Blogs.FindAsync(id);
        }
    }
}
