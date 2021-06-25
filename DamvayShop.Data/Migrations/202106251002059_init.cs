namespace DamvayShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Description = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AppUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        AppUser_Id = c.String(maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AppUsers", t => t.AppUser_Id)
                .ForeignKey("dbo.AppRoles", t => t.IdentityRole_Id)
                .Index(t => t.AppUser_Id)
                .Index(t => t.IdentityRole_Id);
            
            CreateTable(
                "dbo.Erros",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        StackTrace = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Footers",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        Content = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Functions",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50, unicode: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        URL = c.String(nullable: false, maxLength: 256),
                        DisplayOrder = c.Int(nullable: false),
                        ParentId = c.String(maxLength: 50, unicode: false),
                        Status = c.Boolean(nullable: false),
                        IconCss = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Functions", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        OrderID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                        SizeId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.OrderID, t.ProductID, t.SizeId })
                .ForeignKey("dbo.Order", t => t.OrderID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .ForeignKey("dbo.Sizes", t => t.SizeId, cascadeDelete: true)
                .Index(t => t.OrderID)
                .Index(t => t.ProductID)
                .Index(t => t.SizeId);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(nullable: false, maxLength: 256),
                        CustomerAddress = c.String(nullable: false, maxLength: 256),
                        CustomerMobile = c.String(nullable: false, maxLength: 256),
                        CustomerEmail = c.String(maxLength: 256),
                        CustomerMessage = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        CreateBy = c.String(maxLength: 256),
                        PaymentMethod = c.String(maxLength: 256),
                        PaymentStatus = c.String(maxLength: 256),
                        Status = c.Boolean(nullable: false),
                        CustomerId = c.String(maxLength: 128),
                        TotalPayment = c.Decimal(precision: 12, scale: 3),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Alias = c.String(nullable: false, maxLength: 50, unicode: false),
                        CategoryID = c.Int(nullable: false),
                        DisplayOrder = c.Int(),
                        ThumbnailImage = c.String(maxLength: 256),
                        Price = c.Decimal(nullable: false, precision: 12, scale: 3),
                        PromotionPrice = c.Decimal(precision: 12, scale: 3),
                        Warranty = c.Int(),
                        Description = c.String(maxLength: 500),
                        Content = c.String(),
                        HomeFlag = c.Boolean(),
                        HotFlag = c.Boolean(),
                        ViewCount = c.Int(),
                        Tags = c.String(maxLength: 256),
                        MetaKeyword = c.String(maxLength: 250),
                        MetaDiscription = c.String(maxLength: 250),
                        CreateDate = c.DateTime(),
                        CreateBy = c.String(maxLength: 250),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 250),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ProductCategories", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.ProductCategories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Alias = c.String(nullable: false, maxLength: 50, unicode: false),
                        ParentID = c.Int(),
                        Description = c.String(maxLength: 256),
                        DisplayOrder = c.Int(),
                        Image = c.String(maxLength: 256),
                        HomeFlag = c.Boolean(),
                        HomeOrder = c.Int(),
                        MetaKeyword = c.String(maxLength: 250),
                        MetaDiscription = c.String(maxLength: 250),
                        CreateDate = c.DateTime(),
                        CreateBy = c.String(maxLength: 250),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 250),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProductTags",
                c => new
                    {
                        ProductID = c.Int(nullable: false),
                        TagID = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => new { t.ProductID, t.TagID })
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.TagID);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 256),
                        Type = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PostTags",
                c => new
                    {
                        PostID = c.String(nullable: false, maxLength: 128),
                        TagID = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => new { t.PostID, t.TagID })
                .ForeignKey("dbo.Posts", t => t.PostID, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagID, cascadeDelete: true)
                .Index(t => t.PostID)
                .Index(t => t.TagID);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 50),
                        Alias = c.String(nullable: false, maxLength: 50, unicode: false),
                        CategoryID = c.Int(nullable: false),
                        DisplayOrder = c.Int(),
                        Description = c.String(maxLength: 256),
                        Image = c.String(maxLength: 256),
                        Content = c.String(),
                        HomeFlag = c.Boolean(),
                        ViewCount = c.Int(),
                        Tags = c.String(maxLength: 256),
                        MetaKeyword = c.String(maxLength: 250),
                        MetaDiscription = c.String(maxLength: 250),
                        CreateDate = c.DateTime(),
                        CreateBy = c.String(maxLength: 250),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 250),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PostCategories", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.PostCategories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Alias = c.String(nullable: false, maxLength: 50, unicode: false),
                        ParentID = c.Int(),
                        Description = c.String(maxLength: 256),
                        DisplayOrder = c.Int(),
                        Image = c.String(maxLength: 256),
                        HomeFlag = c.Boolean(),
                        HomeOrder = c.Int(),
                        MetaKeyword = c.String(maxLength: 250),
                        MetaDiscription = c.String(maxLength: 250),
                        CreateDate = c.DateTime(),
                        CreateBy = c.String(maxLength: 250),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 250),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Sizes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OrderUserAnnoucements",
                c => new
                    {
                        OrderId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        HasRead = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.OrderId, t.UserId })
                .ForeignKey("dbo.AppUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Order", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AppUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(maxLength: 256),
                        Address = c.String(maxLength: 256),
                        Avatar = c.String(),
                        BirthDay = c.DateTime(),
                        Status = c.Boolean(),
                        Gender = c.Boolean(),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AppUserClaims",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Id = c.Int(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        AppUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AppUsers", t => t.AppUser_Id)
                .Index(t => t.AppUser_Id);
            
            CreateTable(
                "dbo.AppUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        AppUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AppUsers", t => t.AppUser_Id)
                .Index(t => t.AppUser_Id);
            
            CreateTable(
                "dbo.Pages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Alias = c.String(nullable: false, maxLength: 256, unicode: false),
                        Content = c.String(nullable: false),
                        MetaKeyword = c.String(maxLength: 50),
                        MetaDescription = c.String(maxLength: 256),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RoleId = c.String(maxLength: 128),
                        FunctionId = c.String(maxLength: 50, unicode: false),
                        CanCreate = c.Boolean(nullable: false),
                        CanRead = c.Boolean(nullable: false),
                        CanUpdate = c.Boolean(nullable: false),
                        CanDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AppRoles", t => t.RoleId)
                .ForeignKey("dbo.Functions", t => t.FunctionId)
                .Index(t => t.RoleId)
                .Index(t => t.FunctionId);
            
            CreateTable(
                "dbo.PostImages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PostId = c.String(),
                        Path = c.String(maxLength: 250),
                        Caption = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProductImages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Path = c.String(maxLength: 250),
                        Caption = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.ProductQuantities",
                c => new
                    {
                        ProductId = c.Int(nullable: false),
                        SizeId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductId, t.SizeId })
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Sizes", t => t.SizeId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.SizeId);
            
            CreateTable(
                "dbo.Slides",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 256),
                        URL = c.String(nullable: false, maxLength: 256),
                        Image = c.String(maxLength: 256),
                        DisplayOrder = c.Int(),
                        Status = c.Boolean(nullable: false),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SupportOnlines",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Skype = c.String(maxLength: 256),
                        Email = c.String(maxLength: 256),
                        Mobile = c.String(maxLength: 256),
                        Facebook = c.String(maxLength: 256),
                        Status = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SystemConfigs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 50, unicode: false),
                        ValueString = c.String(maxLength: 250),
                        ValueInt = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppUserRoles", "IdentityRole_Id", "dbo.AppRoles");
            DropForeignKey("dbo.ProductQuantities", "SizeId", "dbo.Sizes");
            DropForeignKey("dbo.ProductQuantities", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductImages", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Permissions", "FunctionId", "dbo.Functions");
            DropForeignKey("dbo.Permissions", "RoleId", "dbo.AppRoles");
            DropForeignKey("dbo.OrderUserAnnoucements", "OrderId", "dbo.Order");
            DropForeignKey("dbo.OrderUserAnnoucements", "UserId", "dbo.AppUsers");
            DropForeignKey("dbo.AppUserRoles", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.AppUserLogins", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.AppUserClaims", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.OrderDetails", "SizeId", "dbo.Sizes");
            DropForeignKey("dbo.OrderDetails", "ProductID", "dbo.Products");
            DropForeignKey("dbo.ProductTags", "TagID", "dbo.Tags");
            DropForeignKey("dbo.PostTags", "TagID", "dbo.Tags");
            DropForeignKey("dbo.PostTags", "PostID", "dbo.Posts");
            DropForeignKey("dbo.Posts", "CategoryID", "dbo.PostCategories");
            DropForeignKey("dbo.ProductTags", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Products", "CategoryID", "dbo.ProductCategories");
            DropForeignKey("dbo.OrderDetails", "OrderID", "dbo.Order");
            DropForeignKey("dbo.Functions", "ParentId", "dbo.Functions");
            DropIndex("dbo.ProductQuantities", new[] { "SizeId" });
            DropIndex("dbo.ProductQuantities", new[] { "ProductId" });
            DropIndex("dbo.ProductImages", new[] { "ProductId" });
            DropIndex("dbo.Permissions", new[] { "FunctionId" });
            DropIndex("dbo.Permissions", new[] { "RoleId" });
            DropIndex("dbo.AppUserLogins", new[] { "AppUser_Id" });
            DropIndex("dbo.AppUserClaims", new[] { "AppUser_Id" });
            DropIndex("dbo.OrderUserAnnoucements", new[] { "UserId" });
            DropIndex("dbo.OrderUserAnnoucements", new[] { "OrderId" });
            DropIndex("dbo.Posts", new[] { "CategoryID" });
            DropIndex("dbo.PostTags", new[] { "TagID" });
            DropIndex("dbo.PostTags", new[] { "PostID" });
            DropIndex("dbo.ProductTags", new[] { "TagID" });
            DropIndex("dbo.ProductTags", new[] { "ProductID" });
            DropIndex("dbo.Products", new[] { "CategoryID" });
            DropIndex("dbo.OrderDetails", new[] { "SizeId" });
            DropIndex("dbo.OrderDetails", new[] { "ProductID" });
            DropIndex("dbo.OrderDetails", new[] { "OrderID" });
            DropIndex("dbo.Functions", new[] { "ParentId" });
            DropIndex("dbo.AppUserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.AppUserRoles", new[] { "AppUser_Id" });
            DropTable("dbo.SystemConfigs");
            DropTable("dbo.SupportOnlines");
            DropTable("dbo.Slides");
            DropTable("dbo.ProductQuantities");
            DropTable("dbo.ProductImages");
            DropTable("dbo.PostImages");
            DropTable("dbo.Permissions");
            DropTable("dbo.Pages");
            DropTable("dbo.AppUserLogins");
            DropTable("dbo.AppUserClaims");
            DropTable("dbo.AppUsers");
            DropTable("dbo.OrderUserAnnoucements");
            DropTable("dbo.Sizes");
            DropTable("dbo.PostCategories");
            DropTable("dbo.Posts");
            DropTable("dbo.PostTags");
            DropTable("dbo.Tags");
            DropTable("dbo.ProductTags");
            DropTable("dbo.ProductCategories");
            DropTable("dbo.Products");
            DropTable("dbo.Order");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Functions");
            DropTable("dbo.Footers");
            DropTable("dbo.Erros");
            DropTable("dbo.AppUserRoles");
            DropTable("dbo.AppRoles");
        }
    }
}
