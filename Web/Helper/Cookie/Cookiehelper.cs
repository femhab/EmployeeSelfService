using Microsoft.AspNetCore.Http;
using Web.Helper.Common;
using System;

namespace Web.Helper.Cookie
{
    public interface ICookieHelper
    {
        void SetAuthCookie(string token, string refreshToken);
    }

    public class CookieHelper : ICookieHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public CookieHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public void SetAuthCookie(string token, string refreshToken)
        {
            var cookieOptions = new CookieOptions()
            {
                Path = "/",
                HttpOnly = false,
                IsEssential = true, //<- there
                Expires = DateTime.Now.AddMinutes(15),
                //Domain = _contextAccessor.HttpContext.Request.Host.ToUriComponent()
            };
            _contextAccessor.HttpContext.Response.Cookies.Append(Constant.TokenCacheKey, token, cookieOptions);
            _contextAccessor.HttpContext.Response.Cookies.Append(Constant.RefreshTokenCacheKey, refreshToken, cookieOptions);
        }
    }
}
