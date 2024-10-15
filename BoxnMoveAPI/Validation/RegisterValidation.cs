using BoxnMove.Business.Services.Interface;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BoxnMoveAPI.Validation
{
    public class RegisterValidation
    {
        private IAccountService _accountService;

        public RegisterValidation(IAccountService accountService)
        {
            _accountService = accountService;
        }
       
        public static bool IsPasswordValid(string password)
        {
            if (password == null || password.Length < 6 || password.Length > 12)
            {
                return false;
            }

            // Password should contain at least one lowercase letter, one uppercase letter,
            // one special character, and one digit
            if (!Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,12}$"))
            {
                return false;
            }

            return true;
        }

        public bool IsEmailInUse(string email)
        {
            try
            {
                return (_accountService.IsEmailInUse(email)?.Status ?? 0) == 1;
            }
            catch (Exception ex)
            {
                return false; 
            }
        }
        public bool IsMobileInUse(string mobileNumber)
        {
            try
            {
                return (_accountService.IsMobileNumberInUse(mobileNumber)?.Status ?? 0) == 1;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
