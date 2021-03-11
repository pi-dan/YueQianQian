using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YueQianQian.Model;

namespace YueQianQian.WebApi
{
    /// <summary>
    /// 未使用，用actionfilte实现
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var statusCode = context.Response.StatusCode;
                StatusCodeType statusCodeType = StatusCodeType.Error;
                if (ex is ArgumentException)
                {
                    statusCodeType = StatusCodeType.ArgumentException;
                }
                await HandleExceptionAsync(context, statusCodeType);
            }
            finally
            {
                var statusCode = context.Response.StatusCode;


                StatusCodeType statusCodeType = StatusCodeType.Default;
                if (statusCode == 401)
                {
                    statusCodeType = StatusCodeType.Unauthorized;
                }
                else if (statusCode == 404)
                {
                    statusCodeType = StatusCodeType.NotFound;
                }
                else if (statusCode == 502)
                {
                    statusCodeType = StatusCodeType.RequestError;
                }
                else if (statusCode != 200)
                {
                    statusCodeType = StatusCodeType.Error;
                }
                if (statusCodeType != StatusCodeType.Default)
                {
                    await HandleExceptionAsync(context, statusCodeType);
                }
            }
        }
        //异常错误信息捕获，将错误信息用Json方式返回
        private static Task HandleExceptionAsync(HttpContext context, StatusCodeType statusCode)
        {
            var result = JsonConvert.SerializeObject(new ApiResult() { code = (int)statusCode, msg = statusCode.GetEnumText() });
            context.Response.ContentType = "application/json;charset=utf-8";
            context.Response.StatusCode = 200;
            return context.Response.WriteAsync(result);
        }
    }
    //扩展方法
    public static class ErrorHandlingExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
