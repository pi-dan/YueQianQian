
using System;
using System.Collections.Generic;
using System.Linq;

namespace YueQianQian.Model
{
    /// <summary>
    /// 分页组件实体类
    /// </summary>
    /// <typeparam name="T">泛型实体</typeparam>

    [Serializable]
    public class PagedInfo<T>
    {

        /// <summary>
        /// 分页索引
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages
        {
            get
            {
                if (PageSize == 0)
                    return 0;
                int total = TotalCount / PageSize;
                if (total % PageSize > 0)
                    total++;
                return total;
            }
        }

        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool HasPreviousPage
        {
            get { return PageIndex > 1; }
        }
        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNextPage
        {
            get { return PageIndex < TotalPages; }
        }

        public IEnumerable<T> DataSource { get; set; }

    }

}