using BoxnMove.Database.DbModels;
using BoxnMove.Models.Models;
using BoxnMove.Models.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Business.Services.Shared
{
    public interface ISharedService
    {
        Task<ServiceResponse<User>> SaveFileDetailsAsync(List<IFormFile> files, int contactId);
        Task<ServiceResponse<bool>> SaveFileAsync(IFormFile file, int contactId, string fileName);
        Task<ServiceResponse<CouponResponse>> ValidateCoupon(string coupon);
        Task<ServiceResponse<bool>> VerifyOtp(string sessionId, int otp, string mobileNumber);
        Task<ServiceResponse<string>> SaveOTP(int otp, string mobileNumber);
    }
}
