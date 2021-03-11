using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace YueQianQian.WebApi
{

    public class NLogMiddleware
    {
        private readonly RequestDelegate _next;
        /// <summary>
        /// 日志接口
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private Stopwatch _stopwatch;
        private IWebHostEnvironment _env;

        public NLogMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _stopwatch = new Stopwatch();
            _next = next;
            _env = env;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            // 过滤，只有接口
            if (context.Request.Path.Value.ToLower().Contains("api"))
            {
                context.Request.EnableBuffering();
                Stream originalBody = context.Response.Body;

                _stopwatch.Restart();

                // 获取 Api 请求内容
                var requestContent = await GetRequesContent(context);


                // 获取 Api 返回内容
                using (var ms = new MemoryStream())
                {
                    context.Response.Body = ms;

                    await _next(context);
                    ms.Position = 0;

                    await ms.CopyToAsync(originalBody);
                }

                context.Response.Body = originalBody;

                _stopwatch.Stop();

                if (_env.IsDevelopment())
                {
                    //var eventInfo = new LogEventInfo();
                    //eventInfo.Message = "Success";
                    //eventInfo.Properties["Elapsed"] = _stopwatch.ElapsedMilliseconds;
                    //eventInfo.Properties["RequestBody"] = requestContent;
                    string trace = $"请求:{requestContent} {Environment.NewLine}返回:{context.Connection}{Environment.NewLine}耗时:{_stopwatch.ElapsedMilliseconds}ms";

                    logger.Trace(trace);
                }

            }
            else
            {
                await _next(context);
            }
        }

        private async Task<string> GetRequesContent(HttpContext context)
        {
            var request = context.Request;
            var sr = new StreamReader(request.Body);

            var content = $"{await sr.ReadToEndAsync()}";

            if (!string.IsNullOrEmpty(content))
            {
                request.Body.Position = 0;
            }

            return content;
        }
    }
}
