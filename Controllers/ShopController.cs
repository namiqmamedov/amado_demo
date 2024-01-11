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
        public async Task<IActionResult> Index(int? page)
        {
            ShopVM shopVM = new ShopVM
            {
                Brands = await _context.Brands.ToListAsync(),
                Categories = await _context.Categories.ToListAsync(),
                Colors = await _context.Colors.ToListAsync(),
                Products = await _context.Products.ToListAsync(),
                Productss = _context.Products.OrderBy(x=>x.ID)
            };

            return View(shopVM);
        }

        [HttpPost]
        public async Task<IActionResult> FilterProducts(int? selectedBrand, int? selectedCategory,int? selectedColor,int? minPrice, int? maxPrice)
        {
            var filteredProducts =  _context.Products.AsQueryable();

            var filteredColors = await _context.ProductColors.ToListAsync();


            if (selectedBrand.HasValue)
            {
                filteredProducts = filteredProducts.Where(p => p.BrandID == selectedBrand);
            }

            if (selectedCategory.HasValue)
            {
                filteredProducts = filteredProducts.Where(p => p.CategoryID == selectedCategory);
            }

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

            return PartialView("_FilteredProductPartial", result);
        }


    }


}
