using Sabio.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Users
{
    public class UserAddRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string LastName { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string AvatarUrl { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string TenantId { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 2)]
        //[RegularExpression(@"^(?=.*?[A - Z])(?=.*?[a - z])(?=.*?[0 - 9])(?=.*?[#?!$%^&*-]).{8,}")]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
    }
}
