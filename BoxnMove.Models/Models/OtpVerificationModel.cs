using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Models.Models
{
    public class OtpVerificationModel
    {
        public string MobileNumber { get; set; }
        public int OTP { get; set; }
        public string SessionId { get; set; }
    }
}
