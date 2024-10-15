using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Models.Models
{
    public class ContactBaseModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string MobileNumber { get; set; }
        public string? SessionId { get; set; }
    }
    public class ContactsModel : ContactBaseModel
    {

        [Required]
        public string RelocationType { get; set; }
       
        public string Description { get; set; }
    }

    public class ContactFileModel: ContactBaseModel
    {
        [Required]
        public int OTP { get; set; }

        [DataType(DataType.Upload)]
        public List<IFormFile> Files { get; set; }

    }
    public class ContactDescriptionModel : ContactBaseModel
    {
        public string Description { get; set; }
        [Required]
        public int OTP { get; set; }
    }
}
