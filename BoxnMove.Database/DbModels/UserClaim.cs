using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Database.DbModels
{
    public class UserClaim
    {
        public int UserClaimID { get; set; }
        public int UserID { get; set; }
        public int ClaimID { get; set; }
        public string ClaimValue { get; set; }
    }
}
