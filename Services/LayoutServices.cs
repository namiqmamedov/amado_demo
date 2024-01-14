using Amado.DAL;
using Amado.Interface;
using Amado.Models;
using Amado.ViewModels.Cart;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Amado.Services
{
    public class LayoutServices : ILayoutServices
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LayoutServices(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<CartVM>> GetCart()
        {
            string cart = _httpContextAccessor.HttpContext.Request.Cookies["cart"];

            List<CartVM> cartVMs = null;

            if (!string.IsNullOrWhiteSpace(cart))
            {
                cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(cart);
            }
            else
            {
                cartVMs = new List<CartVM>();
            }

            foreach (CartVM cartVM in cartVMs)
            {
                Product product = await _context.Products.FirstOrDefaultAsync(x => x.ID == cartVM.ProductID);

                cartVM.Title = product.Name;
                cartVM.Image = product.MainImage;
            }

            return cartVMs;
        }
    }
}
