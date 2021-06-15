using Microsoft.AspNetCore.Http;
using System;

namespace Services.Helper
{
    public static class DinhDangThapPhanHelper
    {
        public static decimal Round(this decimal input, IHttpContextAccessor accessor, string claimType)
        {
            var claim = accessor.HttpContext.Request.Cookies[claimType];
            if (claim != null)
            {
                var places = int.Parse(claim);
                return Math.Round(input, places);
            }

            return input;
        }

        public static decimal Round(this decimal? input, IHttpContextAccessor accessor, string claimType)
        {
            if (input.HasValue == false)
            {
                return 0;
            }

            var claim = accessor.HttpContext.Request.Cookies[claimType];
            if (claim != null)
            {
                var places = int.Parse(claim);
                return Math.Round(input.Value, places);
            }

            return input.Value;
        }
    }
}
