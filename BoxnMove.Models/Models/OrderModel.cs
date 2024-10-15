using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Models.Models
{
    public class OrderModel
    {
    }

    public class ProductItemsRequestModel
    {
        public int CategoryId { get; set; }
        public int ProductId { get; set; }
        public List<ProductTypeRequestModel> ProductTypes { get; set; }
    }

    public class ProductTypeRequestModel
    {
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
        public int ItemCount { get; set; }
    }
}
