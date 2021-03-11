using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YueQianQian.Service.Bookmark
{
    /// <summary>
    /// 搜索书签返回的实体
    /// </summary>
    public class BmSearchDto
    {

        public int Id { get; set; }

        public short? ClickCount { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime? LastClick { get; set; }

        public string Title { get; set; } = string.Empty;


        public string Url { get; set; } = string.Empty;

        public string Favicon { get; set; } = string.Empty;

        /// <summary>
        /// 书签的路径
        /// </summary>
        public string Path { get; set; }
    }
}
