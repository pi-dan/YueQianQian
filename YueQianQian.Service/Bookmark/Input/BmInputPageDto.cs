using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YueQianQian.Model;

namespace YueQianQian.Service.Bookmark
{
    /// <summary>
    /// 书签分页查询 输入实体
    /// </summary>
    public class BmInputPageDto : PageParm
    {
        /// <summary>
        /// 目录id
        /// </summary>
        public int TagId { get; set; }
    }
}
