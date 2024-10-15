using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Database.DbModels
{
    public class Contact
    {
        public int ContactId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string? RelocationType { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        public string? Description { get; set; }
        public string? ContactType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }

       
    }
}
