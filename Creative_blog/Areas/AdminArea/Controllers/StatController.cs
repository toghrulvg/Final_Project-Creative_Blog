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
    public class StatController : Controller
    {
        private readonly AppDbContext _context;
        public StatController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Stats.Where(m => !m.IsDeleted).ToListAsync());
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Stat stat)
        {
            if (!ModelState.IsValid) return View();

            Stat newStat = new Stat
            {
                Icon = stat.Icon,
                Name = stat.Name,
                Count = stat.Count  
            };

            await _context.Stats.AddAsync(newStat);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            Stat stat = await GetByIdAsync((int)id);

            if (stat == null) return NotFound();

            return View(stat);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Stat stat)
        {
            if (id is null) return BadRequest();

            var dbStat = await GetByIdAsync((int)id);

            if (dbStat == null) return NotFound();

            dbStat.Icon = stat.Icon;
            dbStat.Name = stat.Name;
            dbStat.Count = stat.Count;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            Stat stat = await GetByIdAsync((int)id);

            if (stat == null) return NotFound();

            return View(stat);
        }

        public async Task<IActionResult> Delete(int id)
        {
            Stat stat = await GetByIdAsync(id);

            if (stat == null) return NotFound();


            stat.IsDeleted = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }















        private async Task<Stat> GetByIdAsync(int id)
        {
            return await _context.Stats.FindAsync(id);
        }
    }
}
