using Creative_blog.Data;
using Creative_blog.Models;
using Creative_blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Creative_blog.Controllers
{
    public class SendMessageController : Controller
    {
        private readonly AppDbContext _context;
        public SendMessageController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(TouchMessage touchMessage)
        {
            if (!ModelState.IsValid) return View();

            TouchMessage newTouchMessage = new TouchMessage
            {
                Email = touchMessage.Email,
                Fullname = touchMessage.Fullname,
                Message = touchMessage.Message,
                Date = DateTime.Now
            };

            await _context.TouchMessages.AddAsync(newTouchMessage);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
