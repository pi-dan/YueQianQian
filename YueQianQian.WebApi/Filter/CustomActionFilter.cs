using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using YueQianQian.Model;

namespace YueQianQian.WebApi
{
    public class CustomActionFilter : IActionFilter
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private IWebHostEnvironment _env;
        private Stopwatch _stopwatch;

        public CustomActionFilter(IWebHostEnvironment env)
        {

            _env = env;
        }

        /// <summary>
        /// 方法执行前
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {

            if (_env.IsDevelopment())
            {
                var actionLog = $"{Environment.NewLine}{DateTime.Now} 开始调用 {context.ActionDescriptor.AttributeRouteInfo.Template}{Environment.NewLine}" +
                $"参数为：{Newtonsoft.Json.JsonConvert.SerializeObject(context.ActionArguments)}";
                _logger.Trace(actionLog);
                _stopwatch = new Stopwatch();
                _stopwatch.Restart();
            }
        }

        /// <summary>
        /// 方法执行后
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (_env.IsDevelopment())
            {
                _stopwatch.Stop();
                var result = context.Result;

                var resultLog = $"{DateTime.Now} 结束调用 {context.ActionDescriptor.AttributeRouteInfo.Template} {Environment.NewLine}" +
                    $"执行结果：{JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })}{Environment.NewLine}" +
                    $"耗时:{_stopwatch.ElapsedMilliseconds}ms";
                _logger.Trace(resultLog);
            }

        }
    }
}
