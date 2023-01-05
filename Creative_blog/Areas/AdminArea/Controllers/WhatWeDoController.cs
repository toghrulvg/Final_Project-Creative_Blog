using Creative_blog.Data;
using Creative_blog.Helpers;
using Creative_blog.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Creative_blog.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class WhatWeDoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public WhatWeDoController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        
        public async Task<IActionResult> Index()
        {
            return View(await _context.WhatWeDos.Where(m => !m.IsDeleted).ToListAsync());
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WhatWeDo whatWeDo)
        {
            if (!ModelState.IsValid) return View();


            if (!whatWeDo.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Please choose correct image type");
                return View();
            }


            if (!whatWeDo.Photo.CheckFileSize(1000))
            {
                ModelState.AddModelError("Photo", "Please choose correct image size");
                return View();
            }


            string fileName = Guid.NewGuid().ToString() + "_" + whatWeDo.Photo.FileName;

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/imgs", fileName);

            await Helper.SaveFile(path, whatWeDo.Photo);

            WhatWeDo newWhatWeDo = new WhatWeDo
            {
                Icon = fileName,
                Name = whatWeDo.Name,
                Desc = whatWeDo.Desc
                
            };


            await _context.WhatWeDos.AddAsync(newWhatWeDo);


            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}
