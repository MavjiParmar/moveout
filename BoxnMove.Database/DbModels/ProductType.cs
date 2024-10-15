using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Database.DbModels
{
    public class ProductType
    {
        public int ProductTypeId { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; } = -1;
        public int CFT { get; set; } = -1;
        public string? BuildType { get; set; }
        // Foreign key
        public int ProductId { get; set; }
        // Navigation properties
        public Product? Product { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDt { get; set; }
        public DateTime? ModifiedDt { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
