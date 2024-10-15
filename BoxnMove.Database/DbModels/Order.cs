using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Database.DbModels
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public string PaidAmount { get; set; }
        public int PickupLocationId { get; set; }
        public int DropOffLocationId { get; set; }
        // Navigation properties
        public UserLocation PickupLocation { get; set; }
        public UserLocation DropOffLocation { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public CouponCode CouponCode { get; set; }
    }

}
