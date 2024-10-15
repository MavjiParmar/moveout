using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Database.DbModels
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        // Navigation property
        public List<Product> Products { get; set; }

        public bool? IsActive { get; set; }
        public DateTime CreatedDt { get; set; }
        public DateTime? ModifiedDt { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
