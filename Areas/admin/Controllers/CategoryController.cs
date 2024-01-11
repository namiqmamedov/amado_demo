using Amado.DAL;
using Amado.Models;
using Amado.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Amado.Areas.admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoryController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int page)
        {
            IQueryable<Category> categories = _context.Categories.OrderBy(x => x.ID);

            return View(PageNationList<Category>.Create(categories, page, 5));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid) return View();

            category.Name = category.Name.Trim();

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? ID)
        {
            if (ID == null) return BadRequest();

            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.ID == ID);

            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? ID,Category category)
        {
            if (ID == null) return BadRequest();

            Category categories = await _context.Categories.FirstOrDefaultAsync(x => x.ID == ID);

            categories.Name = category.Name.Trim();

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? ID,int page)
        {
            IQueryable<Category> categories = _context.Categories.OrderByDescending(x => x.ID == ID);

            if (ID == null) return BadRequest();

            if (categories == null) return NotFound();

            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.ID == ID);

            if (category == null) return NotFound();

            _context.Categories.Remove(category);   

            await _context.SaveChangesAsync();  

            return PartialView("_CategoryIndexPartial",PageNationList<Category>.Create(categories,page,5));
        }

        [HttpGet]
        public async Task<IActionResult> Read(int? ID)
        {
            if(ID == null) return BadRequest();

            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.ID == ID);

            if (category == null) return NotFound();

            return View(category);
        }
    }
}
