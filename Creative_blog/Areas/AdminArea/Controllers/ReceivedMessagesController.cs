using Creative_blog.Data;
using Creative_blog.Models;
using Creative_blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Creative_blog.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class ReceivedMessagesController : Controller
	{
        private readonly AppDbContext _context;
        public ReceivedMessagesController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
		{
            List<TouchMessage> touchMessages = _context.TouchMessages.ToList();
            HomeVM home = new HomeVM
            {

                TouchMessages = touchMessages
            };
            return View(home);
		}

        public async Task<IActionResult> Delete(int id)
        {
            TouchMessage touchMessage = await GetByIdAsync(id);

            if (touchMessage == null) return NotFound();


            touchMessage.IsDeleted = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        private async Task<TouchMessage> GetByIdAsync(int id)
        {
            return await _context.TouchMessages.FindAsync(id);
        }
    }
}
