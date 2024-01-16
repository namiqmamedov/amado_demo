using Amado.DAL;
using Amado.Models;
using Amado.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Amado.Areas.admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SubscriberController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SubscriberController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int page)
        {
            IQueryable<Subscriber> subscribers = _context.Subscribers.OrderBy(x => x.ID);

            return View(PageNationList<Subscriber>.Create(subscribers, page, 5));
        }

        [HttpGet]
        public async Task<IActionResult> Read(int? ID)
        {
            if (ID == null) return BadRequest();

            Subscriber subscriber = await _context.Subscribers.FirstOrDefaultAsync(x => x.ID == ID);

            if (subscriber == null) return NotFound();

            return View(subscriber);
        }
    }
}
