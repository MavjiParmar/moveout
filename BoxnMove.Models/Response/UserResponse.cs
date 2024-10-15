using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Models.Response
{
  
    public class UserResponse
    {
        public int UserID { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
       
    }

    public class RegisterResponse: UserResponse
    {
        public string Name { get; set; } = string.Empty;
    }
    public class LoginResponse: UserResponse
    {
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public int RoleID { get; set; }
    }


}
