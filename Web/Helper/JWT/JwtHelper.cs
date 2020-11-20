using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ViewModel.Model;
using Web.Helper.Common;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Web.Helper.JWT
{
    public class JwtHelper
    {
        public static JwtSecurityToken GetToken(string authToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(authToken);
            return token;
        }

        public static AuthDataModel GetAuthData(HttpRequest httpRequest)
        {
            if (httpRequest.Cookies.TryGetValue(Constant.TokenCacheKey, out string authToken))
            {
                var token = GetToken(authToken);
                if (token != null)
                {
                    var authData = token.Claims.First(claim => claim.Type == "UserData").Value;
                    if (!string.IsNullOrEmpty(authData))
                    {
                        return JsonConvert.DeserializeObject<AuthDataModel>(authData);
                    }
                }
            }
            return null;
        }

        public static IEnumerable<Claim> GetClaims(string accessToken)
        {
            var jwt = GetToken(accessToken);
            if (jwt != null)
            {
                return jwt.Claims;
            }
            return default;
        }

        public static ClaimsPrincipal CreatePrincipal(IEnumerable<Claim> claims, string authenticationType = null, string roleType = null)
        {
            var claimsPrincipal = new ClaimsPrincipal();
            claimsPrincipal.AddIdentity(new ClaimsIdentity(claims, authenticationType ?? "Token", ClaimTypes.Name, roleType ?? "User"));

            return claimsPrincipal;
        }

        public static AuthenticationProperties CreateAuthProperties(string accessToken)
        {
            var authProps = new AuthenticationProperties();
            authProps.StoreTokens(
                new[]
                {
                    new AuthenticationToken()
                    {
                        Name = "jwt",
                        Value = accessToken
                    }
                });

            return authProps;
        }
    }
}
