using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Database.DbModels
{
    public class CouponCode
    {
        public int CouponCodeId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public decimal Value { get; set; }
        public decimal MinimumOrderAmount { get; set; }
        public bool IsActive { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
