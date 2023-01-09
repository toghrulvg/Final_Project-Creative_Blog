using Creative_blog.Data;
using Creative_blog.Helpers;
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

        public async Task<IActionResult> Delete(int id)
        {
            WhatWeDo whatWeDo = await GetByIdAsync(id);

            if (whatWeDo == null) return NotFound();


            whatWeDo.IsDeleted = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            WhatWeDo whatWeDo = await GetByIdAsync((int)id);

            if (whatWeDo == null) return NotFound();

            return View(whatWeDo);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, WhatWeDo whatWeDo)
        {
            if (id is null) return BadRequest();

            if (whatWeDo.Photo == null) return RedirectToAction(nameof(Index));

            var dbwhatWeDo = await GetByIdAsync((int)id);

            if (dbwhatWeDo == null) return NotFound();

            if (!whatWeDo.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Please choose correct image type");
                return View(dbwhatWeDo);
            }

            if (!whatWeDo.Photo.CheckFileSize(1000))
            {
                ModelState.AddModelError("Photo", "Please choose correct image size");
                return View(dbwhatWeDo);
            }

            string oldPath = Helper.GetFilePath(_env.WebRootPath, "assets/imgs", dbwhatWeDo.Icon);

            Helper.DeleteFile(oldPath);

            string fileName = Guid.NewGuid().ToString() + "_" + whatWeDo.Photo.FileName;

            string newPath = Helper.GetFilePath(_env.WebRootPath, "assets/imgs", fileName);


            using (FileStream stream = new FileStream(newPath, FileMode.Create))
            {
                await whatWeDo.Photo.CopyToAsync(stream);
            }

            dbwhatWeDo.Icon = fileName;
            dbwhatWeDo.Name = whatWeDo.Name;
            dbwhatWeDo.Desc = whatWeDo.Desc;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            WhatWeDo whatWeDo = await GetByIdAsync((int)id);

            if (whatWeDo == null) return NotFound();

            return View(whatWeDo);

        }






        private async Task<WhatWeDo> GetByIdAsync(int id)
        {
            return await _context.WhatWeDos.FindAsync(id);
        }
    }
}
