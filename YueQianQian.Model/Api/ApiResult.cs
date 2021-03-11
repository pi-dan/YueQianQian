using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YueQianQian.Model
{
    /// <summary>
    /// 统一接口返回
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult
    {

        public ApiResult()
        {
            code = 0;
        }

        /// <summary>
        /// 请求状态
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 返回时间戳
        /// </summary>
        public string Timestamp { get; set; } = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

    }
    public class ApiResult<T> : ApiResult
    {
        /// <summary>
        /// 接口返回值
        /// </summary>
        public T data;

    }
}
