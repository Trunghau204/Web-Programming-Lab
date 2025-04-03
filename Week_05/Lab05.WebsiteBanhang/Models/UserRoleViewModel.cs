using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab04.WebsiteBanHang.Models
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int? Age { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn vai trò")]
        public string SelectedRole { get; set; }

        public List<string> AvailableRoles { get; set; } = new List<string>();
    }
}
