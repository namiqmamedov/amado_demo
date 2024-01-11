using Amado.DAL;
using Amado.Extension;
using Amado.Helpers;
using Amado.Models;
using Amado.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Amado.Areas.admin.Controllers
{
    [Area("admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int page)
        {
            IQueryable<Product> products =  _context.Products.Include(x => x.Category).Include(x => x.Brand).OrderBy(x=>x.ID);

            return View(PageNationList<Product>.Create(products, page, 5));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Brands = await _context.Brands.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            ViewBag.Description = await _context.Descriptions.Select(x=>x.Text).ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.Brands = await _context.Brands.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            ViewBag.Description = await _context.Descriptions.ToListAsync();


            if (!ModelState.IsValid) return View();

            if(!await _context.Brands.AnyAsync(x => x.ID == product.BrandID))
            {
                ModelState.AddModelError("BrandID", "Select a correct brand");
                
                return View();
            }

            if(product?.CategoryID == null)
            {
                ModelState.AddModelError("BrandID", "Select a correct brand");

                return View();
            }

            if (!await _context.Categories.AnyAsync(x => x.ID == product.CategoryID))
            {
                ModelState.AddModelError("CategoryID", "Select a correct category");

                return View();
            }

            if(product.ColorIDs.Count() != product.Counts.Count())
            {
                ModelState.AddModelError("", "Select a correct list");

                return View();
            }

            List<ProductColor> productColors = new List<ProductColor>();

            for (int i = 0; i < product.ColorIDs.Count(); i++)
            {

                if (!await _context.Colors.AnyAsync(c => c.ID == product.ColorIDs[i]))
                {
                    ModelState.AddModelError("", $"This color id {product.ColorIDs[i]} is incorrect`");

                    return View();
                }

                if (product.Counts[i] <= 0)
                {
                    ModelState.AddModelError("", $"Count is incorrect");

                    return View();
                }

                ProductColor productColor = new ProductColor
                {
                    ColorID = product.ColorIDs[i],
                    Count = product.Counts[i],
                };

                productColors.Add(productColor);
            }
            product.ProductColors = productColors;


            if (product.Files != null && product.Files.Count() > 4)
            {
                ModelState.AddModelError("Files ", "You can select a maximum 5 images");

                return View();
            }

            if(product.MainFile != null)
            {
                if (!product.MainFile.CheckFileType())
                {
                    ModelState.AddModelError("File", "File type should be jpg or png.");
                    return View(product);
                }


                if (!product.MainFile.CheckFileSize(20000))
                {
                    ModelState.AddModelError("MainFile", "The maximum size should be 20mb!");
                    return View(product);
                }
                product.MainImage = product.MainFile.CreateImage(_env, "assets", "img", "products");
            }
            else
            {
                ModelState.AddModelError("MainFile", "Images is required");
                return View();
            }
            if (product.HoverFile != null)
            {
                if (!product.HoverFile.CheckFileType())
                {
                    ModelState.AddModelError("HoverFile", "File type should be jpg or png.");
                    return View(product);
                }


                if (!product.HoverFile.CheckFileSize(20000))
                {
                    ModelState.AddModelError("HoverFile", "The maximum size should be 20mb!");
                    return View(product);
                }
                product.HoverImage = product.HoverFile.CreateImage(_env, "assets", "img", "products");
            }
            else
            {
                ModelState.AddModelError("HoverFile", "Images is required");
                return View();
            }
            
            if(product.Files != null && product.Files.Count() > 0)
            {
                List<ProductImage> productImages = new List<ProductImage>();
                foreach (IFormFile file in product.Files)
                {
                    if (!file.CheckFileType())
                    {
                        ModelState.AddModelError("Files", "File type should be jpg or png.");
                        return View(product);
                    }

                    if (!file.CheckFileSize(20000))
                    {
                        ModelState.AddModelError("Files", "The maximum size should be 20mb!");
                        return View();
                    }

                    ProductImage productImage = new ProductImage
                    {
                        Image =  file.CreateImage(_env, "assets", "img", "products")
                    };

                    productImages.Add(productImage);
                }
                product.ProductImages = productImages;
            }

            if (product.ProductDescription != null)
            {
                List<ProductDescription> productDescriptions = new List<ProductDescription>();
                for (int i = 0; i < product.ProductDescription.Count; i++)
                {
                    ProductDescription productDescription = new ProductDescription
                    {
                        Text = product.ProductDescription[i],
                    };
                    productDescriptions.Add(productDescription);
                }
                product.Description = productDescriptions;
            }

            product.Name = product.Name.Trim();
            product.Price = product.Price;
            product.Count = productColors.Sum(x => x.Count);

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? ID)
        {
            ViewBag.Brands = await _context.Brands.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            ViewBag.Description = await _context.Descriptions.ToListAsync();

            if (ID == null) return BadRequest();

            Product product = await _context.Products.Include(x => x.Category).Include(x => x.Brand).Include(x => x.ProductImages).Include(x=>x.ProductColors).ThenInclude(x=>x.Color).Include(x=>x.Description).FirstOrDefaultAsync(x => x.ID == ID);

            if (product == null) return NotFound();

            product.ColorIDs = await _context.ProductColors.Where(x => x.ProductID == ID).Select(x => x.ColorID).ToListAsync();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? ID,Product product)
        {
            ViewBag.Brands = await _context.Brands.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();

            if (ID == null) return BadRequest();

            Product products = await _context.Products.Include(x=>x.Category).Include(x=>x.Brand).Include(x=>x.ProductImages).Include(x=>x.ProductColors).ThenInclude(x=>x.Color).Include(x=>x.Description).FirstOrDefaultAsync(x => x.ID == ID);

            if(products == null) return NotFound();

            if (!await _context.Brands.AnyAsync(x => x.ID == product.BrandID))
            {
                ModelState.AddModelError("BrandID", "Select a correct brand");

                return View();
            }
            if (product?.CategoryID == null)
            {
                ModelState.AddModelError("BrandID", "Select a correct brand");

                return View();
            }

            if (!await _context.Categories.AnyAsync(x => x.ID == product.CategoryID))
            {
                ModelState.AddModelError("CategoryID", "Select a correct category");

                return View();
            }

            if (product.ColorIDs.Count() != product.Counts.Count())
            {
                ModelState.AddModelError("", "Select a correct list");

                return View();
            }
            
            List<ProductColor> productColors = new List<ProductColor>();
            
            if(product.ColorIDs != null)
            {
                _context.ProductColors.RemoveRange(products.ProductColors);

                for (int i = 0; i < product.ColorIDs.Count(); i++)
                {
                    if (!await _context.Colors.AnyAsync(c => c.ID == product.ColorIDs[i]))
                    {
                        ModelState.AddModelError("", $"This color id {product.ColorIDs[i]} is incorrect`");

                        return View();
                    }

                    if (product.Counts[i] <= 0)
                    {
                        ModelState.AddModelError("", $"Count is incorrect");

                        return View();
                    }

                    ProductColor productColor = new ProductColor
                    {
                        ColorID = product.ColorIDs[i],
                        Count = product.Counts[i],
                    };

                    productColors.Add(productColor);
                }
                product.ProductColors = productColors;
                products.ProductColors = productColors;
            }

            if (product.ProductDescription != null)
            {
                _context.Descriptions.RemoveRange(products.Description);

                List<ProductDescription> productDescriptions = new List<ProductDescription>();

                for (int i = 0; i < product.ProductDescription.Count; i++)
                {
                    if (product.ProductDescription[i] != null)
                    {
                        ProductDescription productDescription = new ProductDescription
                        {
                            Text = product.ProductDescription[i],
                        };
                        productDescriptions.Add(productDescription);
                    }
                }
                product.Description = productDescriptions;
                products.Description = product.Description;
            }

            if (product.MainFile != null)
            {
                if (!product.MainFile.CheckFileType())
                {
                    ModelState.AddModelError("MainFile", "File type should be jpg or png.");
                    return View(product);
                }


                if (!product.MainFile.CheckFileSize(20000))
                {
                    ModelState.AddModelError("MainFile", "The maximum size should be 20mb!");
                    return View(product);
                }

                FileHelper.DeleteFile(_env, products.MainImage, "assets", "img", "products");

                products.MainImage = product.MainFile.CreateImage(_env, "assets", "img", "products");
            }

            if (product.HoverFile != null)
            {
                if (!product.HoverFile.CheckFileType())
                {
                    ModelState.AddModelError("HoverFile", "File type should be jpg or png.");
                    return View(product);
                }


                if (!product.HoverFile.CheckFileSize(20000))
                {
                    ModelState.AddModelError("HoverFile", "The maximum size should be 20mb!");
                    return View(product);
                }

                FileHelper.DeleteFile(_env, products.HoverImage, "assets", "img", "products");

                products.HoverImage = product.HoverFile.CreateImage(_env, "assets", "img", "products");
            }

            if (product.Files != null && product.Files.Count() > 0)
            {
                List<ProductImage> productImages = new List<ProductImage>();

                foreach (IFormFile file in product.Files)
                {
                    if (!file.CheckFileType())
                    {
                        ModelState.AddModelError("Files", "File type should be jpg or png.");
                        return View();
                    }

                    if (!file.CheckFileSize(20000))
                    {
                        ModelState.AddModelError("Files", "The maximum size should be 20mb!");
                        return View();
                    }

                    if (product.Files.Count() > 4)
                    {
                        ModelState.AddModelError("Files", "You cannot select more than 4 images");
                        return View();
                    }

                    ProductImage productImage = new ProductImage
                    {
                        Image = file.CreateImage(_env, "assets", "img", "products")
                    };

                    productImages.Add(productImage);
                }
                products.ProductImages.AddRange(productImages);
            }

            products.Name = product.Name.Trim();
            products.Price = product.Price;
            products.Count= product.Count;
            products.CategoryID = product.CategoryID;
            products.BrandID = product.BrandID;
            products.Count = productColors.Sum(x => x.Count);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> RemoveImg(int? ID)
        {
            if (ID == null) return BadRequest();

            ProductImage productImage = await _context.ProductImages.Include(x => x.Product).FirstOrDefaultAsync(x => x.ID == ID);

            if (productImage == null) return NotFound();

            Product product = await _context.Products.Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.ID == productImage.ProductID);

            _context.ProductImages.Remove(productImage);

            await _context.SaveChangesAsync();

            Helpers.FileHelper.DeleteFile(_env, productImage.Image);

            return PartialView("_ProductImagePartial", product.ProductImages);
        }

        public async Task<IActionResult> Delete(int? ID,int page)
        {
            IQueryable<Product> products = _context.Products.OrderByDescending(x=>x.ID == ID);

            if (ID == null) return BadRequest();

            if (products == null) return NotFound();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.ID == ID);

            if(product == null) return NotFound();  

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return PartialView("_ProductIndexPartial", PageNationList<Product>.Create(products, page, 5));
        }

        [HttpGet]
        public async Task<IActionResult> Read(int? ID)
        {
            ViewBag.Brands = await _context.Brands.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            ViewBag.Description = await _context.Descriptions.ToListAsync();

            if (ID == null) return BadRequest();

            Product product = await _context.Products.Include(x => x.Category).Include(x => x.Brand).Include(x => x.ProductImages).Include(x => x.ProductColors).ThenInclude(x => x.Color).Include(x => x.Description).FirstOrDefaultAsync(x => x.ID == ID);

            if (product == null) return NotFound();

            product.ColorIDs = await _context.ProductColors.Where(x => x.ProductID == ID).Select(x => x.ColorID).ToListAsync();

            return View(product);
        }
    }
}
