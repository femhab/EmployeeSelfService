using Data.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Business.Providers.JWT
{
    public interface IAuthTokenProvider
    {
        string GenerateJwtToken(AuthTokenModel model, out List<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromToken(string token, bool checkExpiration = false);
        AuthTokenModel DecodeJwtToken(string token, bool checkExpiration = false);
    }

    public class AuthTokenProvider : IAuthTokenProvider
    {
        private readonly IConfiguration _configuration;
        public AuthTokenProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(AuthTokenModel model, out List<Claim> claims)
        {
            claims = new List<Claim>
            {
                new Claim("UserData", JsonConvert.SerializeObject(model, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }))
            };

            if (!string.IsNullOrEmpty(model.EmailAddress))
            {
                claims.Add(new Claim(ClaimTypes.Name, model.EmailAddress));
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, model.EmailAddress));
            };
            //set token property here
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:AccessExpireMinutes"]));

            //create a new token
            var securityToken = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Issuer"], claims, expires: expires, signingCredentials: creds);
            //return a new token
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }

        //used to generate fresh tokens
        public string GenerateRefreshToken()
        {
            var generate = new byte[32];
            //uses cryptography to generate random numbers to use in token
            using (var randomGenerator = RandomNumberGenerator.Create())
            {
                randomGenerator.GetBytes(generate);
                return Convert.ToBase64String(generate);
            }
        }


        public ClaimsPrincipal GetPrincipalFromToken(string token, bool checkExpiration = false)
        {
            var validationParameter = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ValidateLifetime = checkExpiration,
                RequireExpirationTime = checkExpiration,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, validationParameter, out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid Token");
            }
            return principal;
        }

        public AuthTokenModel DecodeJwtToken(string token, bool checkExpiration = false)
        {
            try
            {
                var principal = GetPrincipalFromToken(token, checkExpiration);

                ClaimsIdentity identity = principal.Identity as ClaimsIdentity;

                if (identity == null)
                {
                    return null;
                }

                var userData = identity.FindFirst("UserData")?.Value;

                var tokenData = JsonConvert.DeserializeObject<AuthTokenModel>(userData);

                return tokenData;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    public class AuthTokenModel
    {
        public Guid Id { get; set; }
        public string Emp_No { get; set; }
        public Guid? UnitId { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? DivisionId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public AccessType AccessType { get; set; }
    }
}
