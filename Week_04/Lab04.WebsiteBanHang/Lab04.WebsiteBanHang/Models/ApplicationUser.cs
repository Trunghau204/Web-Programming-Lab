using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;


namespace THLTW_B2.Models {

    public class ApplicationUser: IdentityUser {
        [Required]
        public string FullName { get; set; }
        public string? Address { get; set; }
        public string? Age { get; set; }

        public string Role { get; set; } = SD.Role_User; 
    }
}
