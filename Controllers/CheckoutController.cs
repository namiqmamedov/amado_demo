using Amado.DAL;
using Amado.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Amado.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly AppDbContext _context;

        public CheckoutController(AppDbContext context)
        {
            _context = context;
        }
        public async  Task<IActionResult> Index()
        {
            ViewBag.Country = await _context.Countries.ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Checkout checkout)
        {
            if (!ModelState.IsValid) return BadRequest();

            if (checkout == null) return NotFound();

            checkout.FirstName = checkout.FirstName.Trim();
            checkout.LastName= checkout.LastName.Trim();
            checkout.CompanyName= checkout.CompanyName.Trim();
            checkout.Email = checkout.Email.Trim();
            checkout.Address = checkout.Address.Trim();
            checkout.Town = checkout.Town.Trim();
            checkout.Zip = checkout.Zip.Trim();
            checkout.Phone = checkout.Phone.Trim();
            checkout.Comment = checkout.Comment.Trim();
            checkout.CreatedAt = DateTime.UtcNow;

            checkout.CountryID = checkout.CountryID;

            await _context.Checkouts.AddAsync(checkout);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index","Shop");
        }
    }
}
