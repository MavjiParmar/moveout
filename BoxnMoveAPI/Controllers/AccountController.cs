using BoxnMove.Business.Services.Interface;
using BoxnMove.Business.Services.Shared;
using BoxnMove.Database.Migrations;
using BoxnMove.Models.Models;
using BoxnMove.Models.Response;
using BoxnMove.Shared.Utilities;
using BoxnMoveAPI.Helpers;
using BoxnMoveAPI.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;

namespace BoxnMoveAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private IAccountService _accountService;
        private SMSService _sMSService;
        private ISharedService _sharedService;
        private IConfiguration _configuration;
        private ILogger<AccountController> _logger;
        public AccountController(IAccountService accountService, IConfiguration configuration, ILogger<AccountController> logger, ISharedService sharedService, SMSService sMSService)
        {
            this._accountService = accountService;
            this._configuration = configuration;
            this._logger = logger;
            this._sharedService = sharedService;
            this._sMSService = sMSService;
        }

        [HttpPost("Authorize")]
        public IActionResult Authorize(LoginModel loginModel)
        {
            try
            {
                var user = _accountService.Validate(loginModel);
                if (user.Data == null)
                {
                    return Unauthorized();
                }
                string jwtKey = _configuration["Jwt:Key"] ?? "";
                string jwtIssuer = _configuration["Jwt:Issuer"] ?? "";
                user.Data.Token = JWTHelper.GenerateToken(user.Data, jwtKey, jwtIssuer);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during Authorize User.");
                return Error("Internal Server Error!.", 500);
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterModel registerModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (new RegisterValidation(_accountService).IsMobileInUse(registerModel.MobileNumber))
                {
                    return Error("This mobile number is already in use.", 400);
                }

                if (!RegisterValidation.IsPasswordValid(registerModel.Password))
                {
                    return Error("Invalid password!.", 400);
                }

                var otpResult = await _sharedService.VerifyOtp(registerModel.SessionId, registerModel.OTP, registerModel.MobileNumber);
                if (!otpResult.Data)
                {
                    return Error("Invalid OTP.", 400);
                }

                var registrationResult = _accountService.RegisterUser(registerModel);
                if (registrationResult.Status == 1)
                {
                    return Ok(new { registrationResult });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during user registration.");
                return Error("Internal Server Error!.", 500);
            }
        }

        [HttpGet("SendOTP")]
        public async Task<IActionResult> SendOtp(string mobileNumber)
        {
            try
            {
                var (isSuccess, otpCode) = await _sMSService.SendOtpAsync(mobileNumber);
                if (!isSuccess)
                {
                    return Error("Unable to send OTP.", 401);
                }
                var OTPResult = await _sharedService.SaveOTP(otpCode, mobileNumber);
                return !string.IsNullOrEmpty(OTPResult.Data) ? Success(OTPResult.Data, "OTP sent successfully.") : Error("Unable to send OTP.", 503);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending OTP.");
                return Error("Internal Server Error in SendOtp!.", 500);
            }
        }

        [HttpPost("VerifyOTP")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpVerificationModel model)
        {
            try
            {
                var otpResult = await _sharedService.VerifyOtp(model.SessionId, model.OTP, model.MobileNumber);
                return otpResult.Data ? Success(true, "OTP verified successfully.") : Error("Invalid OTP.", 400);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying OTP.");
                return Error("Failed to verify OTP. Please try again later.", 500);
            }
        }
        [HttpGet("Cities")]
        public IActionResult Cities(string query)
        {

            return Ok(_accountService.Cities(query));

        }

        [HttpPost("ContactQuery")]
        public IActionResult ContactQuery([FromBody] ContactsModel contactModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var response = _accountService.SaveContactQuery(contactModel);
                return response.Status == 1 ? Success(true, "Contact form submitted successfully.") : Error("An error occurred while processing your request.", 400);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing contact form.");
                return Error("An error occurred while processing your request.", 500);
            }
        }

        [HttpPost("ContactFileQuery")]
        public async Task<IActionResult> ContactFileQuery([FromForm] ContactFileModel contactModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var otpResult = await _sharedService.VerifyOtp(contactModel.SessionId, contactModel.OTP, contactModel.MobileNumber);
                if (!otpResult.Data)
                {
                    return Error("Invalid OTP.", 400);
                }

                // Save files to the server
                List<string> fileNames = new List<string>();
                string fileName = "";
                string pathURL = _configuration["FileSettings:BaseURL"] ?? "";
                int fileNameMaxLength = int.TryParse(_configuration["FileSettings:FileNameMaxLength"], out int maxLength) ? maxLength : 255;
                long maxFileSizeBytes = long.TryParse(_configuration["FileSettings:MaxFileSizeBytes"], out long fileSize) ? fileSize : 10485760;

                var response = _accountService.SaveFilesContactQuery(contactModel);

                foreach (var formFile in contactModel.Files)
                {
                    if (formFile.Length > 0)
                    {
                        if (!FileHelper.IsFileNameValid(formFile.FileName, fileNameMaxLength))
                        {
                            return Error($"File name '{formFile.FileName}' exceeds the maximum length.", 400);
                        }

                        if (!FileHelper.IsFileSizeValid(formFile.Length, maxFileSizeBytes))
                        {
                            return Error($"File '{formFile.FileName}' exceeds the maximum size limit.", 400);
                        }

                        fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
                        string appRootPath = AppDomain.CurrentDomain.BaseDirectory;
                        string folderName = "CustomerImages";

                        string filePath = Path.Combine(appRootPath, folderName, fileName);

                        Directory.CreateDirectory(Path.Combine(appRootPath, folderName));

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }

                        fileNames.Add(fileName);
                        await _sharedService.SaveFileAsync(formFile, response.Data, fileName);
                    }
                }

                return Success(response.Status, "Query submitted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading files.");
                return Error("An error occurred while processing your request.", 500);
            }
        }

        [HttpPost("ContactDescriptionQuery")]
        public async Task<IActionResult> ContactDescriptionQuery([FromBody] ContactDescriptionModel contactModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var otpResult = await _sharedService.VerifyOtp(contactModel.SessionId, contactModel.OTP, contactModel.MobileNumber);
                if (!otpResult.Data)
                {
                    return Error("Invalid OTP.", 400);
                }

                var response = _accountService.SaveDescriptionContactQuery(contactModel);

                return Success(response.Status, "Query submitted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ContactDescriptionQuery.");
                return Error("An error occurred while processing your request.", 500);
            }
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordModel model)
        {
            try
            {
                var user = await _accountService.FindByMobileNumberAsync(model.MobileNumber);
                if (user.Data == null)
                {
                    return Error("User not found.", 400);
                }

                var (isSuccess, otpCode) = await _sMSService.SendOtpAsync(model.MobileNumber);

                if (!isSuccess)
                {
                    return Error("Fail to send otp.", 400);
                }
                var OTPResult = await _sharedService.SaveOTP(otpCode, model.MobileNumber);
                return Success(OTPResult.Data, "OTP send successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing ForgetPassword request.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            try
            {
                if (!RegisterValidation.IsPasswordValid(model.NewPassword))
                {
                    return Error("Invalid password!.", 400);
                }
                var otpResult = await _sharedService.VerifyOtp(model.SessionId, model.Otp, model.MobileNumber);
                if (!otpResult.Data)
                {
                    return Error("Invalid OTP.", 400);
                }

                var user = await _accountService.FindByMobileNumberAsync(model.MobileNumber);
                if (user == null)
                {
                    return Error("User not exist.", 400);
                }

                var resetPasswordResult = await _accountService.ResetPasswordAsync(model);
                return resetPasswordResult.Status == 1 ? Success(true, "Password Reset Successfully!.") : Error("Error.", 400);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing ResetPassword request.");
                return Error("Internal Server Error!", 500);
            }
        }
    }
}
