    using System.ComponentModel.DataAnnotations;

    namespace THLTW_B2.Models
    {
        public class RegisterViewModel
        {
            [Required(ErrorMessage = "Vui lòng nhập email.")]
            [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
            public string FullName { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu.")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Mật khẩu không khớp.")]
            public string ConfirmPassword { get; set; }
        }
    }
