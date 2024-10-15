using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Database.DbModels
{
    public class ProjectFile
    {
        public int Id { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string FileFormat { get; set; }
        [Required]
        public string FileSize { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public bool IsActive { get; set; }
        // Foreign key property
        public int ContactId { get; set; }

        // Navigation property
        [ForeignKey("ContactId")]
        public Contact Contact { get; set; }

    }
}
