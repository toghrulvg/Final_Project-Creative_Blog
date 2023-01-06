using Creative_blog.Data;
using Creative_blog.Helpers;
using Creative_blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Creative_blog.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;
        public ServiceController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Services.Where(m => !m.IsDeleted).ToListAsync());
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service service)
        {
            if (!ModelState.IsValid) return View();

            Service newService = new Service
            {
                Icon = service.Icon,
                Name = service.Name,
                Desc = service.Desc
            };

            await _context.Services.AddAsync(newService);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            Service service = await GetByIdAsync((int)id);

            if (service == null) return NotFound();

            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Service service)
        {
            if (id is null) return BadRequest();

            var dbService = await GetByIdAsync((int)id);

            if (dbService == null) return NotFound();

            dbService.Icon = service.Icon;
            dbService.Name = service.Name;
            dbService.Desc = service.Desc;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            Service service = await GetByIdAsync((int)id);

            if (service == null) return NotFound();

            return View(service);
        }

        public async Task<IActionResult> Delete(int id)
        {
            Service service = await GetByIdAsync(id);

            if (service == null) return NotFound();


            service.IsDeleted = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        private async Task<Service> GetByIdAsync(int id)
        {
            return await _context.Services.FindAsync(id);
        }
    }
}
