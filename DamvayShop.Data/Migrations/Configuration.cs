namespace DamvayShop.Data.Migrations
{
    using DamvayShop.Model.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DamvayShop.Data.DamvayShopDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
        protected override void Seed(DamvayShop.Data.DamvayShopDbContext context)
        {
            CreateProductCategorySamble(context);
            CreateFunction(context);
            CreateAdminUser(context);
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }

        private void CreateAdminUser(DamvayShopDbContext dbContext)
        {
            var manager = new UserManager<AppUser>(new UserStore<AppUser>(new DamvayShopDbContext()));
            var roleManager = new RoleManager<AppRole>(new RoleStore<AppRole>(new DamvayShopDbContext()));
            if (manager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    UserName = "ngvanphong",
                    Email = "ngvanphong92@gmail.com",
                    EmailConfirmed = true,
                    BirthDay = DateTime.Now,
                    FullName = "Nguyễn Văn Phong",
                };
                manager.Create(user, "ngvanphong2012");
                if (!roleManager.Roles.Any())
                {
                    roleManager.Create(new AppRole { Name = "Admin", Description = "Admin" });
                    roleManager.Create(new AppRole { Name = "User", Description = "User" });
                };

                var adminUser = manager.FindByEmail("ngvanphong92@gmail.com");
                manager.AddToRoles(adminUser.Id, new string[] { "Admin", "User" });
            }
        }

        private void CreateProductCategorySamble(DamvayShopDbContext dbContext)
        {
            if (dbContext.ProductCategories.Count() == 0)
            {
                List<ProductCategory> listProductCategory = new List<ProductCategory>
            {
                new ProductCategory(){Name="Áo sơ mi", Alias="Ao-so-mi", Status=true },
                new ProductCategory(){Name="Áo thun", Alias="Ao-thun", Status=true },
                new ProductCategory(){Name="Đầm váy", Alias="Dam-vay", Status=true },
            };
                dbContext.ProductCategories.AddRange(listProductCategory);
                dbContext.SaveChanges();
            }
        }

        private void CreateFunction(DamvayShopDbContext context)
        {
            if (context.Functions.Count() == 0)
            {
                context.Functions.AddRange(new List<Function>()
                {
                    new Function() {ID = "SYSTEM", Name = "Hệ thống",ParentId = null,DisplayOrder = 1,Status = true,URL = "/",IconCss = "fa-desktop"  },
                    new Function() {ID = "ROLE", Name = "Nhóm",ParentId = "SYSTEM",DisplayOrder = 1,Status = true,URL = "/main/role/index",IconCss = "fa-home"  },
                    new Function() {ID = "FUNCTION", Name = "Chức năng",ParentId = "SYSTEM",DisplayOrder = 2,Status = true,URL = "/main/function/index",IconCss = "fa-home"  },
                    new Function() {ID = "USER", Name = "Người dùng",ParentId = "SYSTEM",DisplayOrder =3,Status = true,URL = "/main/user/index",IconCss = "fa-home"  },


                    new Function() {ID = "PRODUCT",Name = "Sản phẩm",ParentId = null,DisplayOrder = 2,Status = true,URL = "/",IconCss = "fa-chevron-down"  },
                    new Function() {ID = "PRODUCT_CATEGORY",Name = "Danh mục",ParentId = "PRODUCT",DisplayOrder =1,Status = true,URL = "/main/product-category/index",IconCss = "fa-chevron-down"  },
                    new Function() {ID = "PRODUCT_LIST",Name = "Sản phẩm",ParentId = "PRODUCT",DisplayOrder = 2,Status = true,URL = "/main/product/index",IconCss = "fa-chevron-down"  },
                    new Function() {ID = "ORDER",Name = "Hóa đơn",ParentId = "PRODUCT",DisplayOrder = 3,Status = true,URL = "/main/order/index",IconCss = "fa-chevron-down"  },

                    new Function() {ID = "CONTENT",Name = "Nội dung",ParentId = null,DisplayOrder = 3,Status = true,URL = "/",IconCss = "fa-table"  },
                    new Function() {ID = "POST_CATEGORY",Name = "Danh mục",ParentId = "CONTENT",DisplayOrder = 1,Status = true,URL = "/main/post-category/index",IconCss = "fa-table"  },
                    new Function() {ID = "POST",Name = "Bài viết",ParentId = "CONTENT",DisplayOrder = 2,Status = true,URL = "/main/post/index",IconCss = "fa-table"  },
                     new Function() {ID = "SLIDE",Name = "Slide",ParentId = "CONTENT",DisplayOrder = 3,Status = true,URL = "/main/slide/index",IconCss = "fa-table"  },


                    new Function() {ID = "UTILITY",Name = "Tiện ích",ParentId = null,DisplayOrder = 4,Status = true,URL = "/",IconCss = "fa-clone"  },
                    new Function() {ID = "FOOTER",Name = "Footer",ParentId = "UTILITY",DisplayOrder = 1,Status = true,URL = "/main/footer/index",IconCss = "fa-clone"  },
                    new Function() {ID = "CONTACT",Name = "Liên hệ",ParentId = "UTILITY",DisplayOrder = 2,Status = true,URL = "/main/contact/index",IconCss = "fa-clone"  },
                     new Function() {ID = "TAG",Name = "Tag",ParentId = "UTILITY",DisplayOrder = 3,Status = true,URL = "/main/tag/index",IconCss = "fa-clone"  },
                     new Function() {ID = "SIZE",Name = "Size",ParentId = "UTILITY",DisplayOrder = 4,Status = true,URL = "/main/size/index",IconCss = "fa-clone"  },
                      new Function() {ID = "SYSTEMCONFIG",Name = "Systemconfig",ParentId = "UTILITY",DisplayOrder = 5,Status = true,URL = "/main/systemconfig/index",IconCss = "fa-clone"  },
                });
                context.SaveChanges();
            }
        }
    }
}
