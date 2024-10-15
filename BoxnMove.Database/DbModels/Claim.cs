using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Database.DbModels
{
    public class Claim
    {
        public int ClaimID { get; set; }
        public string ClaimName { get; set; }
        public string Description { get; set; }

        public ICollection<UserClaim> UserClaims { get; set; }
        public ICollection<RoleClaim> RoleClaims { get; set; }
    }
}
