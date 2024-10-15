using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Models.Models
{
    public class CouponResponse
    {
        public string Type { get; set; }
        public decimal Value { get; set; }
        public decimal MinimumOrderAmount { get; set; }
        public bool IsActive { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
