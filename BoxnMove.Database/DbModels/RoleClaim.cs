using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Database.DbModels
{
    public class RoleClaim
    {
        public int RoleClaimID { get; set; }
        public int RoleID { get; set; }
        public int ClaimID { get; set; }
        public string ClaimValue { get; set; }
    }

}
