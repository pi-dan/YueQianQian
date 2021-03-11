using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YueQianQian.Model
{
    /// <summary>
    /// 枚举扩展属性
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// 获得枚举提示文本
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetEnumText(this Enum obj)
        {
            Type type = obj.GetType();
            FieldInfo field = type.GetField(obj.ToString());
            TextAttribute attribute = (TextAttribute)field.GetCustomAttribute(typeof(TextAttribute));
            return attribute.Value;
        }
    }
    public class TextAttribute : Attribute
    {
        public TextAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
    }
    public enum StatusCodeType
    {
        [Text("")]
        Success = 0,
        [Text("ArgumentException")]
        ArgumentException = 201,
        [Text("内部请求出错")]
        Error = 500,

        [Text("当前 Token 已失效, 请重新登陆")]
        Unauthorized = 401,

        [Text("请求参数不完整或不正确")]
        ParameterError = 400,

        [Text("您无权进行此操作，请求执行已拒绝")]
        Forbidden = 403,

        [Text("找不到与请求匹配的 HTTP 资源")]
        NotFound = 404,

        [Text("HTTP请求类型不合法")]
        HttpMehtodError = 405,

        [Text("HTTP请求不合法,请求参数可能被篡改")]
        HttpRequestError = 406,

        [Text("该URL已经失效")]
        URLExpireError = 407,

        [Text("请求错误")]
        RequestError = 407,


        [Text("默认值")]
        Default = 1000,
    }
}
