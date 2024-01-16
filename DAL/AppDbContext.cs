﻿using Amado.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Amado.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductDescription> Descriptions { get; set; }
        public DbSet<Checkout> Checkouts { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<Setting> Settings { get; set; }
    }
}
