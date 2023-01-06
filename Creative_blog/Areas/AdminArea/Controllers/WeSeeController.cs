using Creative_blog.Data;
using Creative_blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Creative_blog.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class WeSeeController : Controller
    {
        private readonly AppDbContext _context;
        public WeSeeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.WeSees.Where(m => !m.IsDeleted).ToListAsync());
        }


        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WeSee weSee)
        {
            if (!ModelState.IsValid) return View();

            WeSee newWeSee = new WeSee
            {
                Icon = weSee.Icon,
                Name = weSee.Name,
            };

            await _context.WeSees.AddAsync(newWeSee);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));


        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            WeSee weSee = await GetByIdAsync((int)id);

            if (weSee == null) return NotFound();

            return View(weSee);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, WeSee weSee)
        {
            if (id is null) return BadRequest();

            var dbWeSee = await GetByIdAsync((int)id);

            if (dbWeSee == null) return NotFound();

            dbWeSee.Icon = weSee.Icon;
            dbWeSee.Name = weSee.Name;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            WeSee weSee = await GetByIdAsync((int)id);

            if (weSee == null) return NotFound();

            return View(weSee);
        }
        public async Task<IActionResult> Delete(int id)
        {
            WeSee weSee = await GetByIdAsync(id);

            if (weSee == null) return NotFound();


            weSee.IsDeleted = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        private async Task<WeSee> GetByIdAsync(int id)
        {
            return await _context.WeSees.FindAsync(id);
        }
    }
}
