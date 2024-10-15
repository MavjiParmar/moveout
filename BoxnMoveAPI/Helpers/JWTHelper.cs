using BoxnMove.Models.Response;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BoxnMoveAPI.Helpers
{
    public class JWTHelper
    {
        #region LoggedIn/API User
        public static LoginResponse GetAuthenticatedUser(ClaimsPrincipal user)
        {
            try
            {
                var userModel = new LoginResponse()
                {
                    UserID = Convert.ToInt32(user.FindFirst("UserID")?.Value),
                    UserName = user.FindFirst(ClaimTypes.Name)?.Value ?? String.Empty,
                    EmailAddress = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? String.Empty,
                    FirstName = user.FindFirst("FirstName")?.Value ?? String.Empty,
                    LastName = user.FindFirst("LastName")?.Value ?? String.Empty,
                    RoleID = Convert.ToInt32(user.FindFirst("RoleID")?.Value),
                };

                return userModel;
            }
            catch { return null; }
        }
        #endregion

        #region Generate Token
        public static string GenerateToken(LoginResponse user, String key, String issuer)
        {
            var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.EmailAddress.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("UserID", user.UserID.ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("RoleID",user.RoleID.ToString()),
                new Claim("RoleName",user.RoleName.ToString()),
            };

            //Get security key 
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            //Get Credentials
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = issuer,
                Audience = issuer,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenhandler = new JwtSecurityTokenHandler();
            var token = tokenhandler.CreateToken(tokenDescriptor);

            return tokenhandler.WriteToken(token);
        }
        #endregion

        #region Validate Token
        public static int? ValidateToken(string token, string privateKey)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(privateKey);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "UserID").Value);
                return userId;
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}
