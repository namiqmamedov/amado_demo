using Amado.DAL;
using Amado.Models;
using Amado.ViewModels;
using Amado.ViewModels.Shops;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace Amado.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;

        public ShopController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int page)
        {
            IQueryable<Product> products = _context.Products.Include(x => x.Category).Include(x => x.Brand).OrderBy(x => x.ID);

            var pagedList = PageNationList<Product>.Create(products, page, 5);

            ViewBag.Brands = _context.Brands.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Colors = _context.Colors.ToList();

            return View(pagedList);
        }

        [HttpPost]
        public async Task<IActionResult> FilterProducts(int? selectedBrand, int? selectedCategory, int? selectedColor, int? minPrice, int? maxPrice,int? pageSize,int? selectedDate, int? page)
        {
            var filteredProducts = _context.Products.AsQueryable();

            int currentPage = page ?? 1;

            var filteredColors = await _context.ProductColors.ToListAsync();

            if (selectedBrand.HasValue)
            {
                filteredProducts = filteredProducts.Where(p => p.BrandID == selectedBrand);
            }

            if (selectedCategory.HasValue)
            {
                filteredProducts = filteredProducts.Where(p => p.CategoryID == selectedCategory);
            }

            if (pageSize.HasValue)
            {
                int pageSizeMultiplier = pageSize.Value + 1;
                int itemsToShow = pageSizeMultiplier * 3;

                filteredProducts = filteredProducts.Take(itemsToShow);
            }

            if (selectedDate.HasValue)
            {
                if (selectedDate == 0)
                {
                    filteredProducts = filteredProducts.OrderByDescending(x => x.ID);
                }
                else if (selectedDate == 1)
                {
                    filteredProducts = filteredProducts.OrderBy(x => x.ID);
                }
            }

            // filteredProducts değişkenini kullanarak devam edin...



            if (selectedColor.HasValue)
            {
                var productIdsWithSelectedColor = _context.ProductColors
                    .Where(pc => pc.ColorID == selectedColor)
                    .Select(pc => pc.ProductID)
                    .ToList();

                filteredProducts = filteredProducts.Where(p => productIdsWithSelectedColor.Contains(p.ID));
            }

            if (minPrice.HasValue && maxPrice.HasValue)
            {
                filteredProducts = filteredProducts.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
            }

            var result = await filteredProducts.ToListAsync();

            var resultQueryable = result.AsQueryable();

            return PartialView("_FilteredProductPartial", PageNationList<Product>.Create(resultQueryable, currentPage, 5));
        }

    }
}
