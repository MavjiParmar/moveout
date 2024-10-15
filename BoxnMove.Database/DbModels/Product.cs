using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Database.DbModels
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }


        // Foreign key
        public int CategoryId { get; set; }
        // Navigation properties
        public Category? Category { get; set; }
        public List<ProductType>? ProductTypes { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDt { get; set; }
        public DateTime? ModifiedDt { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
