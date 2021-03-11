using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YueQianQian.Model;

namespace YueQianQian.WebApi
{
    /// <summary>
    /// 全局异常处理
    /// </summary>
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<CustomExceptionFilter> _logger;
        public CustomExceptionFilter(IWebHostEnvironment env, ILogger<CustomExceptionFilter> logger)
        {
            _env = env;
            _logger = logger;
        }
        /// <summary>
        /// 全局异常处理
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            ApiResult<string> response = new ApiResult<string>();

            response.code = (int)StatusCodeType.Error;
            response.msg = context.Exception.Message;//错误信息

            if (_env.IsDevelopment())
            {
                response.data = context.Exception.StackTrace;//堆栈信息
            }

            context.Result = new JsonResult(response) { StatusCode = 200 };


            //采用log4net 进行错误日志记录
            _logger.LogError(WriteLog(response.msg, context.Exception));
        }
        /// <summary>
        /// 自定义返回格式
        /// </summary>
        /// <param name="throwMsg"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public string WriteLog(string throwMsg, Exception ex)
        {
            return $"\r\n【自定义错误】：{throwMsg} \r\n【异常类型】：{ex.GetType().Name} \r\n【异常信息】：{ex.Message} \r\n【堆栈调用】：{ex.StackTrace }\r\n";
        }
    }
}
