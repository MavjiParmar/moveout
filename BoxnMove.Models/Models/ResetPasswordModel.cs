using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Models.Models
{
    public class ResetPasswordModel
    {
        //[Required(ErrorMessage = "Email is required.")]
        //[EmailAddress(ErrorMessage = "Invalid email format.")]
        //public string Email { get; set; }

        [Required(ErrorMessage = "SessionId is required.")]
        public string SessionId { get; set; }

        [Required(ErrorMessage = "OTP is required.")]
        public int Otp { get; set; }

        [Required(ErrorMessage = "New password is required.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [Phone]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid mobile number")]
        public string MobileNumber { get; set; }
    }
}
