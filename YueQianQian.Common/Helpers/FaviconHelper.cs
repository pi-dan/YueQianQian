using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YueQianQian.Common.Extensions;

namespace YueQianQian.Common.Helpers
{
    public class FaviconHelper
    {
        /// <summary>
        /// 获取页面跳转后的地址
        /// </summary>
        /// <param name="cahxunurl"></param>
        /// <returns></returns>
        public static string GetWebJumpUrl(string cahxunurl)
        {
            string url = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(cahxunurl);
            req.Method = "HEAD";
            req.AllowAutoRedirect = false;
            req.Timeout = 1000;
            HttpWebResponse myResp = (HttpWebResponse)req.GetResponse();
            if (myResp.StatusCode == HttpStatusCode.Redirect)
            { url = myResp.GetResponseHeader("Location"); }
            return url;
        }
        /// <summary>
        /// 根据网址拼接页面的的favicon地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static string TogetherFaviconAddr(string address)
        {
            string addtemp = address;
            var url = new StringBuilder();
            if (addtemp.StartsWith("https://"))
            {
                url.Append("https://");
                addtemp = addtemp.Replace("https://", "");
            }
            if (addtemp.StartsWith("http://"))
            {
                url.Append("http://");
                addtemp = addtemp.Replace("http://", "");
            }
            if (addtemp.Contains("/"))
            {
                url.Append(addtemp.Split('/')[0]);
            }
            else
            {
                url.Append(addtemp);
            }
            url.Append("/favicon.ico");

            return url.ToString();
        }
        /// <summary>
        /// 根据网址计算favicon地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static string GetFaviconAddr(string address)
        {
            string addr = address;

            try
            {
                //addr = GetWebJumpUrl(address);
                //if (addr.IsNull())
                //{
                //    addr = address;
                //}
                return TogetherFaviconAddr(addr);
            }
            catch
            {

            }

            return "";
        }

    }
}
