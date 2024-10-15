using BoxnMove.Business.Services.Interface;
using BoxnMove.Database;
using BoxnMove.Database.DbModels;
using BoxnMove.Models.Models;
using BoxnMove.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Business.Services.Shared
{
    public class SharedService : ISharedService
    {
        private BoxnMoveDBContext _dbContext;
        private IConfiguration _configuration;
        private ILogger<AccountService> _logger;
        public SharedService(BoxnMoveDBContext dbContext, ILogger<AccountService> logger, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<ServiceResponse<User>> SaveFileDetailsAsync(List<IFormFile> files, int contactId)
        {
            var response = new ServiceResponse<User>();
            try
            {
                foreach (var formFile in files)
                {
                    var fileDetails = new ProjectFile
                    {
                        FileName = formFile.FileName,
                        FileFormat = Path.GetExtension(formFile.FileName),
                        FileSize = formFile.Length.ToString(),
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true,
                        ContactId = contactId
                    };

                    // Save file details to the database
                    _dbContext.ProjectFiles.Add(fileDetails);
                    await _dbContext.SaveChangesAsync();

                    _logger.LogDebug($"File details saved successfully for '{fileDetails.FileName}'");
                }

                response.Status = 1;
                response.Message = "Files saved successfully!.";

            }
            catch (Exception ex)
            {
                response.Status = 0;
                response.Message = $"An error occurred during saving file details. {ex.Message}";
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> SaveFileAsync(IFormFile file, int contactId, string fileName)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var fileDetails = new ProjectFile
                {
                    FileName = fileName,
                    FileFormat = Path.GetExtension(file.FileName),
                    FileSize = file.Length.ToString(),
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    ContactId = contactId
                };

                // Save file details to the database
                _dbContext.ProjectFiles.Add(fileDetails);
                await _dbContext.SaveChangesAsync();

                _logger.LogDebug($"File details saved successfully for '{fileDetails.FileName}'");


                response.Status = 1;
                response.Message = "Files saved successfully!.";

            }
            catch (Exception ex)
            {
                response.Status = 0;
                response.Message = $"An error occurred during saving file details. {ex.Message}";
            }
            return response;
        }

        public async Task<ServiceResponse<CouponResponse>> ValidateCoupon(string coupon)
        {
            try
            {
                _logger.LogDebug($"Begin: ValidateCoupon");

                var couponDetails = await _dbContext.CouponCodes.FirstOrDefaultAsync(x => x.Code == coupon);
                if (couponDetails != null)
                {
                    var couponResponse = new CouponResponse
                    {
                        Type = couponDetails.Type,
                        Value = Math.Round(couponDetails.Value, 2),
                        MinimumOrderAmount = Math.Round(couponDetails.MinimumOrderAmount, 2),
                        IsActive = couponDetails.IsActive,
                        ExpiryDate = couponDetails.ExpiryDate
                    };

                    return new ServiceResponse<CouponResponse>
                    {
                        Data = couponResponse,
                        Status = 1,
                        Message = "Coupon code found."
                    };
                }
                else
                {
                    return new ServiceResponse<CouponResponse>
                    {
                        Status = 0,
                        Message = "Coupon code not found."
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while ValidateCoupon");
                return new ServiceResponse<CouponResponse>
                {
                    Status = 0,
                    Message = "An error occurred during ValidateCoupon."
                };
            }
        }
        public async Task<ServiceResponse<bool>> VerifyOtp(string sessionId, int otp, string mobileNumber)
        {
            try
            {
                _logger.LogInformation($"Begin: VerifyOtp");

                var otpData = await _dbContext.OTPStores.FirstOrDefaultAsync(x => x.MobileNumber == mobileNumber && x.SessionId == sessionId);
                if (otpData == null || DateTime.UtcNow > otpData.ExpiresAt)
                {
                    return new ServiceResponse<bool>
                    {
                        Data = false,
                        Status = 0,
                        Message = "OTP has expired or does not exist."
                    };
                }
                if (otpData.IsUsed)
                {
                    return new ServiceResponse<bool>
                    {
                        Data = false,
                        Status = 0,
                        Message = "OTP has already been used."
                    };
                }

                if (otpData.OtpValue == otp)
                {
                    otpData.IsUsed = true;
                    await _dbContext.SaveChangesAsync();
                    return new ServiceResponse<bool>
                    {
                        Data = true,
                        Status = 1,
                        Message = "OTP verified successfully."
                    };
                }

                return new ServiceResponse<bool>
                {
                    Data = false,
                    Status = 1,
                    Message = "OTP does not verify."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while VerifyOtp");
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Status = 0,
                    Message = "An error occurred during VerifyOtp."
                };
            }
        }

        public async Task<ServiceResponse<string>> SaveOTP(int otp, string mobileNumber)
        {
            try
            {
                _logger.LogInformation($"Begin: SaveOTP");
                string sessionId = Guid.NewGuid().ToString();
                var otpStore = new OTPStore
                {
                    MobileNumber = mobileNumber,
                    OtpValue = otp,
                    SessionId = sessionId,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(2), 
                    IsUsed = false,
                    IsActive = true
                };

                _dbContext.OTPStores.Add(otpStore);
                await _dbContext.SaveChangesAsync();

                return new ServiceResponse<string>
                {
                    Data = sessionId,
                    Status = 1,
                    Message = "OTP saved successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while SaveOTP");
                return new ServiceResponse<string>
                {
                    Status = 0,
                    Message = "An error occurred during SaveOTP."
                };
            }
        }

    }
}
