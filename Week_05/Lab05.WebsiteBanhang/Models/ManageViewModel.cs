using System.ComponentModel.DataAnnotations;

namespace Lab04.WebsiteBanHang.Models
{
    public class ManageViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [Display(Name = "Họ tên")]
        public string FullName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Tuổi")]
        public int? Age { get; set; }
    }
}