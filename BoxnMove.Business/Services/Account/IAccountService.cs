using BoxnMove.Database.DbModels;
using BoxnMove.Models.Models;
using BoxnMove.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Business.Services.Interface
{
    public interface IAccountService
    {
        ServiceResponse<LoginResponse> Validate(LoginModel login);
        ServiceResponse<RegisterResponse> RegisterUser(RegisterModel model);
        ServiceResponse<bool> IsEmailInUse(string email);
        ServiceResponse<bool> SaveContactQuery(ContactsModel model);
        Task<ServiceResponse<User>> FindByEmailAsync(string email);
        Task<ServiceResponse<bool>> ResetPasswordAsync(ResetPasswordModel model);
        ServiceResponse<int> SaveFilesContactQuery(ContactFileModel model);
        ServiceResponse<int> SaveDescriptionContactQuery(ContactDescriptionModel model);
        ServiceResponse<bool> IsMobileNumberInUse(string mobileNumber);
        Task<ServiceResponse<User>> FindByMobileNumberAsync(string mobileNumber);
        List<City> Cities(string q);
    }
}
