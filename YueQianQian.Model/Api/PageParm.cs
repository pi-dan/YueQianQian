
using System;
using System.Collections.Generic;
using System.Text;

namespace YueQianQian.Model
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class PageParm
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页总条数
        /// </summary>
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// 是否升序 
        /// </summary>
        public bool Ascending { get; set; } = true;

    }
}
