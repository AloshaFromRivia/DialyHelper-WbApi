using System;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace DailyHelper.Extentions
{
    public static class HttpContextExtensions
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return String.Empty;
            }

            return httpContext.User.Claims.Single(c => c.Type == "Id").Value;
        }
    }
}