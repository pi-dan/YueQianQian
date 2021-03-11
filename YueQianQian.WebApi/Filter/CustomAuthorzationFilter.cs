using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YueQianQian.WebApi
{
    /// <summary>
    /// 自定义鉴权 判断jwt是否失效，jwt鉴权通过后会进入此实例
    /// </summary>
    public class CustomAuthorzationFilter : IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //此处添加判判断jwt是否已经失效，配合redies使用
            //context.Result = new JsonResult(new { StatusCodeResult = "401", Title = "401", Time = DateTime.Now, test = "测试" });
        }
    }
}
