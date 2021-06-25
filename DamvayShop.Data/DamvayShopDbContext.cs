using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DamvayShop.Model.Models;

namespace DamvayShop.Data
{
    public class DamvayShopDbContext : IdentityDbContext<AppUser>
    {
        public DamvayShopDbContext() : base("DamvayShopConnectionString")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<Footer> Footers { set; get; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
        public DbSet<PostCategory> PostCategories { get; set; }
        public DbSet<PostTag> PostTags { get; set; }

        public DbSet<Size> Sizes { set; get; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<ProductImage> ProductImages { set; get; }
        public DbSet<ProductQuantity> ProductQuantities { set; get; }

        public DbSet<SystemConfig> SystemConfigs { set; get; }

        public DbSet<Slide> Slides { get; set; }
        public DbSet<SupportOnline> SupportOnlines { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Error> Errors { get; set; }

        public DbSet<Function> Functions { set; get; }
        public DbSet<Permission> Permissions { set; get; }
        public DbSet<AppRole> AppRoles { set; get; }
        public DbSet<IdentityUserRole> UserRoles { set; get; }

        public DbSet<OrderUserAnnoucement> OrderUserAnnoucements { set; get; }

        public static DamvayShopDbContext Create()
        {
            return new DamvayShopDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasKey<string>(r => r.Id).ToTable("AppRoles");
            builder.Entity<IdentityUserRole>().HasKey(i => new { i.UserId, i.RoleId }).ToTable("AppUserRoles");
            builder.Entity<IdentityUserLogin>().HasKey(i => i.UserId).ToTable("AppUserLogins");
            builder.Entity<IdentityUserClaim>().HasKey(i => i.UserId).ToTable("AppUserClaims");
            builder.Entity<Product>().Property(Product => Product.Price).HasPrecision(12, 3);
            builder.Entity<Product>().Property(Product => Product.PromotionPrice).HasPrecision(12, 3);
            builder.Entity<Order>().Property(Order => Order.TotalPayment).HasPrecision(12, 3);

        }

    }
}
