using Creative_blog.Data;
using Creative_blog.Helpers;
using Creative_blog.Helpers.Enums;
using Creative_blog.Models;
using Creative_blog.ViewModels;
using Creative_Blog.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> Index(int page = 1, int take = 4)
        {
            List<OurService> ourServices = await _context.OurServices
                .Where(m => !m.IsDeleted)
                .Skip((page * take) - take)
                .Take(take).ToListAsync();

            List<OurServiceListVM> mapDatas = GetMapDatas(ourServices);

            int count = await GetPageCount(take);

            Paginate<OurServiceListVM> result = new Paginate<OurServiceListVM>(mapDatas,page,count);


            return View(result);

           


        }


        public async Task<int> GetPageCount(int take)
        {
            int productCount = await _context.OurServices.Where(m => !m.IsDeleted).CountAsync();

            return (int)Math.Ceiling((decimal)productCount / take);
        }


        private List<OurServiceListVM> GetMapDatas(List<OurService> ourServices)
        {
            List<OurServiceListVM> ourServiceList = new List<OurServiceListVM>();
            foreach (var ourService in ourServices)
            {
                OurServiceListVM newOurService = new OurServiceListVM
                {
                    Id = ourService.Id,
                    Icon = ourService.Icon,
                    Desc = ourService.Desc,
                    Name = ourService.Name
                };
                ourServiceList.Add(newOurService);
            }
            return ourServiceList;
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
