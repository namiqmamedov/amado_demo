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

    public class ColorController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ColorController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int page)
        {
            IQueryable<Color> colors = _context.Colors.OrderBy(x => x.ID);

            return View(PageNationList<Color>.Create(colors, page, 5));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Color color)
        {
            if (!ModelState.IsValid) return View();

            color.Name = color.Name.Trim();

            await _context.Colors.AddAsync(color);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? ID)
        {
            if (ID == null) return BadRequest();

            Color color = await _context.Colors.FirstOrDefaultAsync(x => x.ID == ID);

            if (color == null) return NotFound();

            return View(color);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? ID, Color color)
        {
            if (ID == null) return BadRequest();

            Color colors  = await _context.Colors.FirstOrDefaultAsync(x => x.ID == ID);

            colors.Name = color.Name.Trim();

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? ID, int page)
        {
            IQueryable<Color> colors = _context.Colors.OrderByDescending(x => x.ID == ID);

            if (ID == null) return BadRequest();

            if (colors == null) return NotFound();

            Color color = await _context.Colors.FirstOrDefaultAsync(x => x.ID == ID);

            if (color == null) return NotFound();

            _context.Colors.Remove(color);

            await _context.SaveChangesAsync();

            return PartialView("_ColorIndexPartial", PageNationList<Color>.Create(colors, page, 5));
        }

        [HttpGet]
        public async Task<IActionResult> Read(int? ID)
        {
            if (ID == null) return BadRequest();

            Color color  = await _context.Colors.FirstOrDefaultAsync(x => x.ID == ID);

            if (color == null) return NotFound();

            return View(color);
        }
    }
}
