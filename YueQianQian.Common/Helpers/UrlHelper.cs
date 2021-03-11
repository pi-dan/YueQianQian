using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YueQianQian.Common.Helpers
{
    /// <summary>
    /// 获取网页相关信息
    /// </summary>
    public class UrlHelper
    {
        public static String GetWebTitle(String url)
        {
            //请求资源
            System.Net.WebRequest wb = System.Net.WebRequest.Create(url.Trim());

            //响应请求
            WebResponse webRes = null;

            //将返回的数据放入流中
            Stream webStream = null;
            try
            {
                webRes = wb.GetResponse();
                webStream = webRes.GetResponseStream();
            }
            catch (Exception e)
            {
                return "";
            }


            //从流中读出数据
            StreamReader sr = new StreamReader(webStream, System.Text.Encoding.Default);

            //创建可变字符对象，用于保存网页数据 
            StringBuilder sb = new StringBuilder();

            //读出数据存入可变字符中
            String str = "";
            while ((str = sr.ReadLine()) != null)
            {
                sb.Append(str);
            }

            //建立获取网页标题正则表达式
            String regex = @"<title>.+</title>";

            //返回网页标题
            String title = Regex.Match(sb.ToString(), regex).ToString();
            title = Regex.Replace(title, @"[\""]+", "");
            title = title.Replace("<title>", "").Replace("</title>", "").Trim();
            return title;
        }
    }
}
