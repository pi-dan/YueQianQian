using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YueQianQian.Model;

namespace YueQianQian.WebApi.Controllers
{
    public class BaseController : ControllerBase
    {
        #region 统一返回封装

        /// <summary>
        /// 返回封装
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static JsonResult toResponse(bool success)
        {
            ApiResult<bool> response = new ApiResult<bool>();
            if (success)
            {
                response.code = (int)StatusCodeType.Success;
            }
            else
            {
                response.code = (int)StatusCodeType.Error;
            }

            response.data = success;

            return new JsonResult(response);
        }
        /// <summary>
        /// 返回封装
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static JsonResult toResponse(StatusCodeType statusCode)
        {
            ApiResult response = new ApiResult();
            response.code = (int)statusCode;
            response.msg = statusCode.GetEnumText();

            return new JsonResult(response);
        }

        /// <summary>
        /// 返回封装
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="retMessage"></param>
        /// <returns></returns>
        public static JsonResult toResponse(StatusCodeType statusCode, string retMessage)
        {
            ApiResult response = new ApiResult();
            response.code = (int)statusCode;
            response.msg = retMessage;

            return new JsonResult(response);
        }
        /// <summary>
        /// 返回封装
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="retMessage"></param>
        /// <returns></returns>
        public static JsonResult toResponse(int statusCode, string retMessage)
        {
            ApiResult response = new ApiResult();
            response.code = statusCode;
            response.msg = retMessage;

            return new JsonResult(response);
        }
        /// <summary>
        /// 返回封装
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static JsonResult toResponse<T>(T data)
        {
            ApiResult<T> response = new ApiResult<T>();
            response.code = (int)StatusCodeType.Success;
            response.msg = StatusCodeType.Success.GetEnumText();
            response.data = data;
            return new JsonResult(response);
        }

        #endregion
    }
}
