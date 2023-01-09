using Creative_blog.Data;
using Creative_blog.Helpers.Enums;
using Creative_blog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Creative_blog.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "Admin")]
    public class OurServiceController : Controller
    {
        private readonly AppDbContext _context;
        public OurServiceController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.OurServices.Where(m => !m.IsDeleted).ToListAsync());
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OurService ourService)
        {
            if (!ModelState.IsValid) return View();

            OurService newOurService = new OurService
            {
                Icon = ourService.Icon,
                Name = ourService.Name,
                Desc = ourService.Desc
            };

            await _context.OurServices.AddAsync(newOurService);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            OurService ourService = await GetByIdAsync((int)id);

            if (ourService == null) return NotFound();

            return View(ourService);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, OurService ourService)
        {
            if (id is null) return BadRequest();

            var dbOurService = await GetByIdAsync((int)id);

            if (dbOurService == null) return NotFound();

            dbOurService.Icon = ourService.Icon;
            dbOurService.Name = ourService.Name;
            dbOurService.Desc = ourService.Desc;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            OurService ourService = await GetByIdAsync((int)id);

            if (ourService == null) return NotFound();

            return View(ourService);
        }

        public async Task<IActionResult> Delete(int id)
        {
            OurService ourService = await GetByIdAsync(id);

            if (ourService == null) return NotFound();


            ourService.IsDeleted = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        private async Task<OurService> GetByIdAsync(int id)
        {
            return await _context.OurServices.FindAsync(id);
        }
    }
}
