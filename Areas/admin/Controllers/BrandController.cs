using Amado.DAL;
using Amado.Models;
using Amado.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Amado.Areas.admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BrandController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BrandController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int page)
        {
            IQueryable<Brand> brands = _context.Brands.OrderBy(x => x.ID);

            return View(PageNationList<Brand>.Create(brands, page, 5));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand brand)
        {
            if (!ModelState.IsValid) return View();

            brand.Name = brand.Name.Trim();

            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? ID)
        {
            if (ID == null) return BadRequest();

            Brand brand = await _context.Brands.FirstOrDefaultAsync(x => x.ID == ID);

            if (brand == null) return NotFound();

            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? ID, Brand brand)
        {
            if (ID == null) return BadRequest();

            Brand brands = await _context.Brands.FirstOrDefaultAsync(x => x.ID == ID);

            brands.Name = brand.Name.Trim();

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? ID, int page)
        {
            IQueryable<Brand> brands = _context.Brands.OrderByDescending(x => x.ID == ID);

            if (ID == null) return BadRequest();

            if (brands == null) return NotFound();

            Brand brand = await _context.Brands.FirstOrDefaultAsync(x => x.ID == ID);

            if (brand == null) return NotFound();

            _context.Brands.Remove(brand);

            await _context.SaveChangesAsync();

            return PartialView("_BrandIndexPartial", PageNationList<Brand>.Create(brands, page, 5));
        }

        [HttpGet]
        public async Task<IActionResult> Read(int? ID)
        {
            if (ID == null) return BadRequest();

            Brand brand = await _context.Brands.FirstOrDefaultAsync(x => x.ID == ID);

            if (brand == null) return NotFound();

            return View(brand);
        }
    }
}
