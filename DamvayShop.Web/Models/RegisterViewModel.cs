using System.ComponentModel.DataAnnotations;

namespace DamvayShop.Web.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Bạn phải nhập tên tài khoản")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập mật khẩu")]
        [MinLength(6, ErrorMessage = "Mật khẩu tối thiểu 6 kí tự")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập Email")]
         [EmailAddress(ErrorMessage ="Email không hợp lệ")]
        public string Email { get; set; }
    }
}