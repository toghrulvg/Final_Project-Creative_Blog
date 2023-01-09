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
    public class PortfolioController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public PortfolioController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Portfolios.Where(m => !m.IsDeleted).ToListAsync());
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Portfolio portfolio)
        {
            if (!ModelState.IsValid) return View();


            if (!portfolio.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Please choose correct image type");
                return View();
            }


            if (!portfolio.Photo.CheckFileSize(1000))
            {
                ModelState.AddModelError("Photo", "Please choose correct image size");
                return View();
            }


            string fileName = Guid.NewGuid().ToString() + "_" + portfolio.Photo.FileName;

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/imgs", fileName);

            await Helper.SaveFile(path, portfolio.Photo);

            Portfolio newPortfolio = new Portfolio
            {
                Image = fileName

            };


            await _context.Portfolios.AddAsync(newPortfolio);


            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            Portfolio portfolio = await GetByIdAsync((int)id);

            if (portfolio == null) return NotFound();

            return View(portfolio);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Portfolio portfolio)
        {
            if (id is null) return BadRequest();

            if (portfolio.Photo == null) return RedirectToAction(nameof(Index));

            var dbPortfolio = await GetByIdAsync((int)id);

            if (dbPortfolio == null) return NotFound();

            if (!portfolio.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Please choose correct image type");
                return View(dbPortfolio);
            }

            if (!portfolio.Photo.CheckFileSize(1000))
            {
                ModelState.AddModelError("Photo", "Please choose correct image size");
                return View(dbPortfolio);
            }

            string oldPath = Helper.GetFilePath(_env.WebRootPath, "assets/imgs", dbPortfolio.Image);

            Helper.DeleteFile(oldPath);

            string fileName = Guid.NewGuid().ToString() + "_" + portfolio.Photo.FileName;

            string newPath = Helper.GetFilePath(_env.WebRootPath, "assets/imgs", fileName);


            using (FileStream stream = new FileStream(newPath, FileMode.Create))
            {
                await portfolio.Photo.CopyToAsync(stream);
            }

            dbPortfolio.Image = fileName;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            Portfolio portfolio = await GetByIdAsync((int)id);

            if (portfolio == null) return NotFound();

            return View(portfolio);

        }

        public async Task<IActionResult> Delete(int id)
        {
            Portfolio portfolio = await GetByIdAsync(id);

            if (portfolio == null) return NotFound();


            portfolio.IsDeleted = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<Portfolio> GetByIdAsync(int id)
        {
            return await _context.Portfolios.FindAsync(id);
        }
    }
}
