using Amado.DAL;
using Amado.Models;
using Amado.ViewModels.Cart;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Amado.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            string cart = HttpContext.Request.Cookies["cart"];

            List<CartVM> cartVMs = null;

            if(!string.IsNullOrWhiteSpace(cart))
            {
                cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(cart);
            }
            else
            {
                cartVMs = new List<CartVM>();
            }

            return View(await _getCartItemAsync(cartVMs));
        }

        public async Task<IActionResult> GetCart()
        {
            string cart = Request.Cookies["cart"];

            List<CartVM> cartVMs = null;

            if(!string.IsNullOrWhiteSpace(cart))
            {
                cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(cart);
            }
            else
            {
                cartVMs = new List<CartVM>();   
            }

            return PartialView("_CartPartial", await _getCartItemAsync(cartVMs));   
        }

        public async Task<IActionResult> addToCart(int? ID,int? value)
        {
            if (ID == null) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(x => x.ID == ID);

            if (product == null) return NotFound();

            string cart = HttpContext.Request.Cookies["cart"];

            List<CartVM> cartVMs = null;

            if (!string.IsNullOrWhiteSpace(cart)) 
            { 
                cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(cart);
            }

            else
            {
                cartVMs = new List<CartVM>();
            }

            if (value != null && cartVMs.Any(x => x.ProductID == ID))
            {
                cartVMs.First(x => x.ProductID == ID).Count += value.Value;
            }

            if (cartVMs.Exists(x => x.ProductID == ID))
            {
                cartVMs.Find(x => x.ProductID == ID).Count++;
            }

            else
            {
                CartVM cartVM = new CartVM
                {
                    ProductID = product.ID,
                    Count = value ?? 1
                };

                cartVMs.Add(cartVM);
            }

            cart = JsonConvert.SerializeObject(cartVMs);

            HttpContext.Response.Cookies.Append("cart", cart);

            return PartialView("_CartPartial", await _getCartItemAsync(cartVMs));
        }

        public async Task<IActionResult> RemoveCart(int? ID)
        {
            if (ID  == null) return BadRequest();

            if (!await _context.Products.AnyAsync(x => x.ID == ID)) return NotFound();

            string cart = HttpContext.Request.Cookies["cart"];

            if (string.IsNullOrWhiteSpace(cart)) return BadRequest();

            List<CartVM> cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(cart);

            CartVM cartVM = cartVMs.Find(x => x.ProductID == ID);

            if (cartVM == null) return NotFound();

            cartVMs.Remove(cartVM);

            cart = JsonConvert.SerializeObject(cartVMs);

            HttpContext.Response.Cookies.Append("cart", cart);

            return PartialView("_CartIndexPartial", await _getCartItemAsync(cartVMs));
        }

        public async Task<IActionResult> UpdateCount(int? ID, int count)
        {
            if (ID == null) return BadRequest();

            if(!await _context.Products.AnyAsync(x => x.ID == ID))
            {
                return NotFound();
            }

            string cart = HttpContext.Request.Cookies["cart"];

            List<CartVM> cartVMs = null;

            if(!string.IsNullOrWhiteSpace(cart))
            {
                cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(cart);

                CartVM cartVM = cartVMs.FirstOrDefault(x => x.ProductID == ID);

                if (cartVM == null) return NotFound();

                cartVM.Count = count <= 0 ? 0 : count;

                cart = JsonConvert.SerializeObject(cartVMs);

                HttpContext.Response.Cookies.Append("cart", cart);
            }
            else
            {
                return BadRequest();
            }

            return PartialView("_CartIndexPartial", await _getCartItemAsync(cartVMs));
        }

        private async Task<List<CartVM>> _getCartItemAsync(List<CartVM> cartVMs)
        {
            foreach (CartVM item in cartVMs)
            {
                Product product = await _context.Products.FirstOrDefaultAsync(x => x.ID == item.ProductID);

                item.Image = product.MainImage;
                item.Title = product.Name;
                item.Price = product.Price;
            }

            return cartVMs;
        }
    }
}
