using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YueQianQian.Common.Auth
{
    public interface ICurrentUser
    {
        /// <summary>
        /// 主键
        /// </summary>
        int Id { get; }
        /// <summary>
        /// 用户名
        /// </summary>
        string UserName { get; }
        /// <summary>
        /// 邮箱
        /// </summary>
        string Email { get; }
        /// <summary>
        /// 是否管理员
        /// </summary>
        string IsAdmin { get; }
    }
}
