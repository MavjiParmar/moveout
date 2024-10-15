using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Models.Models
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Phone]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid mobile number")]
        public string MobileNumber { get; set; }
        //[Required]
        //[StringLength(3, MinimumLength = 3)]
        //[RegularExpression(@"^\d{3}$", ErrorMessage = "Invalid country code")]
        //public string CountryCode { get; set; }
        [Required]
        public int OTP { get; set; }
        [Required]
        public string SessionId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
