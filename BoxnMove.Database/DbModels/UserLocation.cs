using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Database.DbModels
{
    public class UserLocation
    {
        public int UserLocationId { get; set; }
        public int UserId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int Floors { get; set; }
        public bool ServiceLiftAvailable { get; set; }
        public string Description { get; set; }
        public DateTime PickUpDropDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string LocationType { get; set; }
        public string RelocationType { get; set; }
        public int ParkingDistance { get; set; }
        // Navigation property
        public User User { get; set; }
    }
}
