using DLL.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Services.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Extentions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        Tracert.WriteLog($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} ERROR something went wrong:", contextFeature.Error);
                        //await context.Response.WriteAsync(new ErrorDetails()
                        //{
                        //    StatusCode = context.Response.StatusCode,
                        //    Message = "Internal Server Error."
                        //}.ToString());
                    }
                });
            });
        }
    }
}
