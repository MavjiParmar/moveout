using BoxnMove.Database;
using BoxnMove.Database.DbModels;
using BoxnMove.Database.Migrations;
using BoxnMove.Models.Models;
using BoxnMove.Models.Response;
using BoxnMove.Shared.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Business.Services.Interface
{
    public class AccountService : IAccountService
    {
        private BoxnMoveDBContext _dbContext;
        private ILogger<AccountService> _logger;
        public AccountService(BoxnMoveDBContext dbContext, ILogger<AccountService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public ServiceResponse<LoginResponse> Validate(LoginModel login)
        {
            var response = new ServiceResponse<LoginResponse>();
            try
            {
                _logger.LogDebug($"Begin: Validate for login '{login}'");

                var userWithRoles = _dbContext.Users
                    .Include(u => u.Roles)
                    .FirstOrDefault(x => x.UserName == login.Login && x.Status == true);

                if (userWithRoles != null)
                {
                    if (HashHelper.ValidatePassword(login.Password, userWithRoles.Password))
                    {
                        var userResponse = new LoginResponse
                        {
                            UserID = userWithRoles.UserID,
                            UserName = userWithRoles.UserName,
                            FirstName = userWithRoles.FirstName ?? string.Empty,
                            MobileNumber = userWithRoles.MobileNumber ?? string.Empty,
                            LastName = userWithRoles.LastName ?? string.Empty,
                            EmailAddress = userWithRoles.Email,
                            RoleID = userWithRoles?.Roles?.FirstOrDefault()?.RoleID ?? 0,
                            RoleName = userWithRoles?.Roles?.FirstOrDefault()?.RoleName ?? ""
                        };

                        response.Data = userResponse;
                        response.Status = 1;
                        response.Message = "Login successful";
                    }
                    else
                    {
                        response.Status = 0;
                        response.Message = "Incorrect password";
                    }
                }
                else
                {
                    response.Status = 0;
                    response.Message = "User not found or inactive";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login validation");
                response.Status = 0;
                response.Message = "An error occurred during login validation";
            }

            return response;
        }
        public ServiceResponse<RegisterResponse> RegisterUser(RegisterModel model)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _logger.LogDebug($"Begin: Validate for RegisterUser '{model}'");

                    if (_dbContext.Users.Any(u => u.UserName == model.MobileNumber))
                    {
                        return new ServiceResponse<RegisterResponse>
                        {
                            Status = 0,
                            Message = "This Mobile Number is already in use."
                        };
                    }

                    // Retrieve the "Customer" role from the database
                    var customerRole = _dbContext.Roles.FirstOrDefault(r => r.RoleName == "Customer");
                    if (customerRole == null)
                    {
                        _logger.LogWarning("The 'Customer' role was not found.");
                        return new ServiceResponse<RegisterResponse>
                        {
                            Status = 0,
                            Message = "The 'Customer' role was not found."
                        };
                    }

                    var newUser = new User
                    {
                        Email = model.Email,
                        UserName = model.MobileNumber,
                        Password = HashHelper.GenerateHash(model.Password),
                        MobileNumber = model.MobileNumber,
                        CountryCode = "91",
                        FirstName = model.Name,
                        Status = true,
                        CreatedDt = DateTime.UtcNow,
                        Roles = new List<Role> { customerRole }
                    };

                    // Save the new user and user roles to the database
                    _dbContext.Users.Add(newUser);
                    _dbContext.SaveChanges();

                    var userResponse = new RegisterResponse
                    {
                        UserID = newUser.UserID,
                        Name = newUser.FirstName,
                        MobileNumber = newUser.MobileNumber,
                        EmailAddress = newUser.Email,
                        RoleName = newUser.Roles.FirstOrDefault()?.RoleName ?? "Customer",
                    };

                    transaction.Commit(); 
                    return new ServiceResponse<RegisterResponse>
                    {
                        Status = 1,
                        Message = "User registered successfully.",
                        Data = userResponse
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.InnerException, $"An error occurred while registering user '{model}'");
                    transaction.Rollback();
                    return new ServiceResponse<RegisterResponse>
                    {
                        Status = 0,
                        Message = "An error occurred during user registration."
                    };
                }
            }
        }
        public List<City> Cities(string q)
        {
          var cities=  _dbContext.Cities.AsQueryable().Where(c => c.ActiveStatus == 1&&c.Name.Contains(q)).ToList();
            return cities;
        }
        public ServiceResponse<bool> SaveContactQuery(ContactsModel model)
        {
            try
            {
                _logger.LogDebug($"Begin: Save data for SaveContactQuery '{model}'");
                var contact = new Contact
                {
                    Name = model.Name,
                    Email = model.Email,
                    RelocationType = model.RelocationType,
                    MobileNumber = model.MobileNumber,
                    Description = model.Description,
                    CreatedDate = DateTime.UtcNow,
                    ContactType = "SVC",
                    IsActive = true
                };

                _dbContext.Contacts.Add(contact);
                _dbContext.SaveChanges();

                return new ServiceResponse<bool>
                {
                    Data = true,
                    Status = 1,
                    Message = "Data save successfully!."
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while SaveContactQuery '{model}'");
                return new ServiceResponse<bool>
                {
                    Status = 0,
                    Message = "An error occurred during SaveContactQuery."
                };
            }
        }
        public ServiceResponse<int> SaveFilesContactQuery(ContactFileModel model)
        {
            try
            {
                _logger.LogDebug($"Begin: Save data for SaveFilesContactQueryAsync '{model}'");
                var contact = new Contact
                {
                    Name = model.Name,
                    Email = model.Email,
                    MobileNumber = model.MobileNumber,
                    ContactType = "SVCF",
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
                };

                _dbContext.Contacts.Add(contact);
                _dbContext.SaveChanges();

                return new ServiceResponse<int>
                {
                    Data = contact.ContactId,
                    Status = 1,
                    Message = "Data save successfully!."
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while SaveFilesContactQueryAsync '{model}'");
                return new ServiceResponse<int>
                {
                    Status = 0,
                    Message = "An error occurred during SaveFilesContactQueryAsync."
                };
            }
        }

        public ServiceResponse<int> SaveDescriptionContactQuery(ContactDescriptionModel model)
        {
            try
            {
                _logger.LogDebug($"Begin: Save data for SaveDescriptionContactQuery '{model}'");
                var contact = new Contact
                {
                    Name = model.Name,
                    Email = model.Email,
                    MobileNumber = model.MobileNumber,
                    ContactType = "SVCD",
                    Description = model.Description,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
                };

                _dbContext.Contacts.Add(contact);
                _dbContext.SaveChanges();

                return new ServiceResponse<int>
                {
                    Data = contact.ContactId,
                    Status = 1,
                    Message = "Data save successfully!."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while SaveDescriptionContactQuery '{model}'");
                return new ServiceResponse<int>
                {
                    Status = 0,
                    Message = "An error occurred during SaveDescriptionContactQuery."
                };
            }
        }

        public ServiceResponse<bool> IsEmailInUse(string email)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                _logger.LogDebug($"Begin: Validate for IsEmailInUse '{email}'");

                var existingUser = _dbContext.Users.FirstOrDefault(u => u.Email == email);
                if (existingUser != null)
                {
                    response.Status = 1;
                    response.Message = "This email address is already in use.";
                    response.Data = true;
                    return response;
                }

                // Set success message
                response.Status = 0;
                response.Message = "Email is available.";
                response.Data = false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while validating email '{email}'");
                response.Status = 0;
                response.Message = "An error occurred during email validation.";
            }

            return response;
        }
        public ServiceResponse<bool> IsMobileNumberInUse(string mobileNumber)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                _logger.LogDebug($"Begin: Validate for IsMobileNumberInUse '{mobileNumber}'");

                var existingUser = _dbContext.Users.FirstOrDefault(u => u.UserName == mobileNumber);
                if (existingUser != null)
                {
                    response.Status = 1;
                    response.Message = "This mobile Number is already in use.";
                    response.Data = true;
                    return response;
                }

                // Set success message
                response.Status = 0;
                response.Message = "mobile Number is available.";
                response.Data = false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while validating mobile Number '{mobileNumber}'");
                response.Status = 0;
                response.Message = "An error occurred during mobile Number validation.";
            }

            return response;
        }
        public async Task<ServiceResponse<User>> FindByEmailAsync(string email)
        {
            var response = new ServiceResponse<User>();
            try
            {
                _logger.LogDebug($"Begin:  FindByEmailAsync '{email}'");
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
                response.Data = user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while validating email '{email}'");
                response.Status = 0;
                response.Message = "An error occurred during FindByEmailAsync.";
            }
            return response;
        }
        public async Task<ServiceResponse<User>> FindByMobileNumberAsync(string mobileNumber)
        {
            var response = new ServiceResponse<User>();
            try
            {
                _logger.LogInformation($"Begin:  FindByMobileNumberAsync '{mobileNumber}'");
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == mobileNumber);
                response.Data = user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while validating mobileNumber '{mobileNumber}'");
                response.Status = 0;
                response.Message = "An error occurred during FindByMobileNumberAsync.";
            }
            return response;
        }
        public async Task<ServiceResponse<bool>> ResetPasswordAsync(ResetPasswordModel model)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                _logger.LogDebug($"Begin: ResetPasswordAsync '{model}'");

                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == model.MobileNumber);
                if (user == null)
                {
                    response.Status = 0;
                    response.Message = "User not found.";
                    return response;
                }

                user.Password = HashHelper.GenerateHash(model.NewPassword);
                user.ChangePassword = true;
                user.DatePasswordChanged = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();

                response.Status = 1;
                response.Data = true;
                response.Message = "Password reset successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while resetting password for '{model.MobileNumber}'");
                response.Status = 0;
                response.Message = "An error occurred during password reset.";
            }
            return response;
        }

    }
}