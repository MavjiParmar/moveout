using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Database.DbModels
{
    public class User
    {
        public int UserID { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        [Required]
        public string CountryCode { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool Status { get; set; }
        public int? FailedAttempts { get; set; }
        public bool? ChangePassword { get; set; }
        public DateTime? DatePasswordChanged { get; set; }
        public DateTime? CreatedDt { get; set; }
        public DateTime? ModifiedDt { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }

        public ICollection<Role>? Roles { get; set; }
    }
}
