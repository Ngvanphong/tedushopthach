using System;
using System.Globalization;
using DamvayShop.Model.Models;
using DamvayShop.Web.Models;

namespace DamvayShop.Web.Infrastructure.Extensions
{
    public static class EntityEntensions
    {
        public static void UpdatePost(this Post post, PostViewModel postVm)
        {
            post.ID = postVm.ID;
            post.Name = postVm.Name;
            post.Alias = postVm.Alias;
            post.CategoryID = postVm.CategoryID;
            post.DisplayOrder = postVm.DisplayOrder;
            post.Description = postVm.Description;
            post.Content = postVm.Content;
            post.Image = postVm.Image;
            post.HomeFlag = postVm.HomeFlag;
            post.ViewCount = postVm.ViewCount;
            post.MetaKeyword = postVm.MetaKeyword;
            post.MetaDiscription = postVm.MetaDiscription;
            post.CreateDate = postVm.CreateDate;
            post.CreateBy = postVm.CreateBy;
            post.UpdatedBy = postVm.UpdatedBy;
            post.UpdatedDate = postVm.UpdatedDate;
            post.Status = postVm.Status;
            post.Tags = postVm.Tags;
          
        }

        public static void UpdatePostCategory(this PostCategory postCategory, PostCategoryViewModel postCategoryVm)
        {
            postCategory.ID = postCategoryVm.ID;
            postCategory.Name = postCategoryVm.Name;
            postCategory.Alias = postCategoryVm.Alias;
            postCategory.ParentID = postCategoryVm.ParentID;
            postCategory.DisplayOrder = postCategoryVm.DisplayOrder;
            postCategory.Description = postCategoryVm.Description;
            postCategory.DisplayOrder = postCategoryVm.DisplayOrder;
            postCategory.Image = postCategoryVm.Image;
            postCategory.HomeFlag = postCategoryVm.HomeFlag;
            postCategory.MetaKeyword = postCategoryVm.MetaKeyword;
            postCategory.MetaDiscription = postCategoryVm.MetaDiscription;
            postCategory.CreateDate = postCategoryVm.CreateDate;
            postCategory.CreateBy = postCategoryVm.CreateBy;
            postCategory.UpdatedBy = postCategoryVm.UpdatedBy;
            postCategory.UpdatedDate = postCategoryVm.UpdatedDate;
            postCategory.Status = postCategoryVm.Status;
            postCategory.HomeOrder = postCategoryVm.HomeOrder;
           
        }
        public static void UpdatePostImage(this PostImage postImage, PostImageViewModel postImageVm)
        {
            postImage.Path = postImageVm.Path;
            postImage.PostId = postImageVm.PostId;
            postImage.Caption = postImageVm.Caption;
        }

        public static void UpdateProduct(this Product product, ProductViewModel productVm)
        {

            product.ID = productVm.ID;
            product.Name = productVm.Name;
            product.Alias = productVm.Alias;
            product.CategoryID = productVm.CategoryID;
            product.DisplayOrder = productVm.DisplayOrder;
            product.Description = productVm.Description;
            product.Content = productVm.Content;           
            product.HomeFlag = productVm.HomeFlag;
            product.HotFlag = productVm.HotFlag;
            product.ViewCount = productVm.ViewCount;
            product.MetaKeyword = productVm.MetaKeyword;
            product.MetaDiscription = productVm.MetaDiscription;
            product.CreateDate = productVm.CreateDate;
            product.CreateBy = productVm.CreateBy;
            product.UpdatedBy = productVm.UpdatedBy;
            product.UpdatedDate = productVm.UpdatedDate;
            product.Status = productVm.Status;
            product.ThumbnailImage = productVm.ThumbnailImage;
            product.Price = productVm.Price;
            product.PromotionPrice = productVm.PromotionPrice;
            product.Warranty = productVm.Warranty;
            product.Tags = productVm.Tags;

        }
        public static void UpdateProductCategory(this ProductCategory productCategory, ProductCategoryViewModel productCategoryVm)
        {
            productCategory.ID = productCategoryVm.ID;
            productCategory.Name = productCategoryVm.Name;
            productCategory.Alias = productCategoryVm.Alias;
            productCategory.ParentID = productCategoryVm.ParentID;
            productCategory.DisplayOrder = productCategoryVm.DisplayOrder;
            productCategory.Description = productCategoryVm.Description;
            productCategory.DisplayOrder = productCategoryVm.DisplayOrder;
            productCategory.Image = productCategoryVm.Image;
            productCategory.HomeFlag = productCategoryVm.HomeFlag;
            productCategory.MetaKeyword = productCategoryVm.MetaKeyword;
            productCategory.MetaDiscription = productCategoryVm.MetaDiscription;
            productCategory.CreateDate = productCategoryVm.CreateDate;
            productCategory.CreateBy = productCategoryVm.CreateBy;
            productCategory.UpdatedBy = productCategoryVm.UpdatedBy;
            productCategory.UpdatedDate = productCategoryVm.UpdatedDate;
            productCategory.Status = productCategoryVm.Status;
            productCategory.HomeOrder = productCategoryVm.HomeOrder;
        }
        public static void UpdateFunction(this Function function, FunctionViewModel functionVm)
        {
            function.Name = functionVm.Name;
            function.DisplayOrder = functionVm.DisplayOrder;
            function.IconCss = functionVm.IconCss;
            function.Status = functionVm.Status;
            function.ParentId = functionVm.ParentId;
            function.Status = functionVm.Status;
            function.URL = functionVm.URL;
            function.ID = functionVm.ID;
        }
        public static void UpdateApplicationRole(this AppRole appRole, ApplicationRoleViewModel appRoleViewModel, string action = "add")
        {
            if (action == "update")
                appRole.Id = appRoleViewModel.Id;
            else
                appRole.Id = Guid.NewGuid().ToString();
            appRole.Name = appRoleViewModel.Name;
            appRole.Description = appRoleViewModel.Description;
        }

        public static void UpdatePermission(this Permission permission, PermissionViewModel permissionVm)
        {
            permission.RoleId = permissionVm.RoleId;
            permission.FunctionId = permissionVm.FunctionId;
            permission.CanCreate = permissionVm.CanCreate;
            permission.CanDelete = permissionVm.CanDelete;
            permission.CanRead = permissionVm.CanRead;
            permission.CanUpdate = permissionVm.CanUpdate;
        }
        public static void UpdateUser(this AppUser appUser, ApplicationUserViewModel appUserViewModel)
        {
            appUser.Id = appUserViewModel.Id;
            appUser.FullName = appUserViewModel.FullName;
            if (!string.IsNullOrEmpty(appUserViewModel.BirthDay))
            {
                DateTime dateTime = DateTime.ParseExact(appUserViewModel.BirthDay, "dd/MM/yyyy", new CultureInfo("vi-VN"));
                appUser.BirthDay = dateTime;
            }

            appUser.Email = appUserViewModel.Email;
            appUser.Address = appUserViewModel.Address;
            appUser.UserName = appUserViewModel.UserName;
            appUser.PhoneNumber = appUserViewModel.PhoneNumber;
            appUser.Gender = appUserViewModel.Gender == "True" ? true : false;
            appUser.Status = appUserViewModel.Status;
            appUser.Address = appUserViewModel.Address;
            appUser.Avatar = appUserViewModel.Avatar;
        }
        public static void UpdateProductQuantity(this ProductQuantity quantity, ProductQuantityViewModel quantityVm)
        {
            quantity.ProductId = quantityVm.ProductId;
            quantity.SizeId = quantityVm.SizeId;
            quantity.Quantity = quantityVm.Quantity;
        }


        public static void UpdateProductImage(this ProductImage image, ProductImageViewModel imageVm)
        {
            image.ProductId = imageVm.ProductId;
            image.Path = imageVm.Path;
            image.Caption = imageVm.Caption;
        }
        public static void UpdateSize(this Size size, SizeViewModel sizeVm)
        {
            size.ID = sizeVm.ID;
            size.Name = sizeVm.Name;
        }

        public static void UpdateOrder(this Order order, OrderViewModel orderVm)
        {
            order.CustomerName = orderVm.CustomerName;
            order.CustomerAddress = orderVm.CustomerAddress;
            order.CustomerEmail = orderVm.CustomerEmail;
            order.CustomerMobile = orderVm.CustomerMobile;
            order.CustomerMessage = orderVm.CustomerMessage;
            order.PaymentMethod = orderVm.PaymentMethod;
            order.CreateDate = DateTime.Now;
            order.CreateBy = orderVm.CreateBy;
            order.PaymentStatus = orderVm.PaymentStatus;
            order.Status = orderVm.Status;
            order.CustomerId = orderVm.CustomerId;
            order.TotalPayment = orderVm.TotalPayment;
        }

        public static void UpdateTag(this Tag tag, TagViewModel tagVm)
        {
            tag.Name = tagVm.Name;
            tag.Type = tagVm.Type;
           
        }

        public static void UpdateFooter(this Footer footer, FooterViewModel footerVm)
        {
            footer.ID = footerVm.ID;
            footer.Content = footerVm.Content;
        }
        public static void UpdateSupportOnline(this SupportOnline support, SupportOnlineViewModel supportVm)
        {
            support.ID = supportVm.ID;
            support.Name = supportVm.Name;
            support.Skype = supportVm.Skype;
            support.Status = "Kích hoạt";
            support.Facebook = supportVm.Facebook;
            support.Mobile = supportVm.Mobile;
            support.Email = supportVm.Email;

        }
        public static void UpdateSlide(this Slide slide, SlideViewModel slideVm)
        {
            slide.ID = slideVm.ID;
            slide.Name = slideVm.Name;
            slide.Status = slideVm.Status;
            slide.Description = slideVm.Description;
            slide.DisplayOrder = slideVm.DisplayOrder;
            slide.Content = slideVm.Content;
            slide.Image = slideVm.Image;
            slide.URL = slideVm.URL;

        }
        public static void UpdateOrderUserAnnoucement(this OrderUserAnnoucement annoucement, OrderUserAnnoucementViewModel annoucementVm)
        {
            annoucement.UserId = annoucementVm.UserId;
            annoucement.HasRead = annoucementVm.HasRead;
            annoucement.OrderId = annoucementVm.OrderId;
        }

        public static void UpdateSystemConfig(this SystemConfig systemConfig, SystemConfigViewModel systemConfigVm)
        {
            systemConfig.ID = systemConfigVm.ID;
            systemConfig.Code = systemConfigVm.Code;
            systemConfig.ValueString = systemConfigVm.ValueString;
            systemConfig.ValueInt = systemConfigVm.ValueInt;

        }



    }
}