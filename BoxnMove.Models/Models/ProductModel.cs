using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Models.Models
{
    public class ProductItemsModel
    {
        public List<CategoryModel> Categories { get; set; }
    }
    public class CategoryModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<ProductModel> Products { get; set; }
    }
    public class ProductModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
      
        public List<ProductTypeModel> ProductTypes { get; set; }
    }

    public class ProductTypeModel
    {
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
    }
}
